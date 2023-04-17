using Newtonsoft.Json;
using QuickFit.ViewModels;

namespace QuickFit.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel homeView;
    public HomePage()
	{
		InitializeComponent();
        homeView = new HomeViewModel();
        this.BindingContext = homeView;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {

            var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
            var id = userId.User.Email;
            lblUserName.Text = id.ToString();
            homeView.GetAllWorkout();
            homeView.GetAllWorkout();
            homeView.GetFoodScan();
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps!!", ex.Message.ToString(), "Ok");
        }
    }
}
