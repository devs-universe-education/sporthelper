using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Training {
	class ListTrainingViewModel : BaseViewModel {


		public ICommand GoToEditTraining => MakeCommand(GoToEditTrainingExecute);
		public ICommand GoToStartTraining => MakeCommand(GoToStartTrainingExecute);

		void GoToEditTrainingExecute() {
			NavigateTo(AppPages.EditTraining);
		}

		void GoToStartTrainingExecute() {
			NavigateTo(AppPages.StartTraining);
		}
	}
}
