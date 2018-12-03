using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationViewer.Models
{
    public class FieldViewModel : ItemViewModel<Field>
    {
        public FieldViewModel(Field instance) : base(instance)
        {
        }

        public string DeclarationLiteral
        {
            get
            {
                return $"{Instance.Modifiers} {Instance.Type.Name} {Instance.Name}";
            }
        }
    }
}
