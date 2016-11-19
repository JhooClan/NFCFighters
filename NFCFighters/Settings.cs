using Newtonsoft.Json;
using System.IO;

namespace NFCFighters
{
	public class Settings
	{
		public bool isLeftHanded { get; set; }
		public string isColorConfig { get; set; }
        public bool isNightmode { get; set; }

        public Settings(Settings set)
		{
            isLeftHanded = set.isLeftHanded;
            isColorConfig = set.isColorConfig;
            isNightmode = set.isNightmode;
		}

        public Settings() : this(LoadSettings()) {}

        public Settings(bool def)
        {
            if (def)
            {
                isLeftHanded = false;
                isColorConfig = "color1";
                isNightmode = false;
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
            catch (IOException ex) { }

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
