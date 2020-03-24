using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using static LanguageExt.Prelude;
namespace Paczker.Core.ProjectManipulator
{
    public static class NodeFinder
    {
        public static Option<XmlNode> GetVersionNode(XmlDocument document, VersionNode versionNode)
        {
            return document.DocumentElement.SelectSingleNode(GetNodeStringMap()[versionNode]);
        }
        
        public static Option<XmlNode> GetReadOnlyVersionNode(string csprojPath, VersionNode versionNode)
        {
            var doc = ProjectLoader.Load(csprojPath);
            return GetVersionNode(doc, versionNode);
        }
        
        public static Option<string> GetVersionNodeValue(string csprojPath, VersionNode versionNode) =>
            GetReadOnlyVersionNode(csprojPath, versionNode).Select(x => x.InnerText);

        private static Option<XmlNodeList> GetProjectReferenceNodes(string csprojPath)
        {
            var doc = ProjectLoader.Load(csprojPath);
            var nodes = doc.DocumentElement.SelectNodes("/Project/ItemGroup/ProjectReference");
            return nodes.Count > 0 ? Optional(nodes) : None;
        }

        public static IEnumerable<string> GetProjectReferencePaths(string csprojPath)
        {
            var xmlNodes = GetProjectReferenceNodes(csprojPath);
            
            return xmlNodes.Bind(x => 
                    Range(0, x.Count)
                    .Select(y => Optional(x[y].Attributes["Include"]?.InnerText))
                    .MapT(y => y.Replace('\\', '/'))
                    .MapT(y => Path.Combine(Path.GetDirectoryName(csprojPath), y))
                    .MapT(Path.GetFullPath)
                    .Choose(y => y)
            );
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