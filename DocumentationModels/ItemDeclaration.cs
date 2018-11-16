using System.Collections.Generic;

namespace DocumentationModels
{
    public abstract class ItemDeclaration
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public DocumentationComment DocumentationComment { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string Modifiers { get; set; }
    }

    public class TypeReference
    {
        public string FullName { get; set; }
        public string Name { get; set; }

        public List<TypeReference> TypeParameters { get; set; } = new List<TypeReference>();
    }

    public class TypeParameters
    {
        public string Name { get; set; }
    }
}
