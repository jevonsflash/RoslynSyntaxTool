using CommunityToolkit.Mvvm.DependencyInjection;


namespace Workshop.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {


        }
        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();
        public IndexPageViewModel IndexPage => Ioc.Default.GetRequiredService<IndexPageViewModel>();
        public SettingPageViewModel SettingPage => Ioc.Default.GetRequiredService<SettingPageViewModel>();


        public static void Cleanup<T>() where T : class
        {
        }
    }
}