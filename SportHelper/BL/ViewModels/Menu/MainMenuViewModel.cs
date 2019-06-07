using System.Windows.Input;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;

namespace SportHelper.BL.ViewModels.Menu {
	class MainMenuViewModel : BaseViewModel {

		public ICommand GoToListTraining => MakeCommand(GoToListTrainingExecute);
		public ICommand GoToViewProfile => MakeCommand(GoToViewProfileExecute);
		public ICommand GoToAboutProgram => MakeCommand(GoToAboutProgramExecute);
		public ICommand GoToLogin => new Command(execute: async () => {
			await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET Remember = '0' WHERE id_user = 1", CancellationToken);
			NavigateTo(AppPages.Login);
		});

		void GoToListTrainingExecute() {
			NavigateTo(AppPages.ListTraining);
		}

		void GoToViewProfileExecute() {
			NavigateTo(AppPages.ViewProfile);
		}

		void GoToAboutProgramExecute() {
			NavigateTo(AppPages.AboutProgram);
		}
	}
}
