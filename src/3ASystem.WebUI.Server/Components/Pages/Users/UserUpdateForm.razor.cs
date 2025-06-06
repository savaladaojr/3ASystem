using _3ASystem.Domain.Shared;
using _3ASystem.WebUI.Server.Components.Account;
using _3ASystem.WebUI.Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;

namespace _3ASystem.WebUI.Server.Components.Pages.Users
{
	public partial class UserUpdateForm : ComponentBase
	{
		[Inject]
		UserManager<ApplicationUser> UserManager { get; set; } = default!;

		[Inject]
		RoleManager<IdentityRole> RoleManager { get; set; } = default!;

		[Inject]
		IUserStore<ApplicationUser> UserStore { get; set; } = default!;

		[Inject]
		SignInManager<ApplicationUser> SignInManager { get; set; } = default!;

		[Inject]
		IEmailSender<ApplicationUser> EmailSender { get; set; } = default!;
		[Inject]
		ILogger<UserCreateForm> Logger { get; set; } = default!;

		[Inject]
		NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		IdentityRedirectManager RedirectManager { get; set; } = default!;


		[Parameter]
		public EventCallback<string> OnSaveClickSuccess { get; set; }
		[Parameter]
		public EventCallback OnCancelClick { get; set; }


		[Parameter]
		public string UserId { get; set; } = default!;
		private ApplicationUser? _user = default!;


		string ADMIN_EMAIL = "admin@svcode.com.br";
		string ADMINISTRATION_ROLE = "Administrators";
		string USER_ROLE = "Users";
		List<string> ROLES = ["Administrators", "Users"];


		private IEnumerable<IdentityError>? identityErrors;

		private EditForm _frm { get; set; } = default!;
		private ValidationMessageStore validationMessageStore = default!;

		[SupplyParameterFromForm]
		private InputModel Input { get; set; } = new();

		[SupplyParameterFromQuery]
		private string? ReturnUrl { get; set; }

		private string? Message => identityErrors is null ? null : $"Error: {string.Join(" - ", identityErrors.Select(error => error.Description))}";

		private bool readoOnly = false;
		private bool IsEmailFieldDisabled => readoOnly || (_user?.EmailConfirmed ?? false);

		protected override async Task OnInitializedAsync()
		{
			await Start();

		}

		private async Task<bool> Start()
		{
			if (!string.IsNullOrEmpty(UserId))
			{
				_user = await UserManager.FindByIdAsync(UserId);
				if (_user != null)
				{
					readoOnly = false;

					Input.FullName = _user.FullName;
					Input.PhoneNumber = _user.PhoneNumber;
					Input.Email = _user.Email;

					var roles = await UserManager.GetRolesAsync(_user);

					Input.SelectedRoles = new ReadOnlyCollection<string>(roles);

					if (_user.UserName == ADMIN_EMAIL)
						readoOnly = true;


					validationMessageStore ??= new(_frm.EditContext);
					_frm.EditContext!.OnValidationRequested += (sender, args) =>
					{
						validationMessageStore.Clear();
						_frm.EditContext.NotifyValidationStateChanged();
					};
				}
			}

			var task = await Task.FromResult(true);
			return task;
		}

		public async Task UpdateUser(EditContext editContext)
		{
			if (_user is null) return;

			var sendEmailConfirmation = false;

			if ((!_user.EmailConfirmed) && (_user.Email != Input.Email))
			{
				//var emailStore = GetEmailStore();
				//await emailStore.SetEmailAsync(_user, Input.Email, CancellationToken.None);

				// Update Email
				_user.Email = Input.Email;
				_user.UserName = Input.Email!.ToLower();
				sendEmailConfirmation = true;
			}

			if (_user.FullName != Input.FullName)
			{
				_user.FullName = Input.FullName;
			}

			// Update the user
			var result = await UserManager.UpdateAsync(_user);
			if (!result.Succeeded)
			{
				AddIdentityResultErrorsToEditContext(editContext, result);
				identityErrors = result.Errors;
				return;
			}

			/*Change Password Rule*/
			/*
			// Only update password if the current value
			// is not the default value
			if (objUser.PasswordHash != "*****")
			{
				var resetToken =
					await _UserManager.GeneratePasswordResetTokenAsync(user);
				var passworduser =
					await _UserManager.ResetPasswordAsync(
						user,
						resetToken,
						objUser.PasswordHash);
				if (!passworduser.Succeeded)
				{
					if (passworduser.Errors.FirstOrDefault() != null)
					{
						strError =
							passworduser
							.Errors
							.FirstOrDefault()
							.Description;
					}
					else
					{
						strError = "Pasword error";
					}
					// Keep the popup opened
					return;
				}
			}
			*/


			//Roles
			var userRoles = await UserManager.GetRolesAsync(_user);
			await UserManager.RemoveFromRolesAsync(_user, userRoles);		

			foreach (string role in Input.SelectedRoles)
			{
				var roleResult = await RoleManager.FindByNameAsync(role);
				if (roleResult is not null)
					await UserManager.AddToRoleAsync(_user, roleResult.Name!);
			}

			Logger.LogInformation("User was updated successfully.");

			if ((UserManager.Options.SignIn.RequireConfirmedAccount) && (sendEmailConfirmation))
			{
				var userId = _user.Id;
				var code = await UserManager.GenerateEmailConfirmationTokenAsync(_user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				var callbackUrl = NavigationManager.GetUriWithQueryParameters(
					NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
					new Dictionary<string, object?>
					{
						["userId"] = userId,
						["code"] = code,
						["returnUrl"] = (ReturnUrl is null) ? NavigationManager.ToAbsoluteUri("Account/Login").ToString() : ReturnUrl
					}
				);

				await EmailSender.SendConfirmationLinkAsync(_user, _user.Email!, HtmlEncoder.Default.Encode(callbackUrl));
			}

			// Call the parent method via the EventCallback
			await OnSaveClickSuccess.InvokeAsync($"User [{_user!.FullName}] successfully updated.");
		}

		private void AddIdentityResultErrorsToEditContext(EditContext editContext, IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				switch (error.Code)
				{
					case "DuplicateUserName":
						validationMessageStore.Add(editContext.Field(nameof(Input.Email)), error.Description);
						break;

					default:
						break;
				}
			}

			editContext.NotifyValidationStateChanged();
		}

		private static ApplicationUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<ApplicationUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
					$"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
			}
		}

		private IUserEmailStore<ApplicationUser> GetEmailStore()
		{
			if (!UserManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<ApplicationUser>)UserStore;
		}

		private async Task HandleCancelAsync()
		{
			// Call the parent method via the EventCallback
			await OnCancelClick.InvokeAsync();
		}

		private sealed class InputModel
		{
			[Required]
			[Display(Name = "Full Name")]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
			public string FullName { get; set; } = "";

			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string? Email { get; set; } = "";

			[Display(Name = "Phone number")]
			[RegularExpression("^(?!0+$)\\+(\\d{1,2})\\s?\\(?\\d{1,3}\\)?\\s?\\d{1,5}[\\s\\.\\-]?\\d{1,4}$", ErrorMessage = "Please enter valid phone number. Ex: +01 999 9999-9999")]
			public string? PhoneNumber { get; set; }

			public IReadOnlyCollection<string> SelectedRoles { get; set; } = [];

		}
	}
}
