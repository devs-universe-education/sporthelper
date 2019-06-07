using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.DAL.DataObjects {
	[Table("CurrentUserTable")]
	public class CurrentUserDataObject {
		[PrimaryKey, AutoIncrement, Column("id_user"), Unique]
		public int Id { get; set; }

		[Column("id_account")]
		public int Id_account { get; set; }

		[Column("id_training")]
		public int Id_training { get; set; }

		[Column("Remember")]
		public bool Remember { get; set; }
	}
}
