using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Menu {
	class MainMenuViewModel : BaseViewModel {

		public ICommand GoToListTraining => MakeCommand(GoToListTrainingExecute);
		public ICommand GoToViewProfile => MakeCommand(GoToViewProfileExecute);
		public ICommand GoToAboutProgram => MakeCommand(GoToAboutProgramExecute);
		public ICommand GoToLogin => MakeCommand(GoToLoginExecute);

		void GoToListTrainingExecute() {
			NavigateTo(AppPages.ListTraining);
		}

		void GoToViewProfileExecute() {
			NavigateTo(AppPages.ViewProfile);
		}

		void GoToAboutProgramExecute() {
			NavigateTo(AppPages.AboutProgram);
		}

		void GoToLoginExecute() {
			NavigateTo(AppPages.Login);
		}

	}
}
