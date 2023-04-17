using System;
using Firebase.Auth;
using QuickFit.Helper;
using Newtonsoft.Json;
using QuickFit.Views;

namespace QuickFit.ViewsModels
{
	public class NewPasswordPageViewModeel: BaseViewModel
	{
        public string userEmail;
        public string Email
        {
            get => userEmail; set
            {
                userEmail = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string userPassword;
        public string Password
        {
            get => userPassword; set
            {
                userPassword = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public Command ToHomePageCommand { get; set; }
        FirebaseAuthProvider firebaseAuthProvider;
        public NewPasswordPageViewModeel(INavigation navigation)
		{
            this.Navigation = navigation;
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseKey.apiKey));
            ToHomePageCommand = new Command(async () => await NavigateToHome());
		}

        private async Task NavigateToHome()
        {
            try
            {

                IsLoading = true;
                var userid = firebaseAuthProvider.SignInWithEmailAndPasswordAsync(Email,
                    Password);
                var content = userid.Result.User;
                var userData = JsonConvert.SerializeObject(content);

                if (content.LocalId != null)
                {
                    await this.Navigation.PushAsync(new ApplicationTabbsPage());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Opps Error!", "Invalid email password try again", "OK");
                }
            }
            catch (FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Opps Error!", ex.Reason.ToString(), "OK");

            }
        }
    }
}

