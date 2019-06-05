using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportHelper.UI.Controls
{
	public class PaintSurfaceCommandBehavior : Behavior<SKCanvasView> {

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(
				nameof(Command),
				typeof(ICommand),
				typeof(PaintSurfaceCommandBehavior),
				null);

		public ICommand Command {
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		protected override void OnAttachedTo(SKCanvasView bindable) {
			base.OnAttachedTo(bindable);

			bindable.BindingContextChanged += OnBindingContextChanged;
			bindable.PaintSurface += OnPaintSurface;
		}

		protected override void OnDetachingFrom(SKCanvasView bindable) {
			base.OnDetachingFrom(bindable);

			bindable.BindingContextChanged -= OnBindingContextChanged;
			bindable.PaintSurface -= OnPaintSurface;
		}

		private void OnBindingContextChanged(object sender, EventArgs e) {
			BindingContext = ((BindableObject)sender).BindingContext;
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e) {
			if (Command?.CanExecute(e) == true) {
				Command.Execute(e);
			}
		}
	}
}
