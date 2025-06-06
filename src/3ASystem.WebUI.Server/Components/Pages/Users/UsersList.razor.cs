using _3ASystem.WebUI.Server.Components._Shared;
using _3ASystem.WebUI.Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text;
using System.Text.Encodings.Web;

namespace _3ASystem.WebUI.Server.Components.Pages.Users
{
	public partial class UsersList
	{
		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Inject]
		public IDialogService DialogService { get; set; } = default!;

		[Inject]
		public ISnackbar Snackbar { get; set; } = default!;

		[Inject]
		IEmailSender<ApplicationUser> _EmailSender { get; set; } = default!;

		[Inject]
		UserManager<ApplicationUser> _UserManager { get; set; } = default!;

		[Inject]
		RoleManager<IdentityRole> _RoleManager { get; set; } = default!;

		[Inject]
		AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;


		[CascadingParameter]
		private Task<AuthenticationState> authenticationStateTask { get; set; } = default!;
		System.Security.Claims.ClaimsPrincipal CurrentUser = default!;

		string ADMIN_EMAIL = "admin@svcode.com.br";
		string ADMINISTRATION_ROLE = "Administrators";
		string USER_ROLE = "Users";


		private string _error = string.Empty;
		private bool isLoading = true;
		private List<ApplicationUser> _records = [];

		private int _totalOfRecords = 0;
		private int _selectedPage = 1;
		private int _pageSize = 10;
		private int _totalOfPages = 1;

		private IDialogReference dlg = default!;

		protected override async Task OnInitializedAsync()
		{
			// ensure there is a ADMINISTRATION_ROLE
			var RoleResult = await _RoleManager.FindByNameAsync(ADMINISTRATION_ROLE);
			if (RoleResult == null)
			{
				// Create ADMINISTRATION_ROLE Role
				await _RoleManager.CreateAsync(new IdentityRole(ADMINISTRATION_ROLE));
			}

			//Ensure there is a USER_ROLE
			var UserRoleResult = await _RoleManager.FindByNameAsync(USER_ROLE);
			if (UserRoleResult == null)
			{
				// Create ADMINISTRATION_ROLE Role
				await _RoleManager.CreateAsync(new IdentityRole(USER_ROLE));
			}

			// Ensure a user named admin@svcode.com.br exists in both roles
			var user = await _UserManager.FindByNameAsync(ADMIN_EMAIL);
			if (user != null)
			{
				// Is admin@svcode.com.br in administrators role?
				var UserResult = await _UserManager.IsInRoleAsync(user, ADMINISTRATION_ROLE);
				if (!UserResult)
				{
					// Put admin in Administrators role
					await _UserManager.AddToRoleAsync(user, ADMINISTRATION_ROLE);
				}

				// Is admin@svcode.com.br in users role?
				UserResult = await _UserManager.IsInRoleAsync(user, USER_ROLE);
				if (!UserResult)
				{
					// Put admin in Users role
					await _UserManager.AddToRoleAsync(user, USER_ROLE);
				}
			}

			// Get the current logged in user
			CurrentUser = (await authenticationStateTask).User;

			await FetchData();
		}

		private async Task<bool> FetchData()
		{
			isLoading = true;

			// get users from _UserManager
			var users = _UserManager.Users.Select(x => new ApplicationUser
			{
				Id = x.Id,
				UserName = x.UserName,
				FullName = x.FullName,
				Email = x.Email,
				EmailConfirmed = x.EmailConfirmed,
				PasswordHash = "*****"
			}).OrderBy(ob => ob.FullName).ToList();

			if (users is not null)
				_records = [.. users];


			_totalOfRecords = _records.Count();


			isLoading = false;

			return await Task.FromResult(true);
		}

		private async Task CreateUser()
		{
			var parameters = new DialogParameters {
				{ nameof(ModalComponent.Icon), Icons.Material.Filled.AppRegistration},
				{ nameof(ModalComponent.Title), "Register New User" },
				{ nameof(ModalComponent.Body), true },
				{ nameof(ModalComponent.SubmitText), "Save"},
				{ nameof(ModalComponent.ShowActionButtons), false }
			};

			parameters.Add(nameof(ModalComponent.Body), (RenderFragment)(builder =>
			{
				builder.OpenComponent<UserCreateForm>(0);
				builder.AddComponentParameter(1,
					nameof(UserCreateForm.OnSaveClickSuccess),
					EventCallback.Factory.Create(this, async (string successMessage) =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());

						Snackbar.Clear();
						Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
						Snackbar.Add(successMessage, Severity.Success);

						await FetchData();
						StateHasChanged();
					})
				);
				builder.AddComponentParameter(2,
					nameof(UserCreateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);
				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Register New User", parameters, options);
		}


		private async Task ResendConfirmationEmail(string identityUserId)
		{
			if (string.IsNullOrEmpty(identityUserId)) return;

			var _user = await _UserManager.FindByIdAsync(identityUserId);

			if (_user is not null)
			{
				var code = await _UserManager.GenerateEmailConfirmationTokenAsync(_user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				var callbackUrl = Navigation.GetUriWithQueryParameters(
					Navigation.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
					new Dictionary<string, object?>
					{
						["userId"] = identityUserId,
						["code"] = code,
						["returnUrl"] = Navigation.ToAbsoluteUri("Account/Login").ToString()
					}
				);

				await _EmailSender.SendConfirmationLinkAsync(_user, _user.Email!, HtmlEncoder.Default.Encode(callbackUrl));

				Snackbar.Clear();
				Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
				Snackbar.Add($"A confirmation email was sent to {_user.Email}.", Severity.Info);

			}

		}

		private async Task EditUser(string identityUserId)
		{
			var parameters = new DialogParameters {
				{ nameof(ModalComponent.Icon), Icons.Material.Filled.AppRegistration},
				{ nameof(ModalComponent.Title), "Edit User" },
				{ nameof(ModalComponent.Body), true },
				{ nameof(ModalComponent.SubmitText), "Update"},
				{ nameof(ModalComponent.ShowActionButtons), false }
			};

			parameters.Add(nameof(ModalComponent.Body), (RenderFragment)(builder =>
			{
				builder.OpenComponent<UserUpdateForm>(0);
				builder.AddComponentParameter(1, nameof(UserUpdateForm.UserId), identityUserId);
				builder.AddComponentParameter(2,
					nameof(UserUpdateForm.OnSaveClickSuccess),
					EventCallback.Factory.Create(this, async (string successMessage) =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());

						Snackbar.Clear();
						Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
						Snackbar.Add(successMessage, Severity.Success);

						await FetchData();
						StateHasChanged();
					})
				);
				builder.AddComponentParameter(3,
					nameof(UserUpdateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);
				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Edit User", parameters, options);
		}

		private async Task AskConfirmDelete(string identityUserId)
		{
			var parameters = new DialogParameters<ConfirmDialogComponent>
			{
				{ x => x.ContentText, "Do you really want to delete this record? This process cannot be undone." },
				{ x => x.ButtonText, "Delete" },
				{ x => x.Color, Color.Error }
			};

			var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, CloseButton = true, CloseOnEscapeKey = true, BackdropClick = false };

			var dialog = await DialogService.ShowAsync<ConfirmDialogComponent>("Delete", parameters, options);
			var result = await dialog.Result;
			if (result is not null && result.Canceled is false)
			{
				var data = (bool)result.Data!;
				if (data)
				{
					await DeleteUser(identityUserId);
				}
			}


		}

		private async Task DeleteUser(string identityUser)
		{
			var user = await _UserManager.FindByIdAsync(identityUser);
			if (user != null)
			{
				// Delete the user
				var userName = user.FullName;
				var result = await _UserManager.DeleteAsync(user);
				if (!result.Succeeded)
				{
					var errorMessage = string.Join(" - ", result.Errors.Select(error => error.Description));
					Snackbar.Clear();
					Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
					Snackbar.Add(errorMessage, Severity.Error);
				}

				var successMessage = $"User [{userName}] successfully deleted.";
				Snackbar.Clear();
				Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
				Snackbar.Add(successMessage, Severity.Success);

				await FetchData();
				StateHasChanged();
			}
		}


	}
}