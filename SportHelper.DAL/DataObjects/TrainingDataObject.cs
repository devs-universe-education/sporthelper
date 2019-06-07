using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.DAL.DataObjects {

	[Table("TrainingTable")]
	public class TrainingDataObject {
		[PrimaryKey, AutoIncrement, Column("id_training"), Unique]
		public int Id { get; set; }

		[Column("NameTraining"), Unique]
		public string NameTraining { get; set; }


		[Column("id_account")]
		public int Id_account { get; set; }
	}
}
