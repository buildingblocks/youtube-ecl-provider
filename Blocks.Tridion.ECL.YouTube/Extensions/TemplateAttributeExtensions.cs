using System.Collections.Generic;
using System.Linq;
using Google.GData.Client;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    public static class TemplateAttributeExtensions
    {
        public static string Attribute(this IList<ITemplateAttribute> attributes, string localName)
        {
            if (attributes != null)
            {
                localName = localName.ToLowerInvariant();

                return attributes.Select(attr => new {attr, attrName = attr.Name.ToLowerInvariant()})
                                 .Where(@t => @t.attrName == localName || @t.attrName.EndsWith(":" + localName))
                                 .Select(@t => @t.attr.Value).FirstOrDefault();
            }

            return string.Empty;
        }

        public static string SupportedAttributes(this IList<ITemplateAttribute> attributes,
                                                 IEnumerable<string> supportedNames)
        {
            if (attributes != null)
            {
                var attrs = from name in supportedNames
                            let value = attributes.Attribute(name)
                            where !string.IsNullOrEmpty(value)
                            select string.Format("{0}=\"{1}\"", name, HttpUtility.HtmlEncode(value));

                return string.Join(" ", attrs);
            }

            return string.Empty;
        }
    }
}