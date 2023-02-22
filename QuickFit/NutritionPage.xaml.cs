namespace QuickFit;

public partial class NutritionPage : ContentPage
{
	public NutritionPage()
	{
		InitializeComponent();
	}
	private void CameraBarcodeReaderView_BarcodesDetected(object
		sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		Dispatcher.Dispatch(() =>
		{
			barcodeResult.Text = $"{e.Results[0].Value} {e.Results[0].Format}";
		});
	}
}