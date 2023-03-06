namespace QuickFit;

public partial class NutritionPage : ContentPage
{
	public NutritionPage()
	{
		InitializeComponent();
	}

	private async void ScanButtonClick(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ScanPage));
	}
}