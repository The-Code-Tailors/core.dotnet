using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace com.fabioscagliola.Core.Data
{
    /// <summary>
    /// Because dictionaries are not serializable 
    /// </summary>
    public class KeyValuePairList<KeyType, ValueType> : List<KeyValuePair<KeyType, ValueType>>
    {
        protected const string SEPARATOR = ",";

        public ValueType this[KeyType key]
        {
            get
            {
                KeyValuePair<KeyType, ValueType> keyValuePair = Find(key);

                if (keyValuePair != null)
                {
                    return keyValuePair.Value;
                }
                else
                {
                    throw new KeyValuePairListException("Key not found!");
                }
            }
            set
            {
                KeyValuePair<KeyType, ValueType> keyValuePair = Find(key);

                if (keyValuePair != null)
                {
                    keyValuePair.Value = value;
                }
                else
                {
                    Add(new KeyValuePair<KeyType, ValueType> { Key = key, Value = value });
                }
            }
        }

        public bool TryGet(KeyType key, out ValueType value)
        {
            KeyValuePair<KeyType, ValueType> keyValuePair = Find(key);

            if (keyValuePair != null)
            {
                value = keyValuePair.Value;
                return true;
            }
            else
            {
                value = default(ValueType);
                return false;
            }
        }

        protected KeyValuePair<KeyType, ValueType> Find(KeyType key)
        {
            return Find(keyValuePair => keyValuePair.Key.Equals(key));
        }

        public static StringKeyValuePairList FromNameValueCollection(NameValueCollection nameValueCollection)
        {
            StringKeyValuePairList stringKeyValuePairList = new StringKeyValuePairList();

            foreach (string name in nameValueCollection.Keys)
            {
                stringKeyValuePairList.Add(new KeyValuePair<string, string>() { Key = name, Value = nameValueCollection[name] });
            }

            return stringKeyValuePairList;
        }

        public NameValueCollection ToNameValueCollection()
        {
            if (typeof(KeyType) != typeof(string))
            {
                throw new ApplicationException("The key must be a string!");
            }

            if (typeof(ValueType) != typeof(string))
            {
                throw new ApplicationException("The value must be a string!");
            }

            NameValueCollection nameValueCollection = new NameValueCollection();

            foreach (KeyValuePair<KeyType, ValueType> keyValuePair in this)
            {
                nameValueCollection.Add(keyValuePair.Key as string, keyValuePair.Value as string);
            }

            return nameValueCollection;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<KeyType, ValueType> keyValuePair in this)
            {
                stringBuilder.Append(keyValuePair);
                stringBuilder.Append(SEPARATOR);
            }

            if (stringBuilder.Length != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - SEPARATOR.Length, SEPARATOR.Length);
            }

            return stringBuilder.ToString();
        }

    }

    public class StringKeyValuePairList : KeyValuePairList<string, string>
    {
        public static bool TryParse(string s, out StringKeyValuePairList stringKeyValuePairList)
        {
            bool result = false;

            stringKeyValuePairList = null;

            try
            {
                stringKeyValuePairList = new StringKeyValuePairList();

                foreach (string temp in s.Split(SEPARATOR.ToCharArray()))
                {
                    StringKeyValuePair stringKeyValuePair;

                    if (StringKeyValuePair.TryParse(temp, out stringKeyValuePair))
                    {
                        stringKeyValuePairList.Add(stringKeyValuePair);
                    }
                }

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

