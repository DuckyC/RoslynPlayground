using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentationModels;
using Enum = DocumentationModels.Enum;

namespace DocumentationViewer.Models
{
    public class EnumViewModel : ItemViewModel<Enum>
    {
        public EnumViewModel(Enum instance) : base(instance)
        {
        }

    }
}