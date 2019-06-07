using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL;
using SportHelper.DAL.DataServices;

namespace SportHelper.DAL.DataServices
{
    public class DataBaseConnection : ISportHelperService {
		public SQLiteAsyncConnection db;
		

		public DataBaseConnection(string dbPath) {
			db = new SQLiteAsyncConnection(dbPath);
			db.CreateTablesAsync<AccountDataObject, TrainingDataObject, ExerciseDataObject, StatisticDataObject, CurrentUserDataObject>();

				

			
		}


		public async Task<RequestResult<List<AccountDataObject>>> GetAccountAsync( string executeCommand, CancellationToken ctx) {
			var account = await db.QueryAsync<AccountDataObject>(executeCommand);
			return new RequestResult<List<AccountDataObject>>(account, RequestStatus.Ok);
		}

		public async Task<RequestResult<List<TrainingDataObject>>> GetTrainingAsync(string executeCommand, CancellationToken ctx) {
			var training = await db.QueryAsync<TrainingDataObject>(executeCommand);
			return new RequestResult<List<TrainingDataObject>>(training, RequestStatus.Ok);
		}

		public async Task<RequestResult<List<ExerciseDataObject>>> GetExerciseAsync(string executeCommand, CancellationToken ctx) {
			var exercise = await db.QueryAsync<ExerciseDataObject>(executeCommand);
			return new RequestResult<List<ExerciseDataObject>>(exercise, RequestStatus.Ok);
		}

		public async Task<RequestResult<List<StatisticDataObject>>> GetStatisticAsync(string executeCommand, CancellationToken ctx) {
			var statistic = await db.QueryAsync<StatisticDataObject>(executeCommand);
			return new RequestResult<List<StatisticDataObject>>(statistic, RequestStatus.Ok);
		}

		public async Task<RequestResult<List<CurrentUserDataObject>>> GetCurrentUserAsync(string executeCommand, CancellationToken ctx) {
			var currentUser = await db.QueryAsync<CurrentUserDataObject>(executeCommand);
			return new RequestResult<List<CurrentUserDataObject>>(currentUser, RequestStatus.Ok);
		}

		public async Task<RequestResult> DeleteTrainingAsync(int id, CancellationToken ctx) {

			await db.RunInTransactionAsync(execute => {
				var training = execute.Query<TrainingDataObject>("Select * From TrainingTable Where id_training = " + id);
				if (training.Count != 0) {
					var exercise = execute.Query<ExerciseDataObject>("SELECT * FROM ExerciseTable WHERE id_training = " + id);
					foreach (var item in exercise) {
						execute.Execute("DELETE FROM ExerciseTable WHERE id_training = " + id);
					}
					execute.Execute("DELETE FROM TrainingTable WHERE id_training = " + id);
					
				}
			});
			return new RequestResult(RequestStatus.Ok);
		}

		public async Task<RequestResult> ExecuteAsync(string executeCommand, CancellationToken ctx) {
			await db.ExecuteAsync(executeCommand);
			return new RequestResult(RequestStatus.Ok);
		}

		public async Task<RequestResult> CreateCurrentUserAsync(CancellationToken ctx) {

			await db.RunInTransactionAsync(execute => {
				var check = execute.Query<CurrentUserDataObject>("Select * From CurrentUserTable");
				if (check.Count == 0) {
					db.ExecuteAsync("INSERT INTO CurrentUserTable (id_account) VALUES (0)");
				}
			});

			return new RequestResult(RequestStatus.Ok);
		}


	}
}
