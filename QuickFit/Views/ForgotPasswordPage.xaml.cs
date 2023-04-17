using QuickFit.ViewsModels;

namespace QuickFit.Views;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
		this.BindingContext = new ResetPasswordPageViewModel(Navigation);
	}
}
