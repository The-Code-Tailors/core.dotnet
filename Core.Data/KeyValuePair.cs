using System.Text.RegularExpressions;

namespace com.fabioscagliola.Core.Data
{
    public class KeyValuePair<KeyType, ValueType>
    {
        protected const string SEPARATOR = "=";

        public KeyType Key { get; set; }
        public ValueType Value { get; set; }

        public override string ToString()
        {
            return $"{Key}{SEPARATOR}{Value}";
        }

    }

    public class StringKeyValuePair : KeyValuePair<string, string>
    {
        public static bool TryParse(string s, out StringKeyValuePair stringKeyValuePair)
        {
            bool result = false;

            stringKeyValuePair = null;

            try
            {
                string[] temp = Regex.Split(s, $@"((?<!\\){SEPARATOR})");
                stringKeyValuePair = new StringKeyValuePair();
                stringKeyValuePair.Key = temp[0];
                stringKeyValuePair.Value = temp[2].Replace("\\=", SEPARATOR);
                result = true;
            }
            catch
            {
                // Do nothing 
            }

            return result;
        }

    }
}

