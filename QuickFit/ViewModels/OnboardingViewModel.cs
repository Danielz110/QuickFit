using System;
using System.Collections.ObjectModel;
using QuickFit.Models;
using QuickFit.ViewsModels;

namespace QuickFit.ViewsModels
{
	public class OnboardingViewModel: BaseViewModel
	{
        public ObservableCollection<OnboardingModel> Onboardings{ get; set; }
        public OnboardingViewModel()
		{
            Onboardings = new ObservableCollection<OnboardingModel>();
            LoadData();
        }
        private void LoadData()
        {
            var data = GetOnOnboardingData();
            foreach (var item in data)
            {
                Onboardings.Add(item);
            }
        }
		private  List<OnboardingModel>GetOnOnboardingData()
		{
            List<OnboardingModel> onboardings = new List<OnboardingModel>()
            {
				new OnboardingModel
				{
					ImageName="onboarding_icon.png",
					Description="Track your progress and grow bit by bit."

                },

                new OnboardingModel
                {
                    ImageName="onboarding_icon.png",
                    Description="Start your morning with workout."

                },
                new OnboardingModel
                {
                    ImageName="onboarding_icon.png",
                    Description="The future of healthy lifestyle."

                }
            };
            
			return  onboardings;
		}
	}
}

