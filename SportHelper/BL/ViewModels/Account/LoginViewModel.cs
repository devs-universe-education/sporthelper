using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.BL.DB;
using SportHelper.BL.Model;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Account {
	class LoginViewModel : BaseViewModel {

		DataBaseConnection _dataBase = new DataBaseConnection();

		public ICommand GoToRegister => MakeCommand(GoToRegisterExecute);
		public ICommand GoToMainMenu => MakeCommand(GoToMainMenuExecute);
		public ICommand GetUserAutorisation => MakeCommand(GetUserAutorisationExecute);

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

		public override Task OnPageAppearing() {
			var check = _dataBase.db.Query<CurrentUserTable>("Select * From CurrentUserTable");
			if (check.Count == 1) {
				if (check[0].Remember == true) {
					NavigateTo(AppPages.MainMenu);
				}
			}
			return base.OnPageAppearing();
		}


		void GoToRegisterExecute() {

			NavigateTo(AppPages.Register);
		}

		void GoToMainMenuExecute() {

			NavigateTo(AppPages.MainMenu);
		}

		void GetUserAutorisationExecute() {

			if (!string.IsNullOrEmpty(PasswordAuto) && !string.IsNullOrEmpty(LoginAuto)) {

				var logins = _dataBase.db.Query<AccountTable>("SELECT * FROM AccountTable Where Login like '" + LoginAuto + "' AND Password = '" + PasswordAuto + "'");
				if (logins.Count == 1) {

					_dataBase.db.Execute("UPDATE CurrentUserTable SET id_account = " + logins[0].Id + " WHERE id_user = 1");
					_dataBase.db.Execute("UPDATE CurrentUserTable SET Remember = " + (CheckRemember ? 1 : 0).ToString() + " WHERE id_user = 1");
					var check = _dataBase.db.Query<CurrentUserTable>("Select * From CurrentUserTable");
					NavigateTo(AppPages.MainMenu);
				}
				else
					ShowAlert("", "Неверно введен Логин или Пароль", "OK");

			}
			else {
				ShowAlert("", "Заполните поля", "OK");
			}
		}
	}
}
