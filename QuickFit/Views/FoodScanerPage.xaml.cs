using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using QuickFit.Models;
using ZXing.Net.Maui;

namespace QuickFit.Views;

public partial class FoodScanerPage : ContentPage
{
    IDispatcherTimer _timer;
    public FoodTime foodTime;
    FirebaseClient firebaseClient;
    public FoodScanerPage(FoodTime timeModel)
	{
        
        InitializeComponent();

        foodTime = new FoodTime();
        foodTime.Title = timeModel.Title;
        foodTime.ImageUrl = timeModel.ImageUrl;

        firebaseClient = new FirebaseClient(Helper.FirebaseKey.databaseKey);
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
       
    }
    
    async void Back_Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.Navigation.PopModalAsync();
    }

    async void SaveWorkOutButton_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(lblBarcode.Text))
            {
                await App.Current.MainPage.DisplayAlert("Worning", "Kindly scan the item.", "OK");
                return;
            }
            else
            {
                var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
                var id = userId.User.LocalId;
                var val = await firebaseClient.Child("FoodScaning").PostAsync(new FoodScanModel()
                {
                    UserId = id,
                    FoodInfo = lblFoodIngredient.Text,
                    FoodTime = foodTime.Title,
                    ImageUrl = foodTime.ImageUrl

                });
                lblFoodIngredient.Text = string.Empty;
                lblBarcode.Text = string.Empty;
                await App.Current.MainPage.DisplayAlert("Save", "Record added!!!", "OK");
            }

            
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps Error",ex.Message.ToString(),"OK");
        }

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        _timer = App.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += getFoodIno_Click;
        _timer.Start();
        

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _timer.Tick -= getFoodIno_Click;
        _timer.Stop();
    }

    private void getFoodIno_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(lblBarcode.Text))
            return;

        LoadInfo(Convert.ToInt64(lblBarcode.Text));
    }

    async void cameraBarcodeReaderView_BarcodesDetected(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        try
        {

            Dispatcher.Dispatch(() =>
            {
                lblBarcode.Text = e.Results[0].Value;
            });
            
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps!", ex.Message.ToString(), "OK");
        }
    }
    private async void LoadInfo(long barcode) {

        try
        {

            var barcodeeResult = await FoodIngredient(barcode);
            if (barcodeeResult == null)
            {
                await App.Current.MainPage.DisplayAlert("Opps!", "Noting found.", "OK");
                return;
            }
            else
            {
                lblFoodIngredient.Text = barcodeeResult;
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps!", ex.Message.ToString(), "OK");
        }
    }

    private async Task<string> FoodIngredient(long barCode)
    {
        try
        {

            var result = await GetAllFoodInfo();
            foreach (var item in result)
            {
                if (item.Barcode == barCode)
                {
                    return item.FoodIngredient;
                }
            }

            
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps!", ex.Message.ToString(), "OK");
        }
        return null;
    }
    public async Task<List<FoodIngredientModel>> GetAllFoodInfo()
    {

        return (await firebaseClient
          .Child("FoodIngredientTable")
          .OnceAsync<FoodIngredientModel>()).Select(item => new FoodIngredientModel
          {
              Barcode = item.Object.Barcode,
              FoodIngredient = item.Object.FoodIngredient
          }).ToList();
    }
   
}
