using System;
using System.ComponentModel;
using System.Reflection;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Specifies the name and the type of the resource from wich to retrieve the category in which to group the property 
    /// when displayed in a PropertyGrid control set to Categorized mode 
    /// </summary>
    public class CustomCategoryAttribute : CategoryAttribute
    {
        private Type resourceType;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="resourceName">The name of the resource from wich to retrieve the category in which to group the property</param>
        /// <param name="resourceType">The type of the resource from wich to retrieve the category in which to group the property</param>
        public CustomCategoryAttribute(string resourceName, Type resourceType) : base(resourceName)
        {
            this.resourceType = resourceType;
        }

        protected override string GetLocalizedString(string value)
        {
            string resourceName = Category;

            PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);

            if (property == null)
            {
                return resourceName;
            }

            return property.GetValue(null, null) as string;
        }

    }
}

