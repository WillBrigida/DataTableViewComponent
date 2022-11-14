namespace Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        this.BindingContext = MauiProgram.ServiceProvider.GetRequiredService<MainPageViewModel>();
    }

    protected async override void OnAppearing()
    {
        if (((BindingContext as MainPageViewModel).Items?.Count ?? 0) == 0)
            await (BindingContext as MainPageViewModel).LoadItems();

        //await Task.Delay(1000).ContinueWith(_ => DTC.Teste(DTC));


        //Após o carregamento, a lista é rolada até o ultimo item
        //await Task.Delay(1000).ContinueWith(_ => TableViewPage.ScrollToBottom(TableViewPage));
    }

}


