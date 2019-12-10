using FontAwesome5;

namespace VaultBoy.ViewModel.Tabs
{
    public class StartPageViewModel : TabModelBase
    {
        public override string TabTitle => "Welcome";

        public override EFontAwesomeIcon TabIcon => EFontAwesomeIcon.Solid_InfoCircle;

        public override bool CanClose => false;
    }
}
