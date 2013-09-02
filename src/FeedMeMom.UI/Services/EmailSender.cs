using System;
using FeedMeMom.Common;
using MonoTouch.UIKit;
using MonoTouch.MessageUI;

namespace FeedMeMom
{
	public class EmailSender
	{
		public EmailSender(string recepient, UINavigationController navigationController)
		{
			_recepient = recepient;
			_navigationController = navigationController;
		}

		private string _recepient;
		private MFMailComposeViewController _mail;
		private UINavigationController _navigationController;
		private Action _done;

		public void SendEmail(string subject, string body, Action done = null)
		{
			_done = done ?? (() => {});
			if (MFMailComposeViewController.CanSendMail)
			{
				if (_mail == null)
				{
					_mail = new MFMailComposeViewController();
					_mail.Finished += MailSendFinished;
					Skin.Active.SkinNavigationBar(_mail.NavigationBar);
				}
				_mail.SetToRecipients(new [] { _recepient });
				_mail.SetSubject(subject);
				_mail.SetMessageBody(body, false);
				_navigationController.PresentViewController(_mail, true, null);
			}
		}

		private void MailSendFinished(object sender, MFComposeResultEventArgs e)
		{
			if (e.Result == MFMailComposeResult.Failed)
			{
				UIAlertView alert = new UIAlertView("Mail Alert", "The email could not be sent", null, "OK", null);
				alert.Show();
			}
			_navigationController.DismissViewController(true, () => {
				_mail.Dispose();
				_mail = null;
				_done();
			});
		}
	}
}

