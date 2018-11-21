using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class MethodViewModel : ItemViewModel<Method>
    {
        public MethodViewModel(Method instance) : base(instance) {}

        public override string DisplayName
        {
            get
            {
                var parameters = string.Join(", ", Instance.Parameters.Select(p => p.Type.Name).ToArray());
                return Instance.Name + "(" + parameters + ")";
            }
        }
    }
}