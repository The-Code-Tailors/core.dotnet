using System;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Specifies the dictionary that holds the string resources for an enum 
    /// whose values are to be converted using a <see cref="DictionaryEnumConverter"/> 
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, Inherited = true, AllowMultiple = false)]
    public class DictionaryTypeAttribute : Attribute
    {
        protected Type dictionaryType;

        /// <summary>
        /// Specifies the dictionary that holds the string resources for an enum 
        /// whose values are to be converted using a <see cref="DictionaryEnumConverter"/> 
        /// </summary>
        /// <param name="dictionaryType">The type of the dictionary that holds the string resources for the enum</param>
        public DictionaryTypeAttribute(Type dictionaryType)
        {
            this.dictionaryType = dictionaryType;
        }

        public Type DictionaryType
        {
            get
            {
                return dictionaryType;
            }
        }

    }
}

