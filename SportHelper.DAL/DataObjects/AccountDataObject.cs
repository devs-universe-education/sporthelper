using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.DAL.DataObjects {
	[Table("AccountTable")]
	public class AccountDataObject {
		[PrimaryKey, AutoIncrement, Column("id_account"), Unique, NotNull]
		public int Id { get; set; }

		[Unique, Column("Login"), NotNull]
		public string Login { get; set; }

		[Column("Password"), NotNull]
		public string Password { get; set; }

		[Unique, Column("Name")]
		public string Name { get; set; }

		[Column("Age")]
		public int Age { get; set; }

		[Column("Growth")]
		public int Growth { get; set; }

		[Column("BMI")]
		public string BMI { get; set; }
	}
}
