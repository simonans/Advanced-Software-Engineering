using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

        public abstract class ViewModelBase : INotifyPropertyChanged
        {
            // Dieses Event wird ausgelöst, wenn sich eine Eigenschaft ändert
            public event PropertyChangedEventHandler PropertyChanged;

            // Methode zur Benachrichtigung, dass eine Eigenschaft sich geändert hat
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            // Hilfsmethode, um eine Property zu setzen und die Änderung mitzuteilen
            protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
            {
                if (Equals(backingField, value)) return false;

                backingField = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
