using System.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;
using GeolocatorPlugin;
using GeolocatorPlugin.Abstractions;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using QuickFit.Models;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace QuickFit.Views;

public partial class MainPage : ContentPage
{

    FirebaseClient firebaseClient;
    public MainPage()
    {
        InitializeComponent();
        firebaseClient = new FirebaseClient(Helper.FirebaseKey.databaseKey);
        var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
        var id = userId.User.Email;
        lblUserName.Text = id.ToString();

    }

    private Location _location;
    private Double lat;
    private Double lan;
    double index = 0;

    private async void MapPin()
    {
        try
        {
            Dispatcher.Dispatch(() =>
            {

                Position location = new Position()
                {

                    Latitude = lat,
                    Longitude = lan,
                    IsFromMockProvider = true
                };
                _location = new Location();
                _location.Latitude = location.Latitude;
                _location.Longitude = location.Longitude;

            });

            IDispatcherTimer timer;

            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {

                if (startPosition != null || endPosition != null)
                {
                    Location startLocation = new Location(startPosition.Latitude, startPosition.Latitude);
                    Location endLocation = new Location(endPosition.Latitude, endPosition.Latitude);

                    double kilometers = Location.CalculateDistance(startLocation, endLocation, DistanceUnits.Kilometers);
                    lblDistance.Text = Math.Round(kilometers, 2).ToString()+"km";
                }

                Location location = new Location()
                {

                    Latitude = lat,
                    Longitude = lan,
                    IsFromMockProvider = true
                };

                _location.Latitude = location.Latitude;
                _location.Longitude = location.Longitude;
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(_location, Distance.FromMeters(1));
                map.MoveToRegion(mapSpan);
            };

            timer.Start();
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opss Error!", ex.Message.ToString(), "OK");
        }

    }
    public async void ToggleGPS(bool toggleOn)
    {
        try
        {

            if (toggleOn)
            {
                if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 5, true, new ListenerSettings
                {
                    ActivityType = ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = false,
                    ListenForSignificantChanges = false,
                    PauseLocationUpdatesAutomatically = false,
                }))
                {
                    CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                }
            }
            else
            {
                if (await CrossGeolocator.Current.StopListeningAsync())
                {
                    CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
                }
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps Error!", ex.Message.ToString(), "OK");
        }
    }

    private async void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
    {
        await App.Current.MainPage.DisplayAlert("Opps Error!", e.Error.ToString(), "OK");
    }

    private Position startPosition;
    private Position endPosition;
    private async void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
    {

        try
        {
            if (startPosition == null)
            {
                startPosition = e.Position;
            }
            endPosition = e.Position;
            lat = e.Position.Latitude;
            lan = e.Position.Longitude;
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps Error!", ex.Message.ToString(), "OK");
        }

    }
    private bool isRuning;
    private TimeOnly time = new();
    private async void OnStartStopTimer()
    {
        if (isRuning == false)
            return;
        while (isRuning)
        {
            time = time.Add(TimeSpan.FromSeconds(1));
            SetTime();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    private void SetTime()
    {
        lblTime.Text = $"{time.Hour}:{time.Minute}:{time.Second}";
    }
   
    bool isWorkoutStart;
    async void StartButton_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {

            PermissionStatus permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (permissionStatus == PermissionStatus.Granted)
            {
                if (isWorkoutStart == true || CrossGeolocator.Current.IsListening)
                {
                    StartButton.Text = "Start Workout";
                    time = new TimeOnly();
                    Preferences.Set("time",lblTime.Text);
                    Preferences.Set("distance",lblDistance.Text);
                    lblDistance.Text = string.Empty;
                    isRuning = false;
                    isWorkoutStart = false;
                    await CrossGeolocator.Current.StopListeningAsync();
                    
                }
                else

                if (isWorkoutStart == false)
                {
                    
                    isRuning = true;
                    isWorkoutStart = true;
                    StartButton.Text = "Stop Workout";
                    ToggleGPS(true);
                    await Task.Delay(2000);
                    MapPin();
                    OnStartStopTimer();

                }
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps!",ex.Message.ToString(),"OK");
        }
    }

    async void SaveButton_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var WoroutTime = Preferences.Get("time", null);

            if (WoroutTime == null)
            {
                await App.Current.MainPage.DisplayAlert("Opps", "Kindly add new workout.", "Ok");

            }
            else
            {
                var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
                var id = userId.User.LocalId;
                var val = await firebaseClient.Child("WorOutTable").PostAsync(new WorkoutModel()
                {
                    UserId = id,
                    WoroutTime = Preferences.Get("time", null),
                    WorkoutDistance = Preferences.Get("distance", null)

                });
                Preferences.Remove("time");
                Preferences.Remove("distance");
                await App.Current.MainPage.DisplayAlert("Alright!", "Worout save!!!", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Opps Error", ex.Message.ToString(), "OK");
        }
    }
}
