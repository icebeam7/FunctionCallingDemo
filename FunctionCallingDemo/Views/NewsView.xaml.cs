using FunctionCallingDemo.ViewModels;

namespace FunctionCallingDemo.Views;

public partial class NewsView : ContentPage
{
    NewsViewModel vm;

    public NewsView(NewsViewModel vm)
	{
		InitializeComponent();
		
		this.vm = vm;
		BindingContext = vm;
	}
}