using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace SimpleImageViewer.UI
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapSource _loadedImage;
        public BitmapSource LoadedImage
        {
            get => _loadedImage;
            set
            {
                _loadedImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadedImage)));
            }
        }

        private string _statusText;
        public string StatusText {
            get => _statusText;
            set {
                _statusText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusText)));
            }
        }
    }
}
