using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace FeedMeMom
{
	public class ProgressBar: UIView
	{
		public ProgressBar()
		{
			//Layer.CornerRadius = 20;
			ClipsToBounds = true;

			//Frame = new RectangleF(0,0, 40, 40);
			BackgroundColor = ColorUtil.FromHex("#bfbfbf");
			ValueView = new UIView 
			{ 
				//Frame = Frame,
				BackgroundColor = ColorUtil.FromHex("#d6d6d6")
			};
			LabelView = new UILabel 
			{ 
				//Frame = Frame,
				BackgroundColor = UIColor.Clear,
				TextAlignment =   UITextAlignment.Center,
				//Font =  UIFont.FromName("Helvetica Neue", 12),
				TextColor = ColorUtil.FromHex("#919191")
			};

			AddSubview(ValueView);
			AddSubview(LabelView);
			Width = 40;
		}

		public float Width 
		{
			set
			{
				Layer.CornerRadius = value / 2;
				Frame = new RectangleF(0,0, value, value);
				ValueView.Frame = Frame;
				LabelView.Frame = Frame;
				var fontSize = (12f / 40f) * value;
				const int minFontSize = 8;
				if (fontSize < minFontSize)
				{
					fontSize = minFontSize;
				}
				LabelView.Font = UIFont.FromName("Helvetica Neue", fontSize);
			}
		}

		public UIView ValueView { get; private set; }
		public UILabel LabelView { get; private set; }


		public void UpdateValue(int? total, int? value, bool showPercent)
		{
			string sufix = showPercent ? "%" : "";
			if (total == 0)
			{
				value = 0;
				total = 1;
			}
			var percent =  (float)(value ?? 0) / (total ?? 1);
			var rect = ValueView.Frame;
			var height = Frame.Height * percent;
			if (height > Frame.Height) {
				height = Frame.Height;
			}
			ValueView.Frame = new RectangleF(rect.Left, 0, rect.Size.Width, height);

			if (percent < 0.001)
			{
				sufix = "";
			}
			LabelView.Text = String.Format("{0:#}{1}", showPercent ? (int)(100 * percent) : (value / 60), sufix);
		}
	}
}

