using System;
using Firebase.Auth;
using QuickFit.Helper;
using QuickFit.Views;

namespace QuickFit.ViewsModels
{
	public class ResetPasswordPageViewModel:BaseViewModel
	{
		public Command ChangePasswordCommand { get; set; }

		public string _eemail;
		public string Email {

			get => _eemail;
			set {
				_eemail = value;
				OnPropertyChanged("Email");
			}
		}
        FirebaseAuthProvider firebaseAuthProvider;
        public ResetPasswordPageViewModel(INavigation navigation)
		{
			this.Navigation = navigation;
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseKey.apiKey));
            ChangePasswordCommand = new Command(async () => await ChangePassword()); 

        }

        private async Task ChangePassword()
        {
			try
			{
				var isChangePassword =  firebaseAuthProvider.SendPasswordResetEmailAsync(Email);
				
				if (isChangePassword != null)
				{
					await this.Navigation.PushAsync(new NewPasswordPage());
				}
				else
				{
					await App.Current.MainPage.DisplayAlert("Opps Error!","Password not change please try again.","OK");
				}

            }
			catch (FirebaseAuthException ex)
			{
                await App.Current.MainPage.DisplayAlert("Opps Error!",ex.Reason.ToString(), "OK");
            }
        }
    }
}

