using _3ASystem.WebUI.Server.Components.Account;
using _3ASystem.WebUI.Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace _3ASystem.WebUI.Server.Components.Pages.Users
{
	public partial class UserCreateForm : ComponentBase
	{
		[Inject]
		UserManager<ApplicationUser> UserManager { get; set; } = default!;

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

		private IEnumerable<IdentityError>? identityErrors;

		private EditForm _frm { get; set; } = default!;
		private ValidationMessageStore validationMessageStore = default!;

		[SupplyParameterFromForm]
		private InputModel Input { get; set; } = new();

		[SupplyParameterFromQuery]
		private string? ReturnUrl { get; set; }

		private string? Message => identityErrors is null ? null : $"Error: {string.Join(" - ", identityErrors.Select(error => error.Description))}";

		private string USER_ROLE = "Users";

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				if (_frm?.EditContext != null)
				{
					validationMessageStore ??= new(_frm.EditContext);

					_frm.EditContext.OnValidationRequested += (sender, args) =>
					{
						validationMessageStore.Clear();
						_frm.EditContext.NotifyValidationStateChanged();
					};
				}
			}

			return Task.CompletedTask;
		}

		public async Task RegisterUser(EditContext editContext)
		{
			var user = CreateUser();

			user.FullName = Input.FullName;
			user.PhoneNumber = Input.PhoneNumber;

			await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

			var emailStore = GetEmailStore();
			await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

			//Persist the user to the database
			var result = await UserManager.CreateAsync(user, Input.Password);
			
			if (!result.Succeeded)
			{
				AddIdentityResultErrorsToEditContext(editContext, result);
				identityErrors = result.Errors;
				return;
			}

			// Put user in Users role
			await UserManager.AddToRoleAsync(user, USER_ROLE);

			Logger.LogInformation("User created a new account with initial password.");

			if (UserManager.Options.SignIn.RequireConfirmedAccount)
			{
				var userId = await UserManager.GetUserIdAsync(user);
				var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
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

				await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));
			}

			// Call the parent method via the EventCallback
			await OnSaveClickSuccess.InvokeAsync($"User [{user.FullName}] successfully created. An e-mail was sent for confirmation.");
		}

		private void AddIdentityResultErrorsToEditContext(EditContext editContext, IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				switch (error.Code)
				{
					case "PasswordRequiresLower":
						validationMessageStore.Add(editContext.Field(nameof(Input.Password)), error.Description);
						break;

					case "PasswordRequiresUpper":
						validationMessageStore.Add(editContext.Field(nameof(Input.Password)), error.Description);
						break;
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

		//private async Task HandleGeneratePassword()
		//{
		//	var password = Utils.PasswordGenerator.Generate(
		//		requiredLength: 8,
		//		requiredUniqueChars: 4,
		//		requireDigit: true,
		//		requireLowercase: true,
		//		requireNonAlphanumeric: false,
		//		requireUppercase: true
		//	);

		//	Input.Password = password;
		//	Input.ConfirmPassword = password;
		//}


		private sealed class InputModel
		{
			[Required]
			[Display(Name = "Full Name")]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
			public string FullName { get; set; } = "";

			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; } = "";

			[Display(Name = "Phone number")]
			[RegularExpression("^(?!0+$)\\+(\\d{1,2})\\s?\\(?\\d{1,3}\\)?\\s?\\d{1,5}[\\s\\.\\-]?\\d{1,4}$", ErrorMessage = "Please enter valid phone number. Ex: +01 999 9999-9999")]
			public string? PhoneNumber { get; set; }

			[Required]
			[StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; } = "";

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; } = "";

		}
	}
}
