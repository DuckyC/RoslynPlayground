using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocumentationModels
{
    public static class DiskProvider
    {

        private static readonly XmlSerializer Serializer;

        static DiskProvider()
        {
            Serializer = new XmlSerializer(typeof(Namespace));
        }

        public static void Save(string path, List<Namespace> namespaces)
        {
            if(Directory.Exists(path))
            {
                foreach (var f in Directory.EnumerateFiles(path).Where(f => Path.GetExtension(f) == "xml"))
                {
                    File.Delete(f);
                }
            } else
            {
                Directory.CreateDirectory(path);
            }
            
            foreach (var ns in namespaces)
            {
                using (var fs = File.Create(Path.Combine(path, ns.FullName + ".xml")))
                {
                    Serializer.Serialize(fs, ns);
                }
            }

        }

        public static List<Namespace> Load(string path)
        {
            var namespaces = new List<Namespace>();
            foreach (var filePath in Directory.EnumerateFiles(path))
            {
                using (var fs = File.OpenRead(filePath))
                {
                    var ns = Serializer.Deserialize(fs) as Namespace;
                    namespaces.Add(ns);
                }
            }

            return namespaces;
        }
    }
}
