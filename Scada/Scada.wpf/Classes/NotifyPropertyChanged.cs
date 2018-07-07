using System.ComponentModel;

namespace Scada.wpf.Classes
{
    /// <summary>
    /// Notify Property Changed to Dependecy Properties 
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
