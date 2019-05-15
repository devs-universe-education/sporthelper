using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataObjects;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;
using static SportHelper.BL.ViewModels.Training.Item;

namespace SportHelper.BL.ViewModels.Training {
	public class Item {
		public Item(string name) {
			Name = name;
		}

		public class ItemHandler {
			public ItemHandler() {
				Items = new ObservableCollection<Item>();
			}

			public ObservableCollection<Item> Items { get; private set; }

			public void Add(Item item) {
				Items.Add(item);
			}
		}

		public string Name { get; set; }
	}

	public class ListTrainingViewModel : BaseViewModel {

		private readonly ItemHandler _itemHandler = new ItemHandler();
		int i = 0;

		public ICommand GoToEditTraining => MakeCommand(GoToEditTrainingExecute);
		public ICommand GoToStartTraining => MakeCommand(GoToStartTrainingExecute);
		public ICommand Test => MakeCommand(TestExecute);
		public ICommand SampleCommand => MakeCommand(OnSampleCommand);

		void GoToEditTrainingExecute() {
			NavigateTo(AppPages.EditTraining);
		}

		void GoToStartTrainingExecute() {
			NavigateTo(AppPages.StartTraining);
		}

		void TestExecute() {
			_itemHandler.Add(new Item(i.ToString()));
			i++;
		}

		public ObservableCollection<Item> Items {
			get { return _itemHandler.Items; }
		}

		async void OnSampleCommand() {
			await ShowAlert("", Application.Current.MainPage.Width.ToString() + " " + Application.Current.MainPage.Height.ToString(), "OK");
		}
	}
}
