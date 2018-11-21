using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enum = DocumentationModels.Enum;

namespace DocumentationViewer.Models
{
    public class NamespaceViewModel : ItemViewModel<Namespace>
    {
        public NamespaceViewModel(Namespace instance) : base(instance)
        {
        }

        public List<ClassViewModel> Classes => Instance.Declarations.OfType<Class>().Select(c => new ClassViewModel(c)).ToList();
        public List<EnumViewModel> Enums => Instance.Declarations.OfType<Enum>().Select(e => new EnumViewModel(e)).ToList();
        public List<InterfaceViewModel> Interfaces => Instance.Declarations.OfType<Interface>().Select(i => new InterfaceViewModel(i)).ToList();

    }
}