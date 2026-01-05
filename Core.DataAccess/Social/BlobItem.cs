using System;
using System.Xml.Serialization;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public class BlobItem
    {
        public Guid BlobGuid { get; set; }

        protected Blob blob;

        [XmlIgnore]
        public Blob Blob
        {
            get
            {
                if (BlobGuid != Guid.Empty && blob == null)
                {
                    blob = Blob.Select(Milieu.SystemMilieu, BlobGuid);
                }
                return blob;
            }
        }

    }
}

