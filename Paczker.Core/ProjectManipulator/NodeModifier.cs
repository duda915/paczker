using System;
using System.Xml;
using LanguageExt;

namespace Paczker.Core.ProjectManipulator
{
    public static class NodeModifier
    {
        public static Unit Set(string csprojPath, VersionNode node, string value)
        {
            var doc = ProjectLoader.Load(csprojPath);
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