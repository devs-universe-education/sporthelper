using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SportHelper.BL.Model;
using SQLite;

namespace SportHelper.BL.DB
{
    public class DataBaseConnection
    {
		public SQLiteConnection db;
		

		public DataBaseConnection() {
			var dbPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Personal),
			"test9.db3");
			
			db = new SQLiteConnection(dbPath);
		}

		public CurrentUserTable GetUser() {
			var UserTmp = db.Query<CurrentUserTable>("SELECT * FROM CurrentUserTable WHERE id_user = 1");

			var User = new CurrentUserTable {	Id = UserTmp[0].Id,
												Id_account = UserTmp[0].Id_account,
												Id_training = UserTmp[0].Id_training,
												Remember = UserTmp[0].Remember
			};


			return User;
		}

		
	}
}
