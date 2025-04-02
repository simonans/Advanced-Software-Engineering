using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(backingField, value)) return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}