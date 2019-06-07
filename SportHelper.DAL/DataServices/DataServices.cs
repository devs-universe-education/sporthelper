using System;
using SportHelper.DAL.DataServices;

namespace SportHelper.DAL.DataServices
{
	public static class DataServices {
		public static IMainDataService Main { get; private set; }

		public static ISportHelperService SportHelperDataService {get; private set;}

		public static void Init(bool isMock, string dbPath) {
			if (isMock) {
				SportHelperDataService = new Mock.MockSportHelperDataService();
			}
			else {
				SportHelperDataService = new DataBaseConnection(dbPath);
			}
		}
	}
}

