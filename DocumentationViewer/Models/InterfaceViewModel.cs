using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class InterfaceViewModel : ItemViewModel<Interface>
    {
        public InterfaceViewModel(Interface instance) : base(instance) { }

        public string DeclarationLiteral
        {
            get
            {
                return AttributesLiteral + Instance.Modifiers + " interface " + Instance.Name;
            }
        }

        public List<ItemViewModel<Field>> Fields => Instance.Declarations.OfType<Field>().Select(f => new ItemViewModel<Field>(f)).ToList();
        public List<ItemViewModel<Property>> Properties => Instance.Declarations.OfType<Property>().Select(p => new ItemViewModel<Property>(p)).ToList();
        public List<MethodViewModel> Methods => Instance.Declarations.Where(d=>d.GetType() == typeof(Method)).OfType<Method>().Select(m => new MethodViewModel(m)).ToList();
    }
}