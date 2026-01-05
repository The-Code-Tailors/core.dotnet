using com.fabioscagliola.Core.Data;
using com.fabioscagliola.Core.DataAccess.Attributes;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// Converts the values of an enum to and from strings, based on a dictionary that holds the string resources for the enum; 
    /// the dictionary must be specified for the enum using the <see cref="DictionaryTypeAttribute"/> attribute; 
    /// if the enum lacks the <see cref="DictionaryTypeAttribute"/> attribute, an exception is thrown 
    /// </summary>
    public class DictionaryEnumConverter : EnumConverter
    {
        protected StringKeyValuePairList stringKeyValuePairList;

        public DictionaryEnumConverter(Type type) : base(type)
        {
            stringKeyValuePairList = new StringKeyValuePairList();

            //DictionaryTypeAttribute dictionaryTypeAttribute = (DictionaryTypeAttribute)TypeDescriptor.GetAttributes(type)[typeof(DictionaryTypeAttribute)];

            //if (dictionaryTypeAttribute == null)
            //{
            //    throw new DataAccessException($"The \"{type}\" enum lacks the {nameof(DictionaryTypeAttribute)} attribute!");
            //}

            //PropertyInfo property = dictionaryTypeAttribute.DictionaryType.GetProperty("Instance");

            //IEnumerable dictionary = property.GetValue(null) as IEnumerable;

            foreach (object item in GetEnumerableDictionary(type))
            {
                string key = item.GetType().GetProperty("Key").GetValue(item).ToString();

                string value = (string)item.GetType().GetProperty("Value").GetValue(item);

                stringKeyValuePairList.Add(new StringKeyValuePair() { Key = key, Value = value, });
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Enum.Parse(EnumType, stringKeyValuePairList.Find(x => x.Key == (string)value).Key);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return stringKeyValuePairList.Find(x => x.Key == value.ToString()).Value;
        }

        protected IEnumerable GetEnumerableDictionary(Type type)
        {
            DictionaryTypeAttribute dictionaryTypeAttribute = (DictionaryTypeAttribute)TypeDescriptor.GetAttributes(type)[typeof(DictionaryTypeAttribute)];

            if (dictionaryTypeAttribute == null)
            {
                throw new DataAccessException($"The \"{type}\" enum lacks the {nameof(DictionaryTypeAttribute)} attribute!");
            }

            PropertyInfo property = dictionaryTypeAttribute.DictionaryType.GetProperty("Instance");

            if (property == null)
            {
                throw new DataAccessException($"The Instance property of the \"{dictionaryTypeAttribute.DictionaryType}\" attribute class is null!");
            }

            return property.GetValue(null) as IEnumerable;

        }

    }
}

