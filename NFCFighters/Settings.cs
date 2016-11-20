using Newtonsoft.Json;
using System.IO;

namespace NFCFighters
{
	public class Settings
	{
		public bool invertControls { get; set; }
		public int colorConfig { get; set; }
        public bool nightmode { get; set; }

        public Settings(Settings set)
		{
            invertControls = set.invertControls;
            colorConfig = set.colorConfig;
            nightmode = set.nightmode;
		}

        public Settings() : this(LoadSettings()) {}

        public Settings(bool def)
        {
            if (def)
            {
                invertControls = false;
                colorConfig = Color.COLOR_GREEN;
                nightmode = false;
            }
        }

        public static Settings LoadSettings()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "settings.json");

            Settings settings = new Settings(true);
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        settings = serializer.Deserialize<Settings>(reader);
                    }
                }
            }
            catch (IOException) { }

            return settings;
        }

        public static bool SaveSettings(Settings settings)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "settings.json");

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer,settings);
                }
            }

            return true;
        }
    }
}
