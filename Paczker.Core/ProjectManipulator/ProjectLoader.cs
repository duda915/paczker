using System.Xml;

namespace Paczker.Core.ProjectManipulator
{
    public static class ProjectLoader
    {
        public static XmlDocument Load(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);

            return doc;
        }
    }
}