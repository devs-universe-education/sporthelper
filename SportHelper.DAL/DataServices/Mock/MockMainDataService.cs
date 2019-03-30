using System.Threading;
using System.Threading.Tasks;
using SportHelper.DAL.DataObjects;

namespace SportHelper.DAL.DataServices.Mock {
	class MockMainDataService: BaseMockDataService, IMainDataService {
		public Task<RequestResult<SampleDataObject>> GetSampleDataObject(CancellationToken ctx) {
			return GetMockData<SampleDataObject>("SportHelper.DAL.Resources.Mock.Main.SampleDataObject.json");
		}
	}
}

