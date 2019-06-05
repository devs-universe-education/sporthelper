using SportHelper.BL.DB;
using SportHelper.BL.Model;
using SportHelper.DAL.DataServices;
using SportHelper.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SportHelper
{
	public class App : Application
	{
		DataBaseConnection a = new DataBaseConnection();

		void GetPrepareBD() {
			a.db.CreateTable<AccountTable>();
			a.db.CreateTable<TrainingTable>();
			a.db.CreateTable<ExerciseTable>();
			a.db.CreateTable<StatisticTable>();
			a.db.CreateTable<CurrentUserTable>();

			var check = a.db.Query<CurrentUserTable>("Select * From CurrentUserTable");
			if (check.Count == 0) {
				a.db.Execute("INSERT INTO CurrentUserTable (id_account) VALUES (0)");
			}
		}


		public App ()
		{
			GetPrepareBD();
			DialogService.Init(this);
			NavigationService.Init(this);
			DataServices.Init(true);
		}

		protected override void OnStart ()
		{
			

			NavigationService.Instance.SetMainPage(AppPages.Login);
		}
	}
}

