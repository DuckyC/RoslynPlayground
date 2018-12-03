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
        public class EnumValueViewModel : ItemViewModel<EnumValue>
        {
            public EnumValueViewModel(EnumValue instance) : base(instance)
            {
            }

        }

        public EnumViewModel(Enum instance) : base(instance)
        {
        }

        public string DeclarationLiteral
        {
            get
            {
                return AttributesLiteral + Instance.Modifiers + " enum " + DisplayName;
            }
        }

        public List<EnumValueViewModel> Fields => Instance.Values.Select(v => new EnumValueViewModel(v)).ToList();
    }
}