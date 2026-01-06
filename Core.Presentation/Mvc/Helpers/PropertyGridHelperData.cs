namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// Holds the configuration of a property-grid editor or viewer 
    /// </summary>
    public class PropertyGridHelperData
    {
        /// <summary>
        /// The object edited using a property-grid editor or viewed using a property-grid viewer 
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// The number of columns in which the properties are to be arranged in the property-grid editor 
        /// </summary>
        public int Cols { get; set; }

        public PropertyGridHelperData()
        {
            Cols = 1;
        }

    }
}

