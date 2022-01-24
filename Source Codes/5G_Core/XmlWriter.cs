using System;
using System.Xml;
using System.IO;


namespace _5G_Core
{
    class XmlWriter
    {
        private static XmlDocument outputDoc;

        public void add_attribute(XmlDocument Doc, XmlNode node, String name, String value)
        {
            XmlAttribute attri = Doc.CreateAttribute(name);
            attri.Value = value;

            node.Attributes.Append(attri);      
        }

        public void remove_attribute(XmlNode node, String name)
        {
            XmlAttribute attribute = null;
            foreach (XmlAttribute attri in node.Attributes)
            {
                if (attri.Name == name)
                    attribute = attri;
            }
            if (attribute != null)
                node.Attributes.Remove(attribute);
        }
      
        public XmlNode add_element_from_file( XmlDocument Doc, XmlNode parent, String filename)
        {
            String content = File.ReadAllText(filename);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            XmlNode newNode = doc.DocumentElement;
            XmlNode importNode = Doc.ImportNode(newNode, true);
            parent.AppendChild(importNode);
            //Doc.Save(@"C:/Users/mridula/Desktop/Configuration_final.xml");
            return importNode;

        }

        public XmlNode add_element_from_file_with_format( XmlDocument Doc, XmlNode parent, String filename, params String[] args)
        {
            String format = File.ReadAllText(filename);
            String content = string.Format(format, args);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            XmlNode newNode = xmlDoc.DocumentElement;

            XmlNode importNode = Doc.ImportNode(newNode, true);
            
            parent.AppendChild(importNode);
            //Doc.Save(@"C:/Users/mridula/Desktop/Configuration_final.xml");
            return importNode;

        }

        public XmlNode add_device_element_from_file_with_format(XmlDocument Doc, XmlNode parent, String filename, params String[] args)
        {
            String format = File.ReadAllText(filename);
            String content = string.Format(format, args);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            XmlNode newNode = xmlDoc.DocumentElement;

            XmlNode importNode = Doc.ImportNode(newNode, true);

            parent.PrependChild(importNode);
          //  Doc.Save(@"C:/Users/mridula/Desktop/Configuration_final.xml");
            return importNode;

        }

        public XmlNode add_interface_element_from_file_with_format( XmlDocument Doc, XmlNode parent, XmlElement child, String filename, params String[] args)
        {
            String format = File.ReadAllText(filename);
            String content = string.Format(format, args);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            XmlNode newNode = xmlDoc.DocumentElement;

            XmlNode importNode = Doc.ImportNode(newNode, true);

            parent.InsertAfter(importNode, child);
           // Doc.Save(@"C:/Users/mridula/Desktop/Configuration_final.xml");
            return importNode;

        }

        public XmlNode add_element(XmlDocument Doc, XmlNode parent, String name)
        {
            XmlNode node = Doc.CreateElement(name);
            parent.AppendChild(node);
            return node;
        }
        public void save_document(XmlDocument Doc, string filename)
        {
            Doc.Save(filename);
        }

    }
}
