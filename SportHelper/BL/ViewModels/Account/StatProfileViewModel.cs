using System.Threading.Tasks;
using System.Windows.Input;
using SportHelper.DAL.DataServices;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SportHelper.DAL.DataObjects;
using System.Collections.Generic;
using System;

namespace SportHelper.BL.ViewModels.Account {
	class StatProfileViewModel : BaseViewModel {

		public ICommand OnPainting { get; private set; }

		List<StatisticDataObject> _statistic;

		public string AllTraining {
			get => Get<string>();
			set => Set(value);
		}

		public string CompletedTraining {
			get => Get<string>();
			set => Set(value);
		}

		public string ChangeWeight {
			get => Get<string>();
			set => Set(value);
		}

		public async override Task OnPageAppearing() {
			var i = 0;
			var currUser = await DataServices.SportHelperDataService.GetCurrentUserAsync("SELECT * FROM CurrentUserTable", CancellationToken);
			var tmp = await DataServices.SportHelperDataService.GetStatisticAsync("SELECT * FROM StatisticTable Where id_account = " + currUser.Data[0].Id_account, CancellationToken);
			_statistic = tmp.Data;
			
			
		}

		public StatProfileViewModel() {
			OnPainting = new Command<SKPaintSurfaceEventArgs>(OnPaintingExecute);
		}


		private void OnPaintingExecute(SKPaintSurfaceEventArgs e) {
			float max;
				float min;

			var surface = e.Surface;
			var canvas = surface.Canvas;
			canvas.Clear(SKColors.White);

			var width = e.Info.Width;
			var height = e.Info.Height;
			var lineFill = new SKPaint {
				IsAutohinted = true,
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = new SKColor(0x84, 0xcd, 0xed),
				StrokeWidth = 5,
			};
			var sinkLineFill = new SKPaint {
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = new SKColor(0x84, 0xcd, 0xed),
				StrokeWidth = 1,
			};
			var circleFill = new SKPaint {

				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = SKColors.White,
			};
			var circleBorder = new SKPaint {
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Gray,
				StrokeWidth = 5
			};
			var textPaint = new SKPaint {
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = new SKColor(0x84, 0xcd, 0xed),
				TextSize = 20
			};


			var zeroX = 60;
			var zeroY = height - 60;
			


			canvas.DrawLine(zeroX, zeroY, width - 60, zeroY, lineFill);
			canvas.DrawLine(zeroX, 60, zeroX, zeroY, lineFill);

			if (_statistic.Count > 0) {

				

				max = (float)Convert.ToDouble(_statistic[0].Weight);
				min = (float)Convert.ToDouble(_statistic[0].Weight);

				foreach (var item in _statistic) {
					if (min > Convert.ToDouble(item.Weight))
						min = (float)Convert.ToDouble(item.Weight);

					if (max < Convert.ToDouble(item.Weight))
						max = (float)Convert.ToDouble(item.Weight);
				}


				var stepX = (width - 120) / (_statistic.Count + 1);
				var stepY = (height - 120) / (max - min + 10);
				height -= 120;
				canvas.DrawCircle(stepX, height -  stepY * (Convert.ToInt32(_statistic[0].Weight ) - (min - 1)), 15, circleFill);
				canvas.DrawCircle(stepX, height -  stepY * (Convert.ToInt32(_statistic[0].Weight) - (min - 1)), 15, circleBorder);			
				canvas.DrawText(_statistic[0].Weight + " Кг", stepX - 25, height - stepY * (Convert.ToInt32(_statistic[0].Weight) - (min - 1)) - 25, textPaint);
				canvas.DrawText(_statistic[0].Date, stepX - 35, zeroY + 35, textPaint);
				canvas.DrawLine(stepX, zeroY, stepX, height - stepY * (Convert.ToInt32(_statistic[0].Weight) - (min - 1)), sinkLineFill);

				for (var i = 0; i < _statistic.Count - 1; i++) {
					canvas.DrawCircle(stepX * (i + 2), height -  stepY * (Convert.ToInt32(_statistic[i + 1].Weight) - (min - 1)), 15, circleFill);
					canvas.DrawCircle((stepX * (i + 2)), height - stepY * (Convert.ToInt32(_statistic[i + 1].Weight) - (min - 1)), 15, circleBorder);

					canvas.DrawLine(stepX * (i + 1), height - stepY * (Convert.ToInt32(_statistic[i].Weight) - (min - 1)), (stepX * (i + 2)), height - (stepY * (Convert.ToInt32(_statistic[i + 1].Weight) - (min - 1))), lineFill);

					canvas.DrawText(_statistic[i + 1].Weight + " Кг", (stepX * (i + 2)) - 25, height - (stepY * (Convert.ToInt32(_statistic[i + 1].Weight) - (min - 1))) - 25, textPaint);
					canvas.DrawText(_statistic[i + 1].Date, (stepX * (i + 2)) - 35, zeroY + 35, textPaint);

					canvas.DrawLine(stepX * (i + 2), zeroY, (stepX * (i + 2)), height - (stepY * (Convert.ToInt32(_statistic[i + 1].Weight) - (min - 1))), sinkLineFill);
				}
			}

		}
	}
}
