namespace QuickFit.Comman;

public partial class BackButton : ContentView
{
	public BackButton()
	{
		InitializeComponent();
	}

    async void Back_Button_Clicked(System.Object sender, System.EventArgs e)
    {
		await this.Navigation.PopAsync(true);
    }
}
