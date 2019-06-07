using System.Windows.Input;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;



namespace SportHelper.BL.ViewModels.Account {
	class RegisterViewModel : BaseViewModel {


		public ICommand UserRegister => new Command(execute: async () => {
			if (!string.IsNullOrEmpty(LoginReg) && !string.IsNullOrEmpty(PasswordReg)) {
				var logins = await DataServices.SportHelperDataService.GetAccountAsync("SELECT * FROM AccountTable WHERE Login like '" + LoginReg + "'", CancellationToken);
				if (logins.Status == DAL.RequestStatus.Ok) {
					if (logins.Data.Count == 0) {
						await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO AccountTable (Login, Password) VALUES ('" + LoginReg + "', '" + PasswordReg + "')", CancellationToken);
						var NewUser = await DataServices.SportHelperDataService.GetAccountAsync("SELECT * FROM AccountTable ORDER BY id_account DESC LIMIT 1", CancellationToken);
						await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET id_account = " + NewUser.Data[0].Id + " WHERE id_user = 1", CancellationToken);
						NavigateTo(AppPages.MainMenu);
					}
					else {
						await ShowAlert("", "Пользователь с таким Логином зарегестрирован", "Ok");
					}
				}
			}
			else {
				await ShowAlert("", "Заполните поля", "OK");
			}
		});

		public string LoginReg {
			get => Get<string>();
			set => Set(value);
		}

		public string PasswordReg {
			get => Get<string>();
			set => Set(value);
		}

	}
}
