using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportHelper.UI.Controls
{

	public class FocusedEventArgsToFocusedConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var eventArgs = value as FocusEventArgs;
			return eventArgs.IsFocused;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}


	public class NumericValidationBehavior : Behavior<Entry> {


		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create("Command", typeof(ICommand), typeof(NumericValidationBehavior), null);
		public static readonly BindableProperty InputConverterProperty =
				   BindableProperty.Create("Converter", typeof(IValueConverter), typeof(NumericValidationBehavior), null);

		public ICommand Command {
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public IValueConverter Converter {
			get { return (IValueConverter)GetValue(InputConverterProperty); }
			set { SetValue(InputConverterProperty, value); }
		}

		public Entry AssociatedObject { get; private set; }

		protected override void OnAttachedTo(Entry entry) {
			entry.TextChanged += OnEntryTextChanged;
			entry.Unfocused += OnEntryFocus;
			entry.BindingContextChanged += OnBindingContextChanged;
			base.OnAttachedTo(entry);
			AssociatedObject = entry;
		}

		protected override void OnDetachingFrom(Entry entry) {
			entry.TextChanged -= OnEntryTextChanged;
			entry.Focused -= OnEntryFocus;
			entry.BindingContextChanged -= OnBindingContextChanged;
			base.OnDetachingFrom(entry);
			AssociatedObject = null;
		}

		void OnBindingContextChanged(object sender, EventArgs e) {
			OnBindingContextChanged();
		}

		protected override void OnBindingContextChanged() {
			base.OnBindingContextChanged();
			BindingContext = AssociatedObject.BindingContext;
		}

		void OnEntryTextChanged(object sender, TextChangedEventArgs args) {
			double result;
			var isValid = double.TryParse(args.NewTextValue, out result);
			if (!isValid) {
				if (((Entry)sender).Text.Length != 0)
					((Entry)sender).Text = args.OldTextValue; //((Entry)sender).Text.Substring(0, ((Entry)sender).Text.Length - 1);

			}
		}

		void OnEntryFocus(object sender, FocusEventArgs e) {
			if (!e.IsFocused) {
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
