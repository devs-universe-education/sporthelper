using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;
using System.Linq;

namespace SportHelper.BL.ViewModels.Training {
	class EditTrainingViewModel : BaseViewModel {

		int _i = 10000;
		public int Prepare = 5;
		public int Relax = 5;
		public int Working = 5;

		public EditTrainingViewModel() {
			ExerciseList = new ObservableCollection<ExerciseDataObject>() { };
			ExerciseList.Clear();
		}

		public ExerciseDataObject SelectExercise {
			get => Get<ExerciseDataObject>();
			set => Set(value);
		}

		public ObservableCollection<ExerciseDataObject> ExerciseList {
			get => Get<ObservableCollection<ExerciseDataObject>>();
			set => Set(value);
		}

		public string  NameTraining{
			get => Get<string>();
			set => Set(value);
		}
		public string NameExercise {
			get => Get<string>();
			set => Set(value);
		}
		public string PreparationEntry {
			get => Get<string>();
			set => Set(value);
		}
		public string PerformanceEntry {
			get => Get<string>();
			set => Set(value);
		}
		public string RelaxationEntry {
			get => Get<string>();
			set => Set(value);
		}
		public string RepeatEntry {
			get => Get<string>();
			set => Set(value);
		}

		

		CurrentUserDataObject _currUser;

		

		public ICommand PreparationMinus => MakeCommand(PreparationMinusExecute);
		public ICommand PreparationPlus => MakeCommand(PreparationPlusExecute);
		public ICommand PerformanceMinus => MakeCommand(PerformanceMinusExecute);
		public ICommand PerformancePlus => MakeCommand(PerformancePlusExecute);
		public ICommand RelaxationMinus => MakeCommand(RelaxationMinusExecute);
		public ICommand RelaxationPlus => MakeCommand(RelaxationPlusExecute);
		public ICommand RepeatMinus => MakeCommand(RepeatMinusExecute);
		public ICommand RepeatPlus => MakeCommand(RepeatPlusExecute);

		public ICommand ChangeSelectExercise => MakeCommand(ChangeSelectExerciseExecute);

		public ICommand AddingExercise => MakeCommand(AddingExerciseExecute);
		public ICommand SaveTraining => new Command(execute: async () => {

			if(SelectExercise != null) {
				SelectExercise.NameExercise = NameExercise;
				SelectExercise.TimePrepare = GetTimeFromString(PreparationEntry);
				SelectExercise.TimeRest = GetTimeFromString(RelaxationEntry);
				SelectExercise.TimeWorking = GetTimeFromString(PerformanceEntry);
				SelectExercise.Cirle = Convert.ToInt32(RepeatEntry);
				var index = ExerciseList.IndexOf(ExerciseList.Where(X => X.Id == SelectExercise.Id).FirstOrDefault());
				ExerciseList[index] = SelectExercise;
			}

			if(_currUser.Id_training == 0) {
				await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO TrainingTable(NameTraining, id_account) VALUES('" + NameTraining + "', " + _currUser.Id_account + ")", CancellationToken);
				var idTrainingEnd = await DataServices.SportHelperDataService.GetTrainingAsync("SELECT * FROM TrainingTable ORDER BY id_training DESC LIMIT 1", CancellationToken);
				foreach (var item in ExerciseList) {
					await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO ExerciseTable(NameExercise, TimePrepare, TimeWorking, TimeRest, Circle, id_training) " +
					"Values('" + item.NameExercise + "', " + item.TimePrepare + "," + item.TimeWorking + ", " + item.TimeRest + ", " + item.Cirle + ", " + idTrainingEnd.Data[0].Id + ")", CancellationToken);

				}
				NavigateTo(AppPages.ListTraining);
			}
			else {
				foreach (var item in ExerciseList) {
					if (item.Id >= 10000) {
						await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO ExerciseTable(NameExercise, TimePrepare, TimeWorking, TimeRest, Circle, id_training) " +
						"Values('" + item.NameExercise + "', " + item.TimePrepare + "," + item.TimeWorking + ", " + item.TimeRest + ", " + item.Cirle + ", " + _currUser.Id_training + ")", CancellationToken);
					}
					else {
						await DataServices.SportHelperDataService.ExecuteAsync("" +
							"UPDATE ExerciseTable SET	" +
							"NameExercise = '" + item.NameExercise + "'," +
							"TimePrepare = " + item.TimePrepare + "," +
							"TimeWorking = " + item.TimeWorking + "," +
							"TimeRest = " + item.TimeRest + "," +
							"Circle = " + item.Cirle + " " +
							"WHERE id_exercise = " + item.Id, CancellationToken);
					}
				}
				NavigateTo(AppPages.ListTraining);

			}

			
		});

		

		public async override Task OnPageAppearing() {
			PreparationEntry = "00:05";
			PerformanceEntry = "00:05";
			RelaxationEntry = "00:05";
			RepeatEntry = "0";
			var currUser = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			_currUser = currUser.Data[0];
			var currentTraining = await DataServices.SportHelperDataService.GetExerciseAsync("SELECT * FROM ExerciseTable WHERE id_training = " + _currUser.Id_training, CancellationToken);
			var tmp = await DataServices.SportHelperDataService.GetTrainingAsync("SELECT * FROM TrainingTable Where id_training = " + _currUser.Id_training, CancellationToken);
			foreach (var curr in currentTraining.Data) {
				NameTraining = tmp.Data[0].NameTraining;
				ExerciseList.Add(new ExerciseDataObject {
					Id = curr.Id,
					Id_training = curr.Id_training,
					Cirle = curr.Cirle,
					NameExercise = curr.NameExercise,
					TimePrepare = curr.TimePrepare,
					TimeRest = curr.TimeRest,
					TimeWorking = curr.TimeWorking
				});
			}
		}

		
		void CalculationMinus(ref int min, ref int sec) {

			if ((min != 0) && (sec - 5 >= 0)) {

				sec -= 5;

			} else if ((min == 0) && (sec - 5 >= 0)) {

				sec -= 5;

			} else if (min != 0 && sec - 5 < 0) {
				var vrem = Convert.ToInt32(sec) - 5;
				sec = 60 + (sec - 5);
				min--;
			}
		}

		void CalculationPlus(ref int min, ref int sec) {

			if ((sec + 5 >= 60) && (min < 59)) {
				min++;
				sec = (sec + 5) - 60;
			} else if ((sec + 5 < 60) && (min <= 59)) {
				sec += 5;
			} else if ((sec + 5 <= 60) && (min < 59)) {
				sec += 5;
				if (sec + 5 == 60)
					min++;
			}
		}

		string FormatRezult(int min, int sec) {
			var rez = "";
			if ((sec < 10) && (min < 10)) {
				rez = '0' + min.ToString() + ":" + '0' + sec.ToString();
			} else if ((sec >= 10) && (min >= 10)) {
				rez = min.ToString() + ":" + sec.ToString();
			} else if ((min < 10) && (sec >= 10)) {
				rez = '0' + min.ToString() + ":" + sec.ToString();
			} else if ((min >= 10) && (sec < 10)) {
				rez = min.ToString() + ":" + '0' + sec.ToString();
			}
			return rez;
		}

		void PreparationMinusExecute() {
			var time = PreparationEntry;
			if (time != "00:00") {

				var minSec = time.Split(':');
				var min = Convert.ToInt32(minSec[0]);
				var sec = Convert.ToInt32(minSec[1]);
				CalculationMinus(ref min, ref sec);
				Prepare = sec + (60*min);
				PreparationEntry = FormatRezult(min, sec);
			}

		}
		void PreparationPlusExecute() {

			var time = PreparationEntry;
			var minSec = time.Split(':');
			var min = Convert.ToInt32(minSec[0]);
			var sec = Convert.ToInt32(minSec[1]);
			CalculationPlus(ref min, ref sec);
			Prepare = sec + (60 * min);
			PreparationEntry = FormatRezult(min, sec);

		}
		void PerformanceMinusExecute() {
			var time = PerformanceEntry;
			if (time != "00:00") {
				
				var minSec = time.Split(':');
				var min = Convert.ToInt32(minSec[0]);
				var sec = Convert.ToInt32(minSec[1]);

				CalculationMinus(ref min, ref sec);
				Working = sec + (60 * min);
				PerformanceEntry = FormatRezult(min, sec);
			}

		}
		void PerformancePlusExecute() {
			var time = PerformanceEntry;
			var minSec = time.Split(':');
			var min = Convert.ToInt32(minSec[0]);
			var sec = Convert.ToInt32(minSec[1]);

			CalculationPlus(ref min, ref sec);
			Working = sec + (60 * min);
			PerformanceEntry = FormatRezult(min, sec);
		}
		void RelaxationMinusExecute() {
			
			var time = RelaxationEntry;
			if (time != "00:00") {
				var minSec = time.Split(':');
				var min = Convert.ToInt32(minSec[0]);
				var sec = Convert.ToInt32(minSec[1]);

				CalculationMinus(ref min, ref sec);
				Relax = sec + (60 * min);
				RelaxationEntry = FormatRezult(min, sec);
			}
		}
		void RelaxationPlusExecute() {

			var time = RelaxationEntry;
			var minSec = time.Split(':');
			var min = Convert.ToInt32(minSec[0]);
			var sec = Convert.ToInt32(minSec[1]);

			CalculationPlus(ref min, ref sec);
			Relax = sec + (60 * min);
			RelaxationEntry = FormatRezult(min, sec);
		}
		void RepeatMinusExecute() {

			var time = RepeatEntry;
			if (time != "0") {

				var minSec = Convert.ToInt32(time);
				minSec--;

				RepeatEntry = minSec.ToString();
			}
		}
		void RepeatPlusExecute() {
			var time = RepeatEntry;
			var minSec = Convert.ToInt32(time);
			minSec++;

			RepeatEntry = minSec.ToString();
		}

		void AddingExerciseExecute() {
			ExerciseList.Add(new ExerciseDataObject {	Id = _i,
													NameExercise = NameExercise,
													TimePrepare = Prepare,
													TimeWorking = Working,
													TimeRest = Relax,
													Cirle = Convert.ToInt32(RepeatEntry)
			});
			_i++;
		}

		void ChangeSelectExerciseExecute() {
			NameExercise = SelectExercise.NameExercise;
			PreparationEntry = string.Format("{0:00}:{1:00}", SelectExercise.TimePrepare / 60, SelectExercise.TimePrepare % 60);
			PerformanceEntry = string.Format("{0:00}:{1:00}", SelectExercise.TimeWorking / 60, SelectExercise.TimeWorking % 60);
			RelaxationEntry = string.Format("{0:00}:{1:00}", SelectExercise.TimeRest / 60, SelectExercise.TimeRest % 60);
			RepeatEntry = SelectExercise.Cirle.ToString();

		}

		int GetTimeFromString(string time) {
			var minSec = time.Split(':');
			var min = Convert.ToInt32(minSec[0]);
			var sec = Convert.ToInt32(minSec[1]);
			sec += min * 60;
			return sec;
		}


	}
}
  