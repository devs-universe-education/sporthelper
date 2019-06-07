using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SportHelper.DAL.DataObjects;

namespace SportHelper.DAL.DataServices {
	public interface ISportHelperService {

		Task<RequestResult<List<AccountDataObject>>> GetAccountAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult<List<TrainingDataObject>>> GetTrainingAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult<List<ExerciseDataObject>>> GetExerciseAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult<List<StatisticDataObject>>> GetStatisticAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult<List<CurrentUserDataObject>>> GetCurrentUserAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult> DeleteTrainingAsync(int id, CancellationToken ctx);
		Task<RequestResult> ExecuteAsync(string executeCommand, CancellationToken ctx);
		Task<RequestResult> CreateCurrentUserAsync(CancellationToken ctx);


	}
}
