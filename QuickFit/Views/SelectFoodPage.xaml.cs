using Newtonsoft.Json;
using QuickFit.Models;
using QuickFit.ViewModels;

namespace QuickFit.Views;

public partial class SelectFoodPage : ContentPage
{
	public SelectFoodPage()
	{
		InitializeComponent();
		this.BindingContext = new SelectFoodViewModel(Navigation);
        var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
        var id = userId.User.Email;
        lblUserName.Text = id.ToString();

    }

    async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		var timeModel = (sender as View).BindingContext as FoodTime;
		var model = new FoodTime();
        model = timeModel;

        await this.Navigation.PushModalAsync(new FoodScanerPage(model));
    }
}
