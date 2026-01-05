using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Blob : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.BlobDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.BlobUpdate; } }

        /// <summary>
        /// Does not support transactions - different database! 
        /// </summary>
        public override void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            base.Delete(milieu, null, permanently);
        }

        /// <summary>
        /// Does not support transactions - different database! 
        /// </summary>
        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            base.Update(milieu, null);
        }

        protected byte[] content;

        [XmlIgnore()]
        public byte[] Content
        {
            get
            {
                if (content == null)
                {
                    BlobSqlController controller = (BlobSqlController)GetDefaultController();
                    content = controller.SelectContent();
                }
                //if (IsCompressed)
                //{
                //    content = Util.Decompress(content);
                //}
                return content;
            }
            set
            {
                content = value;
                ContentLength = content.LongLength;
                //if (IsCompressed)
                //{
                //    content = Util.Compress(content);
                //}
            }
        }

        /// <summary>
        /// ContentLength is automatically set when Content is set; it should be read-only... 
        /// </summary>
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        /// <summary>
        /// To enable compression, set IsCompressed to true *before setting Content* 
        /// </summary>
        public bool IsCompressed { get; set; }
        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }


        protected override Controller GetController(ControllerConfiguration configuration)
        {
            BlobSqlController controller = new BlobSqlController(configuration, this);
            return controller;
        }

        public static Blob Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.BlobSelect);
            Blob entity = new Blob();
            BlobSqlController controller = (BlobSqlController)entity.GetDefaultController();
            return controller.Select(id);
        }

        public static Blob Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.BlobSelect);
            Blob entity = new Blob();
            BlobSqlController controller = (BlobSqlController)entity.GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Blob> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.BlobSelect);
            Blob entity = new Blob();
            BlobSqlController controller = (BlobSqlController)entity.GetDefaultController();
            return controller.SelectList();
        }

        public static List<Blob> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.BlobSelect);
            Blob entity = new Blob();
            BlobSqlController controller = (BlobSqlController)entity.GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Blob> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.BlobSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            BlobSqlController controller = (BlobSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Blob Copy(Milieu milieu, DataAccessEntity entity)
        {
            Blob blob = Blob.Select(milieu, Id);

            blob.Id = 0;
            blob.Content = Content;

            blob.MasterEntity = entity.GetType().FullName;
            blob.MasterGuid = entity.Guid;
            blob.MasterId = entity.Id;

            blob.Update(milieu);

            return blob;
        }

    }
}

