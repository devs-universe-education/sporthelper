using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;

namespace SportHelper.BL.ViewModels.Training {

	public class ListTrainingViewModel : BaseViewModel {

		public ICommand CreateNewTraining => new Command(execute: async () => {
			await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET id_training = 0 WHERE id_user = 1", CancellationToken);
			NavigateTo(AppPages.EditTraining);
		});

		public ICommand EditTraining => new Command(execute: async () => {
			if (SelectTraining != null) {
				var selectedTraining = await DataServices.SportHelperDataService.GetTrainingAsync("SELECT * FROM TrainingTable " +
																		"WHERE NameTraining = '" + SelectTraining.NameTraining + "' AND id_account = " + _currUser.Id_account, CancellationToken);
				await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET id_training = " + selectedTraining.Data[0].Id + " WHERE id_user = 1", CancellationToken);
				NavigateTo(AppPages.EditTraining);
			}
			else {
				await ShowAlert("", "Выберите тренировку из списка, которую хотите изменить", "OK");
			}
		});

		public ICommand GoToStartTraining => new Command(execute: async () => {
			if (SelectTraining != null) {
				var currTraining = await DataServices.SportHelperDataService.GetTrainingAsync("Select * From TrainingTable Where NameTraining = '" + SelectTraining.NameTraining + "'", CancellationToken);
				await DataServices.SportHelperDataService.ExecuteAsync("UPDATE CurrentUserTable SET id_training = " + currTraining.Data[0].Id + "  Where id_user = 1", CancellationToken);
				NavigateTo(AppPages.StartTraining);
			}
			else {
				await ShowAlert("", "Выберите тренировку", "Ok");
			}
		});

		public ICommand DeleteTraining => new Command(execute: async () => {
			if (SelectTraining != null) {
				await DataServices.SportHelperDataService.DeleteTrainingAsync(SelectTraining.Id, CancellationToken);
				var tmp = await DataServices.SportHelperDataService.GetTrainingAsync("SELECT * FROM TrainingTable Where id_account = " + _currUser.Id_account, CancellationToken);
				TrainingList.Clear();
				foreach (var item in tmp.Data) {
					TrainingList.Add(new TrainingDataObject {
						Id = item.Id,
						Id_account = item.Id_account,
						NameTraining = item.NameTraining
					});
				}
			}
			else {
				await ShowAlert("", "Выберите тренировку, которое хотите удалить", "Ok");
			}
		});

		public ObservableCollection<TrainingDataObject> TrainingList {
			get => Get<ObservableCollection<TrainingDataObject>>();
			set => Set(value);
		}

		public TrainingDataObject SelectTraining {
			get => Get<TrainingDataObject>();
			set => Set(value);
		}

		CurrentUserDataObject _currUser;

		public async override Task OnPageAppearing() {
			var currUser = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			_currUser = currUser.Data[0];
			var tmp = await DataServices.SportHelperDataService.GetTrainingAsync("SELECT * FROM TrainingTable Where id_account = " + _currUser.Id_account , CancellationToken);
			TrainingList.Clear();
			foreach (var item in tmp.Data) {
				TrainingList.Add(new TrainingDataObject {
					Id = item.Id,
					Id_account = item.Id_account,
					NameTraining = item.NameTraining
				});
			}
		}

		public ListTrainingViewModel() {
			TrainingList = new ObservableCollection<TrainingDataObject>();
		}
	}
}
