using System.Collections.Generic;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// This exception is thrown by the <see cref="PropertyGridHelper.UpdateObject"/> method 
    /// if any exception is thrown while updating the properties of the object edited using the property-grid editor 
    /// </summary>
    public class PropertyGridHelperException : PresentationException
    {
        protected List<PropertyGridHelperPropertyExInfo> propertiesExceptions;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="propertiesExceptions">A dictionary to collect any exception thrown while updating the properties of the object edited using the property-grid editor</param>
        public PropertyGridHelperException(List<PropertyGridHelperPropertyExInfo> propertiesExceptions) : base()
        {
            this.propertiesExceptions = propertiesExceptions;
        }

        /// <summary>
        /// The exceptions thrown while updating the properties of the object edited using the property-grid editor 
        /// </summary>
        public List<PropertyGridHelperPropertyExInfo> PropertiesExceptions
        {
            get
            {
                return propertiesExceptions;
            }
        }

    }
}

