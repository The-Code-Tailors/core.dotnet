using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// https://github.com/mledoze/countries 
    /// </summary>
    public class IsoCountry : DataAccessEntity
    {
        public class IsoCountryName
        {
            [JsonProperty("common")]
            public string Common { get; set; }

            [JsonProperty("official")]
            public string Official { get; set; }

        }

        public class IsoCountryNameTranslations
        {
            [JsonProperty("ita")]
            public IsoCountryName Italian { get; set; }

        }

        protected override DataAccessFunction DeleteDataAccessFunction => throw new NotImplementedException();

        protected override DataAccessFunction UpdateDataAccessFunction => throw new NotImplementedException();

        [JsonProperty("name")]
        public IsoCountryName Name { get; set; }

        [JsonProperty("translations")]
        public IsoCountryNameTranslations Translations { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2 
        /// </summary>
        [JsonProperty("cca2")]
        public string Alpha2 { get; protected set; }

        /// <summary>
        /// ISO 3166-1 alpha-3 
        /// </summary>
        [JsonProperty("cca3")]
        public string Alpha3 { get; protected set; }

        /// <summary>
        /// ISO 3166-1 numeric 
        /// </summary>
        [JsonProperty("ccn3")]
        public string Numeric { get; protected set; }

        protected static List<IsoCountry> isoCountryList;

        static IsoCountry()
        {
            if (isoCountryList == null)
            {
                isoCountryList = JsonConvert.DeserializeObject<List<IsoCountry>>(Properties.Resources.Countries);
                isoCountryList.Sort((a, b) => a.Name.Common.CompareTo(b.Name.Common));
                foreach (IsoCountry isoCountry in isoCountryList)
                    isoCountry.Id = long.Parse(isoCountry.Numeric);
            }
        }

        public static IsoCountry Select(Milieu milieu, long id)
        {
            return isoCountryList.Find(x => x.Id == id);
        }

        public static IsoCountry Select(Milieu milieu, string alpha2)
        {
            return isoCountryList.Find(x => x.Alpha2 == alpha2);
        }

        public static List<IsoCountry> SelectList(Milieu milieu)
        {
            return isoCountryList;
        }

        public string CommonNameInCurrentUICulture
        {
            get
            {
                string result = Name.Common;

                if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "it")
                {
                    result = Translations.Italian.Common;
                }

                return result;
            }
        }

        public override string ToString()
        {
            return CommonNameInCurrentUICulture;
        }

    }
}

