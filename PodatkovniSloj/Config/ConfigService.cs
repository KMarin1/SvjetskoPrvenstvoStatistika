using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodatkovniSloj.Config
{
    public class ConfigService
    {
        public void SaveConfig(Dictionary<string, string> config, string path)
        {
            var lines = config.Select(kvp => $"{kvp.Key}={kvp.Value}");
            File.WriteAllLines(path, lines);
        }

        public Dictionary<string, string> LoadConfig(string path)
        {
            var dict = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(path))
            {

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    dict[parts[0]] = parts[1];
                }
            }
            return dict;
        }
    }
}
