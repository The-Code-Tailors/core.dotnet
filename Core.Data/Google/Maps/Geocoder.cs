using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Xml;

namespace com.fabioscagliola.Core.Data.Google.Maps
{
    public class Geocoder
    {
        public static List<GeocoderResult> Do(string address)
        {
            try
            {
                Trace.WriteLine(string.Format("Geocoding \"{0}\"...", address));

                const int ATTEMPTS = 3;

                List<GeocoderResult> resultList = new List<GeocoderResult>();

                XmlDocument xmlDocument = new XmlDocument();

                string status = null;

                int n = 0;

                while (n < ATTEMPTS)
                {
                    xmlDocument.Load(string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", HttpUtility.UrlEncode(address)));

                    status = xmlDocument.SelectSingleNode("/GeocodeResponse/status").FirstChild.Value;

                    Trace.WriteLine(status);

                    if (status != "OVER_QUERY_LIMIT")
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }

                    n++;
                }

                if (status == "OK")
                {
                    foreach (XmlNode xmlNode in xmlDocument.SelectNodes("/GeocodeResponse/result"))
                    {
                        GeocoderResult result = new GeocoderResult();
                        result.FormattedAddress = xmlNode.SelectSingleNode("formatted_address").FirstChild.Value;
                        result.Lat = double.Parse(xmlNode.SelectSingleNode("geometry/location/lat").FirstChild.Value, CultureInfo.InvariantCulture);
                        result.Lng = double.Parse(xmlNode.SelectSingleNode("geometry/location/lng").FirstChild.Value, CultureInfo.InvariantCulture);
                        resultList.Add(result);
                    }
                }
                else if (status == "ZERO_RESULTS")
                {
                    // Do nothing, return the empty result list  
                }
                else
                {
                    string message = string.Format("The Google Geocoding API returned an error: the response status is \"{0}\"", status);
                    XmlNode xmlNode = xmlDocument.SelectSingleNode("/GeocodeResponse/error_message");
                    if (xmlNode != null)
                    {
                        message += string.Format(" and the error message is \"{0}\"", xmlNode.FirstChild.Value);
                    }
                    throw new ApplicationException(message);
                }

                return resultList;

            }
            catch (Exception e)
            {
                throw new ApplicationException($"[Geocoder] {e.Message}", e);
            }
        }

    }
}

