using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;

namespace SportHelper.BL.ViewModels.Account {
	class ViewProfileViewModel : BaseViewModel {

		

		public ICommand GoToStatProfile => MakeCommand(GoToStatProfileExecute);
		public ICommand GetBMIResult => MakeCommand(GetBMIResultExecute);
		public ICommand ChangeWeight => new Command(execute: async () => {
			double weight;
			string date;
			var user = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			if (!string.IsNullOrEmpty(WeightProfile)) {
				if (double.TryParse(WeightProfile, out weight)) {
					date = DateTime.Now.ToString("dd.MM.yyyy");
					var stat = await DataServices.SportHelperDataService.GetStatisticAsync("SELECT * FROM StatisticTable ", CancellationToken);
					if (stat.Data.Count > 0) {
						if (WeightProfile != stat.Data[stat.Data.Count - 1].Weight.ToString()) {
							await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO StatisticTable (Weight, Date, id_account) VALUES ('" + weight.ToString() + "', '" + date + "', " + user.Data[0].Id_account + ")", CancellationToken);
						}
					}
					else {
						await DataServices.SportHelperDataService.ExecuteAsync("INSERT INTO StatisticTable (Weight, Date, id_account) VALUES ('" + weight.ToString() + "', '" + date + "', " + user.Data[0].Id_account + ")", CancellationToken);
					}
				}
				else {
					WeightProfile = "";
				}
			}
			GetBMIResultExecute();
		});


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

		public async override Task OnPageAppearing() {
			var user = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			var statistic = await DataServices.SportHelperDataService.GetStatisticAsync("Select * from StatisticTable Where id_account = " + user.Data[0].Id_account, CancellationToken);
			var profile = await DataServices.SportHelperDataService.GetAccountAsync("Select * from AccountTable Where id_account = " + user.Data[0].Id_account, CancellationToken);
			NameProfile = profile.Data[0].Name;
			if (statistic.Data.Count != 0)
				WeightProfile = statistic.Data[statistic.Data.Count - 1].Weight.ToString();
			if (profile.Data[0].Age != 0)
				AgeProfile = profile.Data[0].Age.ToString();

			if (profile.Data[0].Growth != 0)
				GrowthProfile = profile.Data[0].Growth.ToString();

			BMIProfile = profile.Data[0].BMI;
		}

		public async override Task OnPageDissapearing() {
			double weight;
			double growth;
			int age;

			var user = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			

			if ((!string.IsNullOrEmpty(GrowthProfile)) && (!string.IsNullOrEmpty(WeightProfile))) {
				if ((double.TryParse(WeightProfile, out weight)) && (double.TryParse(GrowthProfile, out growth))) {
					await DataServices.SportHelperDataService.ExecuteAsync("UPDATE AccountTable SET BMI = '" + BMIProfile + "' WHERE id_account = " + user.Data[0].Id_account, CancellationToken);
				}
				else {
					GrowthProfile = "";

				}
			}

			if (!string.IsNullOrEmpty(GrowthProfile)) {
				if (double.TryParse(WeightProfile, out weight)) {
					await DataServices.SportHelperDataService.ExecuteAsync("UPDATE AccountTable SET Growth = " + GrowthProfile + " WHERE id_account = " + user.Data[0].Id_account, CancellationToken);
				}
			}

			if (!string.IsNullOrEmpty(AgeProfile)) {
				if (int.TryParse(AgeProfile, out age)) {
					await DataServices.SportHelperDataService.ExecuteAsync("UPDATE AccountTable SET Age = " + AgeProfile + " WHERE id_account = " + user.Data[0].Id_account, CancellationToken);
				}
			}

			await DataServices.SportHelperDataService.ExecuteAsync("UPDATE AccountTable SET Name = '" + NameProfile + "' WHERE id_account = " + user.Data[0].Id_account, CancellationToken);

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
