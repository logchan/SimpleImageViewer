using System.Windows.Input;

namespace SimpleImageViewer.UI {
    partial class MainWindow {
        public static RoutedCommand NavigatePreviousCommand { get; } = new RoutedCommand();

        public void NavigatePreviousCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public void NavigatePreviousCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            NavigateImage(-1);
        }

        public static RoutedCommand NavigateNextCommand { get; } = new RoutedCommand();

        public void NavigateNextCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public void NavigateNextCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            NavigateImage(1);
        }

        public static RoutedCommand RotateLeftCommand { get; } = new RoutedCommand();

        public void RotateLeftCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public void RotateLeftCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            TheCanvas.Rotation += 270;
            TheCanvas.Rotation %= 360;
            TheCanvas.InvalidateVisual();
        }

        public static RoutedCommand RotateRightCommand { get; } = new RoutedCommand();

        public void RotateRightCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public void RotateRightCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            TheCanvas.Rotation += 90;
            TheCanvas.Rotation %= 360;
            TheCanvas.InvalidateVisual();
        }
    }
}
