using System;
using System.IO;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace SimpleImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DataDir =>
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "co.logu.SimpleImageViewer");
        public static Settings Settings { get; private set; } = new Settings();

        private static string SettingsFile => Path.Combine(DataDir, "settings.json");

        public App()
        {
            InitializeComponent();

            Directory.CreateDirectory(DataDir);

            if (File.Exists(SettingsFile))
            {
                try
                {
                    using (var fs = new StreamReader(SettingsFile, Encoding.UTF8))
                    {
                        Settings = (Settings)CreateSerializer().Deserialize(fs, typeof(Settings));
                    }
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        private static JsonSerializer CreateSerializer()
        {
            return new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                CheckAdditionalContent = false,
                Formatting = Formatting.Indented
            };
        }

        public static void SaveSettings()
        {
            try
            {
                using (var fs = new StreamWriter(SettingsFile, false, Encoding.UTF8))
                {
                    CreateSerializer().Serialize(fs, Settings);
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }
}
