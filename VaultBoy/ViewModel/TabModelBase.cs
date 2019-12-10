using System;
using FontAwesome5;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace VaultBoy.ViewModel
{
    public abstract class TabModelBase : ViewModelBase
    {
        private bool _isSelected;
        public abstract string TabTitle { get; }

        public RelayCommand<TabModelBase> CloseCommand { get; set; }

        public virtual EFontAwesomeIcon TabIcon
        {
            get { return EFontAwesomeIcon.None; }
        }

        public virtual bool IsEnabled
        {
            get { return true; }
        }

        public virtual bool CanClose
        {
            get { return true; }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        protected void SendMessage<T>(T message) where T : MessageBase
        {
            this.MessengerInstance.Send(message);
        }

        protected void RegisterMessage<T>(Action<T> handler) where T : MessageBase
        {
            this.MessengerInstance.Register(this, handler);
        }
    }
}
