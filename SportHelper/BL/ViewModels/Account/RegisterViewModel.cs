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
	class RegisterViewModel : BaseViewModel {

		DataBaseConnection _dataBase = new DataBaseConnection();

		public ICommand GetUserRegister => MakeCommand(GetUserRegisterExecute);

		public string LoginReg {
			get => Get<string>();
			set => Set(value);
		}

		public string PasswordReg {
			get => Get<string>();
			set => Set(value);
		}

		void GetNewUser() {
			_dataBase.db.Execute("INSERT INTO AccountTable (Login, Password) VALUES ('" + LoginReg + "', '" + PasswordReg + "')");
			var NewUser = _dataBase.db.Query<AccountTable>("SELECT * FROM AccountTable ORDER BY id_account DESC LIMIT 1");
			_dataBase.db.Execute("UPDATE CurrentUserTable SET id_account = " + NewUser[0].Id + " WHERE id_user = 1");
		}


		void GetUserRegisterExecute() {

			var checkCompare = true;
			var logins = _dataBase.db.Query<AccountTable>("SELECT * FROM AccountTable");
			if (!string.IsNullOrEmpty(LoginReg) && !string.IsNullOrEmpty(PasswordReg)) {
				foreach (var l in logins) {

					if (l.Login.ToLower() != LoginReg.ToLower()) {
						checkCompare = true;

					}
					else {
						ShowAlert("", "Пользователь с таким Логином зарегестрирован", "Ok");
						checkCompare = false;
						break;
					}
				}

				if(checkCompare == true) {
					GetNewUser();
					NavigateTo(AppPages.MainMenu);
				}

			}
			else {
				ShowAlert("", "Заполните поля", "OK");
			}
		}
	}
}
