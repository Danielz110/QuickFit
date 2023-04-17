using System;
using System.Collections.ObjectModel;
using System.Linq;
using Firebase.Database;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using QuickFit.Models;

namespace QuickFit.ViewModels
{
    public class HomeViewModel
    {
        public ObservableCollection<WorkoutModel> Workouts { get; set; }
        public ObservableCollection<FoodScanModel> Breakfast { get; set; }
        public ObservableCollection<FoodScanModel> Lunch { get; set; }
        public ObservableCollection<FoodScanModel> Dinner { get; set; }

        private FirebaseClient firebaseClient;
        public HomeViewModel()
        {

            firebaseClient = new FirebaseClient(Helper.FirebaseKey.databaseKey);
            Workouts = new ObservableCollection<WorkoutModel>();
            Breakfast = new ObservableCollection<FoodScanModel>();
            Lunch = new ObservableCollection<FoodScanModel>();
            Dinner = new ObservableCollection<FoodScanModel>();

            GetAllWorkout();
            GetFoodScan();
        }
        public async void GetAllWorkout()
        {
            try
            {

                var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
                var id = userId.User.LocalId;
                var collection = firebaseClient
                .Child("WorOutTable")
                .AsObservable<WorkoutModel>()
                .Subscribe((item) =>
                {
                    if (item.Object.UserId == id)
                    {
                        Workouts.Add(item.Object);
                    }
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Opps!!",ex.Message.ToString(),"Ok");
            }
        }
        public async void GetFoodScan()
        {
            try
            {

                var userId = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("userkey", null));
                var id = userId.User.LocalId;
                var items = await GetAllBreakfast();

                foreach (var item in items)
                {
                    if (item.FoodTime == "Breakfast" && item.UserId == id)
                    {
                        Breakfast.Add(item);

                    }
                }

                foreach (var item in items)
                {
                    if (item.FoodTime == "Lunch" && item.UserId == id)
                    {

                        Lunch.Add(item);

                    }
                }

                foreach (var item in items)
                {
                    if (item.FoodTime == "Dinner" && item.UserId == id)
                    {
                        Dinner.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Opps!!", ex.Message.ToString(), "Ok");
            }
        }
        public async Task<List<FoodScanModel>> GetAllBreakfast()
        {

            return (await firebaseClient
              .Child("FoodScaning")
              .OnceAsync<FoodScanModel>()).Select(item => new FoodScanModel
              {
                  UserId = item.Object.UserId,
                  FoodInfo = item.Object.FoodInfo,
                  ImageUrl= item.Object.ImageUrl,
                  FoodTime = item.Object.FoodTime
              }).ToList();
        }
    }
}
