using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Account {
	class RegisterViewModel : BaseViewModel {
		public ICommand GoToMainMenu => MakeCommand(GoToMainMenuExecute);

		void GoToMainMenuExecute() {

			NavigateTo(AppPages.MainMenu);
		}

	}
}
