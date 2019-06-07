using SportHelper.Android;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DataBaseConnectionAndroid))]
namespace SportHelper.Android {
	class DataBaseConnectionAndroid : IDataBaseConnection {
		public string GetdbPath() {
			var dbName = "test3.db3";
			var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
			return path;
		}
	}

}