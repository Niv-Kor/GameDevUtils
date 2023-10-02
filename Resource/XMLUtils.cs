using System.Xml.Linq;
using UnityEngine;

namespace GameDevUtils.Resource
{
    public static class XMLUtils
    {
        /// <param name="docPath">An XML file's path (inside the 'Resources' folder)</param>
        /// <returns>The XML document.</returns>
        public static XDocument ReadXML(string docPath) {
            TextAsset xmlAsset = Resources.Load<TextAsset>(docPath);
            return XDocument.Parse(xmlAsset.text);
        }

        /// <param name="attrName">The name of the attribute to get</param>
        /// <returns>The attribute's string value or an empty string if the attribute doesn't exist.</returns>
        public static bool TryGetStringAttr(this XElement element, string attrName, out string res) {
            XAttribute attr = element.Attribute(attrName);
            res = attr?.Value;
            return res != null;
        }

        /// <param name="attrName">The name of the attribute to get</param>
        /// <returns>The attribute's float value.</returns>
        public static bool TryGetNumberAttr(this XElement element, string attrName, out float res) {
            XAttribute attr = element.Attribute(attrName);
            string strAttr = attr?.Value;
            return float.TryParse(strAttr, out res);
        }
    }
}
