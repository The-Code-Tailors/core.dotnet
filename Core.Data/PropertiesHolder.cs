using System.Collections.Generic;

namespace com.fabioscagliola.Core.Data
{
    public abstract class PropertiesHolder
    {
        public List<Property> Properties { get; set; }

        public PropertiesHolder()
        {
            Properties = new List<Property>();
        }

        public string this[string key]
        {
            get
            {
                string value = null;
                Property property = GetByKey(key);
                if (property != null)
                {
                    value = property.Value;
                }
                return value;
            }
            set
            {
                Property property = GetByKey(key);
                if (property == null)
                {
                    Properties.Add(new Property { Key = key, Value = value });
                }
                else
                {
                    property.Value = value;
                }
            }
        }

        public Property GetByKey(string key)
        {
            return Properties.Find(property => property.Key == key);
        }

        public Property GetByValue(string value)
        {
            return Properties.Find(property => property.Value == value);
        }

    }
}

