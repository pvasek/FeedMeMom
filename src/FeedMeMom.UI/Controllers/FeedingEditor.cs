using MonoTouch.UIKit;
using FeedMeMom.Common.Entities;
using FeedMeMom.Helpers;
using FeedMeMom.Common;
using System;

namespace FeedMeMom.UI
{
	public partial class FeedingEditor : UIViewController
	{
		public FeedingEditor() : base("FeedingEditor", null)
		{
			Title = Resources.Feeding;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Cancel, UIBarButtonItemStyle.Plain, Close);
			NavigationItem.LeftBarButtonItem.SetToolbarStyle();
			NavigationItem.RightBarButtonItem = new UIBarButtonItem(Resources.Save, UIBarButtonItemStyle.Plain, Save);
			NavigationItem.RightBarButtonItem.SetToolbarStyle();
		}

		public void Close(object sender, EventArgs e)
		{
			Close();
		}

		private void Close()
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		public void Save(object sender, EventArgs e)
		{
			var repo = ServiceLocator.Get<Repository>();
			Feeding.Date = datePicker.Date;
			Feeding.LeftBreastLengthSeconds = txtLeftValue.Text.AsInt() * 60;
			Feeding.RightBreastLengthSeconds = txtRightValue.Text.AsInt() * 60;
			if (Feeding.Id == 0)
			{
				repo.Insert(Feeding);
			}
			else
			{
				repo.Update(Feeding);
			}
			Close();
		}

		public FeedingEntry Feeding { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lblLeft.Text = Resources.Left;
			lblRight.Text = Resources.Right;
			lblTotal.Text = Resources.Total;
			lblTime.Text = Resources.Time;

			Skin.SkinChanged += (sender, e) => ApplyColors();

			btnLeftPlus5.TouchUpInside += (sender, e) => AddLeft(5);
			btnLeftMinus5.TouchUpInside += (sender, e) => AddLeft(-5);
			btnLeftPlus1.TouchUpInside += (sender, e) => AddLeft(1);
			btnLeftMinus1.TouchUpInside += (sender, e) => AddLeft(-1);
			btnRightPlus5.TouchUpInside += (sender, e) => AddRight(5);
			btnRightMinus5.TouchUpInside += (sender, e) => AddRight(-5);
			btnRightPlus1.TouchUpInside += (sender, e) => AddRight(1);
			btnRightMinus1.TouchUpInside += (sender, e) => AddRight(-1);

			datePicker.ValueChanged += (sender, e) => Feeding.Date = datePicker.Date;

			btnRemove.TouchUpInside += (sender, e) => {
				var repo = ServiceLocator.Get<Repository>();


				var alert = new UIAlertView();
				alert.Message = Resources.DoYouReallyWantToDeleteThisFeeding;
				alert.AddButton(Resources.Yes);
				alert.AddButton(Resources.No);
				alert.Show();
				alert.Clicked += (bs, be) => {
					if (be.ButtonIndex == 0) 
					{
						repo.Delete(Feeding);
						Close();
					}
				};

			};

			if (Feeding == null)
			{
				Feeding = new FeedingEntry { Date = Time.Now };
			}

			ApplyColors();
			LoadData();
		}

		private void AddLeft(int time)
		{
			Feeding.AddLeft(time);
			LoadData();
		}

		private void AddRight(int time)
		{
			Feeding.AddRight(time);
			LoadData();
		}

				private void ApplyColors()
		{
			pnlTop.BackgroundColor = Skin.Active.Toolbar;
		}

		private void LoadData()
		{
			//lblDateValue.SetTitle(Feeding.Date.ToString("G"));
			datePicker.Date = Feeding.Date;
			txtLeftValue.Text = Feeding.LeftBreastLength.AsMinutesText();
			txtRightValue.Text = Feeding.RightBreastLength.AsMinutesText();
			lblTotalValue.Text = Feeding.TotalBreastLength.AsMinutesText();
		}
	}
}

