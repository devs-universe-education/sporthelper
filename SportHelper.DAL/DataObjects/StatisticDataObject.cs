using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.DAL.DataObjects {
	[Table("StatisticTable")]
	public class StatisticDataObject {
		[PrimaryKey, AutoIncrement, Column("id_statistic"), Unique]
		public int Id { get; set; }

		[Column("Weight")]
		public string Weight { get; set; }

		[Column("Date")]
		public string Date { get; set; }

		[Column("id_account")]
		public int Id_account { get; set; }
	}
}
