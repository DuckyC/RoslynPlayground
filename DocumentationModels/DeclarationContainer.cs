using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocumentationModels
{
    public abstract class DeclarationContainer : ItemDeclaration
    {
        [XmlElement(nameof(Class), type: typeof(Class))]
        [XmlElement(nameof(Interface), type: typeof(Interface))]
        [XmlElement(nameof(Enum), type: typeof(Enum))]
        [XmlElement(nameof(Field), type: typeof(Field))]
        [XmlElement(nameof(Property), type: typeof(Property))]
        [XmlElement(nameof(Method), type: typeof(Method))]
        [XmlElement(nameof(Constructor), type: typeof(Constructor))]
        public List<ItemDeclaration> Declarations { get; set; } = new List<ItemDeclaration>();
    }
}
