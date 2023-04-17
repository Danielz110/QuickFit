using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickFit.ViewsModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool isLoading;
        public bool IsLoading
        {
            get => isLoading; set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}


