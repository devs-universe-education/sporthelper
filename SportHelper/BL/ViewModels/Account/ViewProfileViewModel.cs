using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.BL.DB;
using SportHelper.BL.Model;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;


namespace SportHelper.BL.ViewModels.Account {
	class ViewProfileViewModel : BaseViewModel {

		DataBaseConnection _dataBase = new DataBaseConnection();
		

		public ICommand GoToStatProfile => MakeCommand(GoToStatProfileExecute);
		public ICommand GetBMIResult => MakeCommand(GetBMIResultExecute);

		public string NameProfile {
			get => Get<string>();
			set => Set(value);
		}

		public string AgeProfile {
			get => Get<string>();
			set => Set(value);
			
		}

		public string WeightProfile {
			get => Get<string>();
			set => Set(value);
		}
		public string GrowthProfile {
			get => Get<string>();
			set => Set(value);
		}
		public string BMIProfile {
			get => Get<string>();
			set => Set(value);
		}

		public ViewProfileViewModel() {
			var user = _dataBase.GetUser();
			var statistic = _dataBase.db.Query<StatisticTable>("Select * from StatisticTable");
			var profile = _dataBase.db.Query<AccountTable>("Select * from AccountTable Where id_account = " + user.Id_account);

			NameProfile = profile[0].Name;

			if(statistic.Count != 0)
				WeightProfile = statistic[statistic.Count - 1].Weight.ToString();
			if(profile[0].Age != 0)
				AgeProfile = profile[0].Age.ToString();

			if (profile[0].Growth != 0)
				GrowthProfile = profile[0].Growth.ToString();

			BMIProfile = profile[0].BMI;


		}

		public override Task OnPageDissapearing() {
			double weight;
			double growth;
			int age;
			string date;


			var user = _dataBase.GetUser();

			if (!string.IsNullOrEmpty(WeightProfile)) {
				if (double.TryParse(WeightProfile, out weight)) {

					date = DateTime.Now.ToString("dd.MM.yyyy");
					var stat = _dataBase.db.Query<StatisticTable>("SELECT * FROM StatisticTable ");

					if (stat.Count > 0) {
						if (WeightProfile != stat[stat.Count - 1].Weight.ToString()) {
							_dataBase.db.Execute("INSERT INTO StatisticTable (Weight, Date, id_account) VALUES ('" + weight.ToString() + "', '" + date + "', " + user.Id_account + ")");
						}
					}
					else {
						_dataBase.db.Execute("INSERT INTO StatisticTable (Weight, Date, id_account) VALUES ('" + weight.ToString() + "', '" + date + "', " + user.Id_account + ")");
					}

				}
				else {
					WeightProfile = "";
				}
			}

			if ((!string.IsNullOrEmpty(GrowthProfile)) && (!string.IsNullOrEmpty(WeightProfile))) {
				if ((double.TryParse(WeightProfile, out weight)) && (double.TryParse(GrowthProfile, out growth))) {
					_dataBase.db.Execute("UPDATE AccountTable SET BMI = '" + BMIProfile + "' WHERE id_account = " + user.Id_account);
				}
				else {
					GrowthProfile = "";

				}
			}

			if (!string.IsNullOrEmpty(GrowthProfile)) {
				if (double.TryParse(WeightProfile, out weight)) {
					_dataBase.db.Execute("UPDATE AccountTable SET Growth = " + GrowthProfile + " WHERE id_account = " + user.Id_account);
				}
			}

			if (!string.IsNullOrEmpty(AgeProfile)) {
				if (int.TryParse(AgeProfile, out age)) {
					_dataBase.db.Execute("UPDATE AccountTable SET Age = " + AgeProfile + " WHERE id_account = " + user.Id_account);
				}
			}


			_dataBase.db.Execute("UPDATE AccountTable SET Name = '" + NameProfile + "' WHERE id_account = " + user.Id_account);
			var profile = _dataBase.db.Query<AccountTable>("Select * from AccountTable Where id_account = " + user.Id_account);
			return base.OnPageDissapearing();
		}


		void GetBMIResultExecute() {

			double weight;
			double growth;

			if ((!string.IsNullOrEmpty(GrowthProfile)) && (!string.IsNullOrEmpty(WeightProfile))) {
				if ((double.TryParse(WeightProfile, out weight)) && (double.TryParse(GrowthProfile, out growth))) {
					BMIProfile = ((weight * 10000) / (growth * growth)).ToString();
				}
			}
		}


		void GoToStatProfileExecute() {
			NavigateTo(AppPages.StatProfile);
		}
	}
}
