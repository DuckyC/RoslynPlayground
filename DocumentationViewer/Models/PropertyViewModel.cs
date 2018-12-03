using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationViewer.Models
{
    public class PropertyViewModel : ItemViewModel<Property>
    {
        public PropertyViewModel(Property instance) : base(instance)
        {
        }

        public string DeclarationLiteral
        {
            get
            {
                var getter = Instance.HasGetter ? "get;" : "";
                var setter = Instance.HasSetter ? "set;" : "";
                return $"{Instance.Modifiers} {Instance.Type.Name} {Instance.Name} {{ {getter} {setter} }}";
            }
        }
    }
}
