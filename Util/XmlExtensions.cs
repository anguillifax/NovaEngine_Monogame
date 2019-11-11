using System.Xml;

namespace Nova.Util {

	public static class XmlExtensions {

		public static string GetStringAttribute(this XmlNode node, string attribute) {
			return node.Attributes.GetNamedItem(attribute).Value;
		}

		public static int GetIntAttribute(this XmlNode node, string attribute) {
			return int.Parse(node.GetStringAttribute(attribute));
		}

		public static float GetFloatAttribute(this XmlNode node, string attribute) {
			return float.Parse(node.GetStringAttribute(attribute));
		}

	}

}