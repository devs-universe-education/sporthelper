namespace SportHelper
{
	public enum AppPages {
		Main,
		Login,
		Register,
		StatProfile,
		ViewProfile,
		AboutProgram,
		MainMenu,
		EditTraining,
		ListTraining,
		StartTraining
	}

	public enum NavigationMode {
		Normal,
		Modal,
		Root,
		Custom
	}

	public enum PageState {
		Clean,
		Loading,
		Normal,
		NoData,
		Error,
		NoInternet
	}
}

