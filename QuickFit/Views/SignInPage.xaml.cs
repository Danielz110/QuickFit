using QuickFit.ViewsModels;

namespace QuickFit.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
		this.BindingContext = new SignInViewModel(Navigation);
	}
}
