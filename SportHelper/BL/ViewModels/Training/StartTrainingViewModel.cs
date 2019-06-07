using System.Timers;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Training {
	class StartTrainingViewModel : BaseViewModel {

		public ICommand StartTimer => MakeCommand(StartTimerExecute);
		public ICommand StopTimer => MakeCommand(StopTimerExecute);

		int _currentCirle = 0;
		int _currentStep = 0;
		int _currentExercise = 0;
		double _currentTime = 0;

		int _min;
		int _sec;
		int _msec;


		Timer _timer;

		double[,] _exercises;
		string[] _nameExercise;
		int[] _cirle;


		public string Timer {
			get => Get<string>();
			set => Set(value);
		}

		public string Step {
			get => Get<string>();
			set => Set(value);
		}

		public string Circle {
			get => Get<string>();
			set => Set(value);
		}

		public string NameExercise {
			get => Get<string>();
			set => Set(value);
		}

		public string NextNameExercise {
			get => Get<string>();
			set => Set(value);
		}

		public async override Task OnPageAppearing() {
			_timer = new Timer();
			var i = 0;
			var currUser = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			var tmp = await DataServices.SportHelperDataService.GetExerciseAsync("Select * From ExerciseTable Where id_training = " + currUser.Data[0].Id_training, CancellationToken);
			_exercises = new double[tmp.Data.Count, 3];
			_nameExercise = new string[tmp.Data.Count];
			_cirle = new int[tmp.Data.Count];
			foreach (var t in tmp.Data) {
				_exercises[i, 0] = t.TimePrepare;
				_exercises[i, 1] = t.TimeWorking;
				_exercises[i, 2] = t.TimeRest;
				_cirle[i] = t.Cirle;
				_nameExercise[i] = t.NameExercise;
				i++;
			}
			_currentTime = _exercises[0, 0];
			SetTimer();
		}

		public StartTrainingViewModel() {
			
		}


		void SetTimer() {
			_timer.Interval = 1;
			_timer.AutoReset = true;
			_timer.Elapsed += OnTimedEvent;
		}



		void onTimerTick() {

			if(_currentCirle != _cirle[_currentExercise] - 1) {
				_currentStep++;
				if(_currentStep == 3) {
					_currentCirle++;
					_currentStep = 1;
				}
			}
			else {
				_currentExercise++;
				_currentStep = 0;
				_currentCirle = 0;
			}
			if(_currentExercise == _nameExercise.Length) {
				_timer.Stop();
				ShowAlert("","Тренировка окончена","OK");
			}
			_currentTime = _exercises[_currentExercise, _currentStep];
		}

		private void OnTimedEvent(object source, ElapsedEventArgs e) {

			_min = (int)_currentTime / 60;
			_sec = (int)_currentTime % 60;
			_msec = (int)((_currentTime - (int)_currentTime) * 1000);
			_currentTime = _currentTime - 0.0017;

			switch (_currentStep) {
				case 0: {
						Step = "Подготовка";
						break;
				}
				case 1: {
						Step = "Работа";
						break;
				}
				case 2: {
						Step = "Отдых";
						break;
				}

				default: {
						break;
				}
			}
			NameExercise = _nameExercise[_currentExercise];
			Circle = (_cirle[_currentExercise] - _currentCirle).ToString();
			if(_currentExercise + 1 != _nameExercise.Length) {
				NextNameExercise = _nameExercise[_currentExercise + 1];
			}
			else {
				NextNameExercise = "Финал";
			}
			Timer = string.Format("{0}:{1:00}:{2:000}", _min, _sec, _msec);
			if(_currentTime < 0) {
				onTimerTick();
			}

		}

		void StartTimerExecute() {
			_timer.Start();
		}

		void StopTimerExecute() {
			_timer.Stop();
		}

	}
}
