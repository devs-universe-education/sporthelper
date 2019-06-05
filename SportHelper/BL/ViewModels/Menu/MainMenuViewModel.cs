using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.BL.DB;
using SportHelper.BL.Model;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Menu {
	class MainMenuViewModel : BaseViewModel {

		DataBaseConnection a = new DataBaseConnection();

		public ICommand GoToListTraining => MakeCommand(GoToListTrainingExecute);
		public ICommand GoToViewProfile => MakeCommand(GoToViewProfileExecute);
		public ICommand GoToAboutProgram => MakeCommand(GoToAboutProgramExecute);
		public ICommand GoToLogin => MakeCommand(GoToLoginExecute);

		public MainMenuViewModel() {

				

		}

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

			a.db.Execute("UPDATE CurrentUserTable SET Remember = '0' WHERE id_user = 1");
			NavigateTo(AppPages.Login);
		}

	}
}
