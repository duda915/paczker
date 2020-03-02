using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using static LanguageExt.Prelude;
namespace Paczker.Core.ProjectManipulator
{
    public static class NodeFinder
    {
        public static Option<XmlNode> GetVersionNode(XmlDocument doc, VersionNode versionNode)
        {
            return doc.DocumentElement.SelectSingleNode(GetNodeStringMap()[versionNode]);
        }
        
        public static Option<string> GetVersionNodeValue(XmlDocument doc, VersionNode versionNode) =>
            GetVersionNode(doc, versionNode).Select(x => x.InnerText);

        public static Option<XmlNodeList> GetProjectReferenceNodes(XmlDocument doc)
        {
            return doc.DocumentElement.SelectNodes("/Project/ItemGroup/ProjectReference");
        }

        public static IEnumerable<string> GetProjectReferenceNodeValues(XmlDocument doc)
        {
            var xmlNodes = GetProjectReferenceNodes(doc);
            
            return xmlNodes.Match(x =>
            {
                return Range(0, x.Count)
                    .Select(y => Optional(x[y].Attributes["Include"]?.InnerText))
                    .Select(y => y.Replace("\\", "/"))
                    .Where(y => y.IsSome)
                    .Select(y => y.ValueUnsafe());
            }, new string[0]);
        }
            
        private static Map<VersionNode, string> GetNodeStringMap()
        {
            return Map(
                (VersionNode.Version, "/Project/PropertyGroup/Version"),
                (VersionNode.AssemblyVersion, "/Project/PropertyGroup/AssemblyVersion")
            );
        }
    }
}