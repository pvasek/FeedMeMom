using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using MonoTouch.CoreGraphics;

namespace FeedMeMom.Helpers
{
	public static class ControlHelper
	{
		static ControlHelper()
		{
			IsIPhone5 = (568.0f - UIScreen.MainScreen.Bounds.Height) < 0.1;
		}

		public static bool IsIPhone5 { get; private set; }

		public static T SafeDispose<T>(this T view) where T: class, IDisposable
		{
			if (view != null)
			{
				view.Dispose();
			}
			return null;
		}

		public static void SetAllBackgrounds(this UIBarButtonItem button, UIImage image)
		{
			button.SetBackgroundImage(image, UIControlState.Normal, UIBarMetrics.Default);
			button.SetBackgroundImage(image, UIControlState.Highlighted, UIBarMetrics.Default);
			button.SetBackgroundImage(image, UIControlState.Selected, UIBarMetrics.Default);
		}

		public static RectangleF Add(this RectangleF rect, float x = 0, float y = 0, float width = 0, float height = 0)
		{
			return new RectangleF(rect.X + x, rect.Y + y, rect.Width + width, rect.Height + height);
		}

		public static RectangleF Set(this RectangleF rect, float? x = null, float? y = null, float? width = null, float? height = null)
		{
			return new RectangleF(x ?? rect.X, y ?? rect.Y, width ?? rect.Width, height ?? rect.Height);
		}

		public static void ShowCurrentScreenBrightness()
		{
			using (var img = UIApplication.SharedApplication.Windows.First().Screen.Capture())
			{
				var cgimg = img.CGImage;
				var imgData = new byte[cgimg.Width * cgimg.Height * 4];
				var totalPerc = 0f;
				using (var bc = new CGBitmapContext(imgData, cgimg.Width, cgimg.Height, 8, 4 * cgimg.Width, CGColorSpace.CreateDeviceRGB(), CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big))
				{
					bc.DrawImage(new RectangleF(0, 0, cgimg.Width, cgimg.Height), cgimg);
					var totalBrightnessPercentage = 0f;
					var count = 0;
					for (var i = 0; i < imgData.Length; i = i + 4)
					{
						var r = imgData[i] / 255f;
						var g = imgData[i] / 255f;
						var b = imgData[i] / 255f;
						totalBrightnessPercentage += (r + g + b) / 3f;
						count++;
					}
					totalPerc = totalBrightnessPercentage * 100 / count;
				}
				var alert = new UIAlertView();
				alert.Title = String.Format("Brightness: {0}%", totalPerc);
				alert.AddButton("Close");
				alert.CancelButtonIndex = 0;
				alert.Show();
			}
		}

	}
}

