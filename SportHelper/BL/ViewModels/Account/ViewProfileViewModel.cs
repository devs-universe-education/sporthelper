using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;

namespace SportHelper.BL.ViewModels.Account {
	class ViewProfileViewModel : BaseViewModel {

		public SampleDataObject SampleObject {
			get => Get<SampleDataObject>();
			private set => Set(value);
		}
		
		public override async Task OnPageAppearing() {
			State = PageState.Loading;
			var result = await DataServices.Main.GetSampleDataObject(CancellationToken);
			if (result.IsValid) {
				SampleObject = result.Data;
				State = PageState.Normal;
			} else
				State = PageState.Error;
		}


		public ICommand GoToStatProfile => MakeCommand(GoToStatProfileExecute);
		void GoToStatProfileExecute() {
			NavigateTo(AppPages.StatProfile);
		}
	}
}
