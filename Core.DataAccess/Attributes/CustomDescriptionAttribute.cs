using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Specifies the name and the type of the resource from wich to retrieve the description for the property 
    /// </summary>
    public class CustomDescriptionAttribute : DescriptionAttribute
    {
        private Type resourceType;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="resourceName">The name of the resource from wich to retrieve the description for the property</param>
        /// <param name="resourceType">The type of the resource from wich to retrieve the description for the property</param>
        public CustomDescriptionAttribute(string resourceName, Type resourceType) : base(resourceName)
        {
            this.resourceType = resourceType;
        }

        public override string Description
        {
            get
            {
                string resourceName = base.Description;

                PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);

                if (property == null)
                {
                    property = resourceType.GetRuntimeProperties().Where(x => x.Name == resourceName).FirstOrDefault();

                    if (property == null)
                    {
                        return resourceName;
                    }
                }

                return property.GetValue(null, null) as string;
            }
        }

    }
}

