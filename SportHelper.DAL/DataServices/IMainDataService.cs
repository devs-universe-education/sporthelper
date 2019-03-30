using System.Threading;
using System.Threading.Tasks;
using SportHelper.DAL.DataObjects;

namespace SportHelper.DAL.DataServices {
	public interface IMainDataService {
		Task<RequestResult<SampleDataObject>> GetSampleDataObject(CancellationToken ctx);
	}
}

