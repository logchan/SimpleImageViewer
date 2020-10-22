using Tklc.Wpf.UI;

namespace SimpleImageViewer {
    public class Settings {
        public WindowPosition LastPosition { get; set; } = new WindowPosition();
        public bool Chessboard { get; set; } = false;
        public bool DrawFrame { get; set; } = false;
    }
}
