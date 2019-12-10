using GalaSoft.MvvmLight.Ioc;
using VaultBoy.Editing;
using VaultBoy.Services;
using VaultBoy.ViewModel.Tabs;

namespace VaultBoy.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.Default.Register<ProfileService>();
            SimpleIoc.Default.Register<DatabaseService>();
            SimpleIoc.Default.Register<StartPageViewModel>();
            SimpleIoc.Default.Register<CollectionEditorViewModel>();
            SimpleIoc.Default.Register<VLTDataProxy>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }
    }
}