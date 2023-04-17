using System;
using QuickFit.ViewsModels;
using System.Collections.ObjectModel;
using QuickFit.Models;

namespace QuickFit.ViewModels
{
	public class SelectFoodViewModel: BaseViewModel
	{
		public ObservableCollection<FoodTime> FoodTimes { get; set; }
		public SelectFoodViewModel(INavigation navigation)
		{
            this.Navigation = navigation;
			FoodTimes = new ObservableCollection<FoodTime>();
            GetItems();

        }
		private void GetItems() {

			FoodTimes.Add(new FoodTime() {
				ImageUrl = "breakfast_icon.jpg",
				Title="Breakfast",
				Description= "Tap To Scan Barcode"
            });

            FoodTimes.Add(new FoodTime()
            {
                ImageUrl = "lunch_icon.jpg",
                Title = "Lunch",
                Description = "Tap To Scan Barcode"
            });

            FoodTimes.Add(new FoodTime()
            {
                ImageUrl = "dinner_icon.jpg",
                Title = "Dinner",
                Description = "Tap To Scan Barcode"
            });
        }
	}
}

