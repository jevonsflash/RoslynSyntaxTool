using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Workshop.View;

namespace Workshop.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IndexPageViewModel>();
            SimpleIoc.Default.Register<SettingPageViewModel>();

        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public IndexPageViewModel IndexPage => ServiceLocator.Current.GetInstance<IndexPageViewModel>();
        public SettingPageViewModel SettingPage => ServiceLocator.Current.GetInstance<SettingPageViewModel>();


        public static void Cleanup<T>() where T : class
        {
            SimpleIoc.Default.Unregister<T>();
            SimpleIoc.Default.Register<T>();
        }
    }
}