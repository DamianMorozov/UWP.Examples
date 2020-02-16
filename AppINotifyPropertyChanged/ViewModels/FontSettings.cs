using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppINotifyPropertyChanged.ViewModels
{
    public class FontSettings : INotifyPropertyChanged
    {
        #region Constructor
        
        public FontSettings()
        {
            Size = 22;
        }

        #endregion

        #region Public fields and properties

        private int _size;
        public int Size
        {
            get => _size;
            set 
            {
                _size = value;
                OnPropertyRaised();
            }
        }
        
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
