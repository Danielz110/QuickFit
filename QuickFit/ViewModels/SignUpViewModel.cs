using System;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Maui.Storage;
using QuickFit.Helper;
using QuickFit.Views;

namespace QuickFit.ViewsModels
{
    public class SignUpViewModel: BaseViewModel
    {
        #region Properties
        public Command SignInButtonCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public Command SignInCommand { get; set; }
       
        public string userName;
        public string  Name
        {
            get => userName; set{
                userName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

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
       
        #endregion

        FirebaseAuthProvider firebaseAuthProvider;
        public SignUpViewModel(INavigation navigation)
        {

            this.Navigation = navigation;
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseKey.apiKey));
            SignInButtonCommand = new Command(async () =>
            await this.Navigation.PushAsync(new SignInPage()));
            SignUpCommand = new Command(async () => await SignUpUser());
        }
        
        private async Task SignUpUser()
        {
            try
            {
                IsLoading = true;
                var auth = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(Email,
                    Password,Name);
                var user =  auth.FirebaseToken;
                if (user != null)
                {
                    await this.Navigation.PushAsync(new SignInPage());
                    Password = string.Empty;
                    Email = string.Empty;
                    Name = string.Empty;


                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Opps!", "Error kindly try again.", "OK"); ;

                }
                
            }
            catch (FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Opps!",$"Error {ex.Reason}", "OK");
            }

            IsLoading = false;
        }
    }
}

