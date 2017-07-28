using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace IGT.Webjet.GALConnection
{
    static class XmlHelper
    {

        public static XElement RemoveAllNamespaces(XElement _xmlDocument)
        {
            if (!_xmlDocument.HasElements)
            {
                XElement xElement = new XElement(_xmlDocument.Name.LocalName);
                xElement.Value = _xmlDocument.Value;

                foreach (XAttribute attribute in _xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(_xmlDocument.Name.LocalName, _xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public static XElement GetChildElement(XElement _xmlelement, string _path)
        {
            XElement childElement = _xmlelement;
            List<string> subPathList = _path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();

            foreach (var subPath in subPathList)
            {
                childElement = childElement.Element(subPath);

                if (childElement == null)
                    return childElement;
            }

            if (childElement.Elements() != null && childElement.Elements().Count() > 0)
            {
                childElement = childElement.Elements().FirstOrDefault();
            }

            return childElement;
        }

        public static IEnumerable<XElement> GetChildElementList(XElement _xmlelement, string _path)
        {
            IEnumerable<XElement> childElement = null;// _xmlelement.Elements();
            List<string> subPathList = _path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();
            int i = 0;
            foreach (var subPath in subPathList)
            {
                if (i == 0)
                    childElement = _xmlelement.Element(subPath).Elements();
                else
                    childElement = childElement.FirstOrDefault().Element(subPath).Elements();

                i++;
                if (childElement == null)
                    return childElement;
            }

            return childElement;
        }

        public static void SetChildElementValue(XElement _xmlelement, string _path, object _value)
        {
            XElement childElement = _xmlelement;
            List<string> subPathList = _path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();

            foreach (var subPath in subPathList)
            {
                childElement = childElement.Element(subPath);

                if (childElement == null)
                    return;
            }

            if (childElement.Elements() != null && childElement.Elements().Count() > 0)
            {
                childElement = childElement.Elements().FirstOrDefault();
            }

            childElement.SetValue(_value);
        }
    }
}
