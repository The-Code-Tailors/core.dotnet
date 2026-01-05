using System;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Indicate if property is the item list of data collection item
    /// </summary>    
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DataCollectionItemListAttribute : Attribute
    {
        /// <summary>
        /// Controls the comunication data type of the property 
        /// </summary>
        /// <param name="dataType">The type of the data used for comunication with the device</param>
        public DataCollectionItemListAttribute()
        { }

        public bool DataCollectionItemList
        {
            get
            {
                return true;
            }
        }

    }
}

