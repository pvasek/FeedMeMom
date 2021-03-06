using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreAnimation;

namespace FeedMeMom
{
	public class ProgressBar: UIView
	{
		public ProgressBar()
		{
			ForeColor = ColorUtil.FromHex("#d6d6d6");
			BackColor = ColorUtil.FromHex("#bfbfbf");
			TextColor = ColorUtil.FromHex("#919191");

			ClipsToBounds = true;
			ValueView = new UIView();
			LabelView = new UILabel 
			{ 
				TextAlignment =   UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};

			AddSubview(ValueView);
			AddSubview(LabelView);
			Width = 40;
			Active = false;
		}

		public UIColor BackColor { get; set; }
		public UIColor ForeColor { get; set; }
		public UIColor TextColor { get; set; }
		public UIColor ActiveBackColor { get; set; }
		public UIColor ActiveForeColor { get; set; }
		public UIColor ActiveTextColor { get; set; }

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

		private bool _active;

		public void ApplyColors()
		{
			if (_active)
			{
				ValueView.BackgroundColor = ActiveForeColor;
				BackgroundColor = ActiveBackColor;
				LabelView.TextColor = ActiveTextColor;
			} 
			else
			{
				ValueView.BackgroundColor = ForeColor;
				BackgroundColor = BackColor;
				LabelView.TextColor = TextColor;
			}
		}

		public bool Active 
		{
			get { return _active; }
			set 
			{
				_active = value;
				ApplyColors();

				if (_active ^ _running)
				{
					_running = _active;
					if (_running)
					{
						StartAnimation();
					} 
					else
					{
						Layer.RemoveAllAnimations();
						Layer.Opacity = 1f;
					}
				}
			}
		}

		public UIView ValueView { get; private set; }
		public UILabel LabelView { get; private set; }

		private bool _running;
		private void StartAnimation()
		{
			Layer.Opacity = 1f;
			UIView.Animate(0.7, 0, UIViewAnimationOptions.Autoreverse | UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.Repeat, () => {
				Layer.Opacity = 0.7f;
			}, null);
		}

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

