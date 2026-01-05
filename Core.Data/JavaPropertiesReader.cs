using System.IO;

namespace com.fabioscagliola.Core.Data
{
    public class JavaPropertiesReader : PropertiesHolder
    {
        public void Read(string path)
        {
            string[] lineList = File.ReadAllLines(path);

            foreach (string line in lineList)
            {
                if (!line.StartsWith("#"))
                {
                    int i = line.IndexOf("=");

                    if (i != -1)
                    {
                        Property property = new Property();
                        property.Key = line.Substring(0, i).Trim();
                        property.Value = line.Substring(i + 1).Trim();
                        Properties.Add(property);
                    }
                }
            }
        }

    }
}

