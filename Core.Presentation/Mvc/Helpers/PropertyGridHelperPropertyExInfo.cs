using System;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// Holds information about an exception thrown while updating a property of an object edited using a property-grid editor 
    /// </summary>
    public class PropertyGridHelperPropertyExInfo
    {
        protected PropertyGridHelperProperty property;

        protected Exception exception;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="property">The property being updated when the exception was thrown</param>
        /// <param name="exception">The exception thrown while updating the property</param>
        public PropertyGridHelperPropertyExInfo(PropertyGridHelperProperty property, Exception exception)
        {
            this.property = property;
            this.exception = exception;
        }

        /// <summary>
        /// The property being updated when the exception was thrown 
        /// </summary>
        public PropertyGridHelperProperty Property { get => property; }

        /// <summary>
        /// The exception thrown while updating the property 
        /// </summary>
        public Exception Exception { get => exception; }

    }
}

