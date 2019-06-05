using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace SportHelper.UI.Controls {

	public class SelectedEventArgsToSelectedConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var eventArgs = value as SelectedItemChangedEventArgs;
			return eventArgs.SelectedItem;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}

	class ListViewItemSelectedBehavior : Behavior<ListView> {

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewItemSelectedBehavior), null);
		public static readonly BindableProperty InputConverterProperty =
				   BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ListViewItemSelectedBehavior), null);

		public ICommand Command {
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public IValueConverter Converter {
			get { return (IValueConverter)GetValue(InputConverterProperty); }
			set { SetValue(InputConverterProperty, value); }
		}

		public ListView AssociatedObject { get; private set; }

		protected override void OnAttachedTo(ListView listView) {

			listView.ItemSelected += OnListViewSelectedItem;
			listView.BindingContextChanged += OnBindingContextChanged;
			base.OnAttachedTo(listView);
			AssociatedObject = listView;
		}

		protected override void OnDetachingFrom(ListView listView) {
			listView.BindingContextChanged -= OnBindingContextChanged;
			listView.ItemSelected -= OnListViewSelectedItem;
			base.OnDetachingFrom(listView);
			AssociatedObject = null;
		}

		void OnBindingContextChanged(object sender, EventArgs e) {
			OnBindingContextChanged();
		}

		protected override void OnBindingContextChanged() {
			base.OnBindingContextChanged();
			BindingContext = AssociatedObject.BindingContext;
		}

		void onListTapped(object sender, TappedEventArgs e) {

		}

		void OnListViewSelectedItem(object sender, SelectedItemChangedEventArgs e) {

			if (e.SelectedItem != null) {
				if (Command == null) {
					return;
				}


				var parameter = Converter.Convert(e, typeof(object), null, null);
				if (Command.CanExecute(parameter)) {
					Command.Execute(parameter);
				}
			}
			
		}
	}
}
