using InputKit.Shared.Controls;
using QuickFit.ViewsModels;

namespace QuickFit.Views;

public partial class NewPasswordPage : ContentPage
{
	public NewPasswordPage()
	{
		InitializeComponent();
		this.BindingContext = new NewPasswordPageViewModeel(Navigation);
	}

}
