using System;
using System.Xml;
using LanguageExt;

namespace Paczker.Core.ProjectManipulator
{
    public static class NodeModifier
    {
        public static Unit Set(string path, VersionNode node, string value)
        {
            var doc = ProjectLoader.Load(path);
            return Set(doc, node, value);
        }
        public static Unit Set(XmlDocument doc, VersionNode node, string value)
        {
            var versionNode = NodeFinder.GetVersionNode(doc, node);
            versionNode.IfSome(x =>
            {
                x.InnerText = value;
                doc.Save(new Uri(doc.BaseURI).LocalPath);
            });
            
            return Unit.Default;
        }
    }
}