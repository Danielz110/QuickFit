using QuickFit.ViewsModels;

namespace QuickFit.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
		this.BindingContext = new SignUpViewModel(Navigation);
	}
}
