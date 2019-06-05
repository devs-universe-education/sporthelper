using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SportHelper.BL.Model;
using SportHelper.Helpers;
using Xamarin.Forms;
using SportHelper.BL.ViewModels;
using System;
using SQLite;
using SportHelper.BL.DB;

namespace SportHelper.BL.ViewModels.Training {
	class EditTrainingViewModel : BaseViewModel {

		DataBaseConnection _dataBase = new DataBaseConnection();
		public int Prepare = 5;
		public int Relax = 5;
		public int Working = 5;

		public EditTrainingViewModel() {
			ExerciseList = new ObservableCollection<ExerciseTable>() { };
			ExerciseList.Clear();
		}

		public TrainingTable SelectExercise {
			get => Get<TrainingTable>();
			set => Set(value);
		}

		public ObservableCollection<ExerciseTable> ExerciseList {
			get => Get<ObservableCollection<ExerciseTable>>();
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

		public ICommand PreparationMinus => MakeCommand(PreparationMinusExecute);
		public ICommand PreparationPlus => MakeCommand(PreparationPlusExecute);
		public ICommand PerformanceMinus => MakeCommand(PerformanceMinusExecute);
		public ICommand PerformancePlus => MakeCommand(PerformancePlusExecute);
		public ICommand RelaxationMinus => MakeCommand(RelaxationMinusExecute);
		public ICommand RelaxationPlus => MakeCommand(RelaxationPlusExecute);
		public ICommand RepeatMinus => MakeCommand(RepeatMinusExecute);
		public ICommand RepeatPlus => MakeCommand(RepeatPlusExecute);

		public ICommand AddingExercise => MakeCommand(AddingExerciseExecute);
		public ICommand SaveTraining => MakeCommand(SaveTrainingExecute);

		public override Task OnPageAppearing() {
			PreparationEntry = "00:05";
			PerformanceEntry = "00:05";
			RelaxationEntry = "00:05";
			RepeatEntry = "0";
			var currentTraining = _dataBase.db.Query<ExerciseTable>("SELECT * FROM ExerciseTable WHERE id_training = " + _dataBase.GetUser().Id_training);
			foreach (var curr in currentTraining) {
				NameTraining = _dataBase.db.Query<TrainingTable>("SELECT * FROM TrainingTable Where id_training = " + _dataBase.GetUser().Id_training)[0].NameTraining;
				ExerciseList.Add(new ExerciseTable {
					Id = curr.Id,
					Id_training = curr.Id_training,
					Cirle = curr.Cirle,
					NameExercise = curr.NameExercise,
					TimePrepare = curr.TimePrepare,
					TimeRest = curr.TimeRest,
					TimeWorking = curr.TimeWorking
				});
			}
			return base.OnPageAppearing();
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
			ExerciseList.Add(new ExerciseTable {	Id = 0,
													NameExercise = NameExercise,
													TimePrepare = Prepare,
													TimeWorking = Working,
													TimeRest = Relax,
													Cirle = Convert.ToInt32(RepeatEntry)
			});
		}
		void SaveTrainingExecute() {

			var tmp = _dataBase.GetUser();
			var t = _dataBase.db.Query<TrainingTable>("Select * FROM TrainingTable");
			_dataBase.db.Execute("Insert into TrainingTable(NameTraining, id_account) Values('" + NameTraining + "', " + tmp.Id_account + ")");
			var idTrainingEnd = _dataBase.db.Query<TrainingTable>("SELECT * FROM TrainingTable ORDER BY id_training DESC LIMIT 1");

			foreach(var exer in ExerciseList) {
				if(exer.Id == 0) {
					_dataBase.db.Execute("INSERT INTO ExerciseTable(NameExercise, TimePrepare, TimeWorking, TimeRest, Circle, id_training) " +
						"Values('" + exer.NameExercise + "', " + exer.TimePrepare + "," + exer.TimeWorking + ", " + exer.TimeRest + ", " + exer.Cirle + ", " + tmp.Id_account + ")");
				} else {
					_dataBase.db.Execute("UPDATE ExerciseTable SET	NameExercise = " + exer.NameExercise + "," +
										"TimePrepare = " + exer.TimePrepare + "," +
										"TimeWorking = " + exer.TimeWorking + "," +
										"TimeRest = " + exer.TimeRest + "," +
										"Circle = " + exer.Cirle + " " +
										"WHERE Id = " + exer.Id);
				}
			}

			var test = _dataBase.db.Query<TrainingTable>("SELECT * FROM TrainingTable");
			var test1 = _dataBase.db.Query<ExerciseTable>("SELECT * FROM ExerciseTable");
		}

		
	}
}
  