using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.DAL.DataServices.Mock {
	class MockSportHelperDataService : BaseMockDataService, ISportHelperService {
		public  Task<RequestResult<List<AccountDataObject>>> GetAccountAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult<List<TrainingDataObject>>> GetTrainingAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult<List<ExerciseDataObject>>> GetExerciseAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult<List<StatisticDataObject>>> GetStatisticAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult<List<CurrentUserDataObject>>> GetCurrentUserAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult> DeleteTrainingAsync(int id, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult> ExecuteAsync(string executeCommand, CancellationToken ctx) {
			throw new NotImplementedException();
		}

		public  Task<RequestResult> CreateCurrentUserAsync(CancellationToken ctx) {
			throw new NotImplementedException();
		}
	}
}
