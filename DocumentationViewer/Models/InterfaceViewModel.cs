using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class InterfaceViewModel : ItemViewModel<Interface>
    {
        public InterfaceViewModel(Interface instance) : base(instance)
        {
        }
    }
}