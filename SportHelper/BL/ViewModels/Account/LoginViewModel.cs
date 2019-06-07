using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;

namespace SportHelper.BL.ViewModels.Account {
	class LoginViewModel : BaseViewModel {
		
		public ICommand GoToRegister => MakeCommand(GoToRegisterExecute);
		public ICommand GoToMainMenu => MakeCommand(GoToMainMenuExecute);
		public ICommand UserAutorisation => new Command(execute: async () => {

			if (!string.IsNullOrEmpty(PasswordAuto) && !string.IsNullOrEmpty(LoginAuto)) {
				var logins = await DataServices.SportHelperDataService.GetAccountAsync("SELECT * FROM AccountTable Where Login like '" + LoginAuto + "' AND Password = '" + PasswordAuto + "'", CancellationToken);
				if (logins.Status == DAL.RequestStatus.Ok) {
					if (logins.Data.Count == 1) {
						await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET id_account = " + logins.Data[0].Id + " WHERE id_user = 1", CancellationToken);
						await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET Remember = " + (CheckRemember ? 1 : 0).ToString() + " WHERE id_user = 1", CancellationToken);
						NavigateTo(AppPages.MainMenu);
					}
					else {
						await ShowAlert("", "Неверно введен Логин или Пароль", "OK");
					}
				}
			}
			else {
				await ShowAlert("", "Заполните поля", "OK");
			}
		});

		public string LoginAuto {
			get => Get<string>();
			set => Set(value);
		}

		public string PasswordAuto {
			get => Get<string>();
			set => Set(value);
		}

		public bool CheckRemember {
			get => Get<bool>();
			set => Set(value);
		}

		public override async Task OnPageAppearing() {
			await DataServices.SportHelperDataService.CreateCurrentUserAsync(CancellationToken);
			var test = await DataServices.SportHelperDataService.GetCurrentUserAsync("Select * From CurrentUserTable", CancellationToken);
			if(test.Data[0].Remember == true) {
				NavigateTo(AppPages.MainMenu);
			}
		}


		void GoToRegisterExecute() {

			NavigateTo(AppPages.Register);
		}

		void GoToMainMenuExecute() {

			NavigateTo(AppPages.MainMenu);
		}
	}
}
