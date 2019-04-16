using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Account {
	class ViewProfileViewModel : BaseViewModel {
		public ICommand GoToStatProfile => MakeCommand(GoToStatProfileExecute);
		void GoToStatProfileExecute() {
			NavigateTo(AppPages.StatProfile);
		}
	}
}
