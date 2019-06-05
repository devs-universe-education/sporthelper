using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SportHelper.BL.Model
{
	[Table("AccountTable")]
	public class AccountTable {

		[PrimaryKey, AutoIncrement, Column("id_account"), Unique, NotNull]
		public int Id { get; set; }

		[Unique, Column("Login"), NotNull]
		public string Login { get;set; }

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

	[Table("TrainingTable")]
	public class TrainingTable {

		[PrimaryKey, AutoIncrement, Column("id_training"), Unique]
		public int Id { get; set; }

		[Column("NameTraining"), Unique]
		public string NameTraining { get; set; }


		[Column("id_account")]
		public int Id_account{ get; set; }

	}

	[Table("ExerciseTable")]
	public class ExerciseTable {

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

	[Table("StatisticTable")]
	public class StatisticTable {

		[PrimaryKey, AutoIncrement, Column("id_statistic"), Unique]
		public int Id { get; set; }

		[Column("Weight")]
		public string Weight { get; set; }

		[Column("Date")]
		public string Date { get; set; }

		[Column("id_account")]
		public int Id_account { get; set; }

	}

	[Table("CurrentUserTable")]
	public class CurrentUserTable {

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
