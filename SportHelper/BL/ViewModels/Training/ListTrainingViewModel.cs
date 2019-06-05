using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.BL.Model;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;
using SportHelper.Helpers;
using Xamarin.Forms;
using SportHelper.BL.ViewModels;
using System;
using SQLite;
using SportHelper.BL.DB;

namespace SportHelper.BL.ViewModels.Training {

	public class ListTrainingViewModel : BaseViewModel {
		DataBaseConnection _dataBase = new DataBaseConnection();

		public ICommand CreateNewTraining => MakeCommand(CreateNewTrainingExecute);
		public ICommand EditTraining => MakeCommand(EditTrainingExecute);
		public ICommand GoToStartTraining => MakeCommand(GoToStartTrainingExecute);
		public ICommand SampleCommand => MakeCommand(OnSampleCommand);

		public ObservableCollection<TrainingTable> TrainingList {
			get => Get<ObservableCollection<TrainingTable>>();
			set => Set(value);
		}

		public TrainingTable SelectTraining {
			get => Get<TrainingTable>();
			set => Set(value);
		}

		public ListTrainingViewModel() {

			TrainingList = new ObservableCollection<TrainingTable>();
			var tmp = _dataBase.db.Query<TrainingTable>("SELECT * FROM TrainingTable");
			TrainingList.Clear();
			foreach(var training in tmp) {
				TrainingList.Add(new TrainingTable {Id = training.Id,
					Id_account = training.Id_account,
					NameTraining = training.NameTraining,
				});
			}
		}

		

		

		void EditTrainingExecute() {
			if (SelectTraining != null) {
				var currentUser = _dataBase.GetUser();
				var selectedTraning = _dataBase.db.Query<TrainingTable>("SELECT * FROM TrainingTable " +
																		"WHERE NameTraining = '" + SelectTraining.NameTraining + "' AND id_account = " + currentUser.Id_account);
				_dataBase.db.Execute("UPDATE CurrentUserTable SET id_training = " + selectedTraning[0].Id + " WHERE id_user = 1");
				NavigateTo(AppPages.EditTraining);
			}
			else {
				ShowAlert("","Выберите тренировку из списка, которую хотите изменить","OK");
			}	
		}

		void CreateNewTrainingExecute() {
			_dataBase.db.Execute("UPDATE CurrentUserTable SET id_training = 0 WHERE id_user = 1");
			NavigateTo(AppPages.EditTraining);
		}

		void GoToStartTrainingExecute() {
			/*_dataBase.db.Execute("INSERT INTO TrainingTable (NameTraining, id_account) VALUES ('Понедельник', 1)");
			_dataBase.db.Execute("INSERT INTO TrainingTable (NameTraining, id_account) VALUES ('Среда', 1)");
			_dataBase.db.Execute("INSERT INTO ExerciseTable (NameExercise, TimePrepare, TimeRest, TimeWorking, Circle, id_training) VALUES ('Ноги', 5, 15, 20, 2, 1)");
			_dataBase.db.Execute("INSERT INTO ExerciseTable (NameExercise, TimePrepare, TimeRest, TimeWorking, Circle, id_training) VALUES ('Плечи', 7, 10, 15, 3, 1)");
			_dataBase.db.Execute("INSERT INTO ExerciseTable (NameExercise, TimePrepare, TimeRest, TimeWorking, Circle, id_training) VALUES ('Грудь', 10, 8, 15, 3, 2)");
			_dataBase.db.Execute("INSERT INTO ExerciseTable (NameExercise, TimePrepare, TimeRest, TimeWorking, Circle, id_training) VALUES ('Спина', 7, 10, 15, 3, 2)");*/
			if (SelectTraining != null) {
				var currUser = _dataBase.GetUser();
				var currTrain = _dataBase.db.Query<TrainingTable>("Select * From TrainingTable Where NameTraining = '" + SelectTraining.NameTraining + "'");
				_dataBase.db.Execute("UPDATE CurrentUserTable SET id_training = " + currTrain[0].Id + "  Where id_user = 1");
				NavigateTo(AppPages.StartTraining);
			}
		}

		void OnSampleCommand() {
			ShowAlert("", "Вы выбрали элемент", "Ok");
		}
	}
}
