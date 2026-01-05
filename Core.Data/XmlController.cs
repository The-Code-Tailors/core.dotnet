using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace com.fabioscagliola.Core.Data
{
    public class XmlController<EntityType, EntityIdType> : Controller where EntityType : Entity<EntityIdType>, new()
    {
        protected EntityType entity;

        protected XmlControllerConfiguration configuration;

        public XmlController(XmlControllerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            XmlDocument xmlDocument = GetXmlDocument();

            XmlNode xmlNode = GetXmlNode(entity.Id, xmlDocument);

            if (xmlNode != null)
            {
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EntityType));
            xmlSerializer.Serialize(xmlWriter, entity);
            xmlWriter.Close();

            XPathNavigator xPathNavigator = xmlDocument.DocumentElement.CreateNavigator();
            xPathNavigator.AppendChild(stringBuilder.ToString());

            xmlDocument.Save(configuration.Path);
        }

        //protected int GetNewId<T>(List<T> entityList) where T : Entity
        //{
        //    int id = 0;
        //    foreach (T entity in entityList)
        //        if (entity.Id > id)
        //            id = entity.Id;
        //    return ++id;
        //}

        public EntityType Select(EntityIdType id)
        {
            EntityType thing = default(EntityType);

            XmlDocument xmlDocument = GetXmlDocument();

            XmlNode xmlNode = GetXmlNode(id, xmlDocument);

            if (xmlNode != null)
            {
                XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlNode);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(EntityType));
                thing = (EntityType)xmlSerializer.Deserialize(xmlNodeReader);
                xmlNodeReader.Close();
            }

            return thing;
        }

        protected XmlDocument GetXmlDocument()
        {
            XmlDocument xmlDocument = new XmlDocument();
            if (File.Exists(configuration.Path))
            {
                xmlDocument.Load(configuration.Path);
            }
            else
            {
                xmlDocument.LoadXml("<Root />");
            }
            return xmlDocument;
        }

        protected XmlNode GetXmlNode(EntityIdType id, XmlDocument xmlDocument)
        {
            return xmlDocument.SelectSingleNode(string.Format("/Root/*[id = {0}]", id));
        }

    }
}

