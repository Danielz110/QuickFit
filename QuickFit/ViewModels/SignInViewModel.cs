using System;
using Firebase.Auth;
using QuickFit.Helper;
using QuickFit.Views;
using Newtonsoft.Json;

namespace QuickFit.ViewsModels
{

    public class SignInViewModel: BaseViewModel
    {
        #region properties
        public Command SingUpButtonComman { get; set; }
        public Command SinInButtonCommand { get; set; }
        public Command ResetPasswordCommand { get; set; }


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
        public SignInViewModel(INavigation navigation)
        {
           
            this.Navigation = navigation;
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseKey.apiKey));
            ResetPasswordCommand= new Command(async () =>  await this.Navigation.PushAsync(new ForgotPasswordPage()));
            SinInButtonCommand = new Command(async () => await SignIn());
            SingUpButtonComman = new Command(async () => await OpenSignUpPage());
        }

        private async Task ResetPassword()
        {
            await this.Navigation.PushAsync(new NewPasswordPage());
        }

        private async Task OpenSignUpPage()
        {
            await this.Navigation.PushAsync(new Views.SignUpPage());

        }

        /// <summary>
        /// This function will check user sign in proccess.
        /// </summary>
        /// <returns></returns>
        private async Task SignIn()
        {
            IsLoading = true;
            try
            {

                
                var userid = firebaseAuthProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                var content = userid.Exception;
                var data = await userid.Result.GetFreshAuthAsync();

                var  userData = JsonConvert.SerializeObject(data);
                Preferences.Set("userkey",userData);

                if (content == null)
                {
                    await this.Navigation.PushAsync(new ApplicationTabbsPage());
                    Password = string.Empty;
                    Email = string.Empty;
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
            catch (System.AggregateException ex) {
                await App.Current.MainPage.DisplayAlert("Opps Error!", ex.Message.ToString(), "OK");
            }
            IsLoading = false;
            }
        }
       
    }


