

using QuickFit.Views;
using QuickFit.ViewsModels;
namespace QuickFit.Views
{

    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            InitializeComponent();
            this.BindingContext = new OnboardingViewModel();
        }

        async void NextButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }
    }
}