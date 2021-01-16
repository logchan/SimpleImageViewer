using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Tklc.Drawing;
using Tklc.IO;
using Tklc.Wpf.Commands;
using Tklc.Wpf.Media;
using Tklc.Wpf.UI;

namespace SimpleImageViewer.UI {
    public partial class MainWindow {
        private string _loadedFile;
        private string _loadedDirectory;
        private double _dpiX = 96.0;
        private double _dpiY = 96.0;
        private Task _loadImagesTask;
        private List<string> _allFiles;
        private int _allFilesIdx = 0;

        public MainWindow() {
            InitializeComponent();
            this.RegisterRoutedCommands();

            App.Settings.LastPosition.Restore(this);
            TheCanvas.Chessboard = App.Settings.Chessboard;
            TheCanvas.DrawFrame = App.Settings.DrawFrame;
            TheCanvas.Focus();
        }

        private async void LoadImage(string file) {
            try {
                if (file.ToLower().EndsWith(".gif")) {
                    var gif = new GifImage();
                    await Task.Run(() => {
                        gif = new GifDecoder().Decode(file, _dpiX, _dpiY);
                        foreach (var frame in gif.Frames) {
                            frame.Image.Freeze();
                        }
                    });

                    var frames = new List<ImageViewerCanvas.FrameData>(gif.Frames.Count);
                    for (var i = 0; i < gif.Frames.Count; ++i) {
                        frames.Add(new ImageViewerCanvas.FrameData {
                            Duration = gif.Frames[(i + 1) % gif.Frames.Count].Delay,
                            Image = gif.Frames[i].Image
                        });
                    }
                    TheCanvas.SetFrames(frames);
                }
                else {
                    var image = await Task.Run(() => {
                        var bm = new BitmapImage();
                        bm.BeginInit();
                        bm.UriSource = new Uri(file);
                        bm.CacheOption = BitmapCacheOption.OnLoad;
                        bm.EndInit();
                        bm.Freeze();
                        return bm;
                    });

                    TheCanvas.SetImage(image);
                }

                _loadedFile = Path.GetFileName(file);
                _loadedDirectory = Path.GetDirectoryName(file);
                Title = _loadedFile;

                if (_loadImagesTask == null) {
                    StatusText = "Loading images";
                    _loadImagesTask = Task.Run(() => {
                        _allFiles = IOHelpers.GetFiles(
                            _loadedDirectory,
                            predicate: f => DrawingHelpers.CommonImageExtensions.Contains(f.Extension.ToLower()));
                        _allFilesIdx = _allFiles.IndexOf(_loadedFile);
                        Dispatcher.Invoke(() => StatusText = "Ready");
                    });
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TheWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            App.Settings.LastPosition.Update(this);
            App.Settings.Chessboard = TheCanvas.Chessboard;
            App.Settings.DrawFrame = TheCanvas.DrawFrame;
            App.SaveSettings();
        }

        private void TheWindow_Loaded(object sender, RoutedEventArgs e) {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                var fi = new FileInfo(args[1]);
                if (!fi.Exists) {
                    return;
                }

                LoadImage(fi.FullName);
            }
        }

        private void NavigateImage(int delta) {
            if (_allFiles == null || !_loadImagesTask.IsCompleted) {
                return;
            }

            var idx = _allFilesIdx + delta;
            if (idx < 0 || idx >= _allFiles.Count) {
                return;
            }

            _allFilesIdx = idx;
            LoadImage(Path.Combine(_loadedDirectory, _allFiles[idx]));
        }
    }
}
