using FontAwesome5;
using VaultBoy.ViewModel.Tabs.ClassInfo;
using VaultLib.Core.Data;

namespace VaultBoy.ViewModel.Tabs
{
    public class ClassInfoViewModel : TabModelBase
    {
        private VLTClass _class;
        private StaticDataProxy _dataProxy;

        public ClassInfoViewModel(VLTClass @class)
        {
            Class = @class;
            DataProxy = new StaticDataProxy(Class);
        }

        public VLTClass Class
        {
            get => _class;
            set
            {
                _class = value;
                RaisePropertyChanged();
            }
        }

        public StaticDataProxy DataProxy
        {
            get => _dataProxy;
            private set
            {
                _dataProxy = value;
                RaisePropertyChanged();
            }
        }

        public override string TabTitle => $"Info - {Class.Name}";

        public override EFontAwesomeIcon TabIcon => EFontAwesomeIcon.Solid_Code;
    }
}
