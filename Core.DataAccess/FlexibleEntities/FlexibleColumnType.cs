using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public class FlexibleColumnType
    {
        public static readonly string Boolean = typeof(bool).FullName;
        public static readonly string DateTime = typeof(DateTime).FullName;
        public static readonly string Double = typeof(double).FullName;
        public static readonly string String = typeof(string).FullName;

        public string Name { get; set; }

        public static List<FlexibleColumnType> SelectList()
        {
            return new List<FlexibleColumnType>() { 
                new FlexibleColumnType() { Name = Boolean }, 
                new FlexibleColumnType() { Name = DateTime }, 
                new FlexibleColumnType() { Name = Double }, 
                new FlexibleColumnType() { Name = String }, 
            };
        }

    }
}

