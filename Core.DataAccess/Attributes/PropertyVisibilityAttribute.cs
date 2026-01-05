using System;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Controls the visibility of a property in the UI based on the value of the visibility level of the app: 
    /// the property will be visible if the visibility level of the app is equal to or grater than the specified minimum level; 
    /// if the attribute is not specified, the minimum level will be zero 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertyVisibilityAttribute : Attribute
    {
        readonly PropertyVisibilityLevel minLevel;

        /// <summary>
        /// Controls the visibility of a property in the UI based on the value of the visibility level of the app: 
        /// the property will be visible if the visibility level of the app is equal to or grater than the specified minimum level; 
        /// if the attribute is not specified, the minimum level will be zero 
        /// </summary>
        /// <param name="minLevel">The minimum UI level for the property to be visible</param>
        public PropertyVisibilityAttribute(PropertyVisibilityLevel minLevel = PropertyVisibilityLevel.Level0)
        {
            this.minLevel = minLevel;
        }

        public PropertyVisibilityLevel MinLevel
        {
            get
            {
                return minLevel;
            }
        }

    }
}

