using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.DAL.DataObjects {
	[Table("ExerciseTable")]
	public class ExerciseDataObject {
		[PrimaryKey, AutoIncrement, Column("id_exercise"), Unique]
		public int Id { get; set; }

		[Column("NameExercise")]
		public string NameExercise { get; set; }

		[Column("TimePrepare")]
		public int TimePrepare { get; set; }

		[Column("TimeRest")]
		public int TimeRest { get; set; }

		[Column("TimeWorking")]
		public int TimeWorking { get; set; }

		[Column("Circle")]
		public int Cirle { get; set; }

		[Column("id_training")]
		public int Id_training { get; set; }

	}
}
