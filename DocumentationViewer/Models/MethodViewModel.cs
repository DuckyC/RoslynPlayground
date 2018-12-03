using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class MethodViewModel : ItemViewModel<Method>
    {
        public class MethodParametersViewModel : ItemViewModel<MethodParameters>
        {
            public MethodParametersViewModel(MethodParameters instance) : base(instance)
            {

            }
        }
        public MethodViewModel(Method instance) : base(instance) { }

        public bool IsConstructor => Instance is Constructor;

        public string ReturnType => IsConstructor ? "" : Instance.ReturnType.Name;

        public string TypeName => IsConstructor ? "Constructor" : "Method";

        public override string DisplayName
        {
            get
            {
                var parameters = string.Join(", ", Instance.Parameters.Select(p => p.Type.Name));
                var typeParameters = string.Join(", ", Instance.TypeParameters.Select(tp => tp.Name));
                return Instance.Name + (typeParameters.Length == 0 ? "" : "<" + typeParameters + ">") + "(" + parameters + ")";
            }
        }

        public string DeclarationLiteral
        {
            get
            {
                var parameters = string.Join(", ", Instance.Parameters.Select(p => p.Type.Name + " " + p.Name));
                var typeParameters = string.Join(", ", Instance.TypeParameters.Select(tp => tp.Name));

                return AttributesLiteral + Instance.Modifiers + " " + ReturnType + " " + Instance.Name + (typeParameters.Length == 0 ? "" : "<" + typeParameters + ">") + "(" + parameters + ")";
            }
        }

        public List<MethodParametersViewModel> Parameters
        {
            get
            {
                return Instance.Parameters.Select(p=>new MethodParametersViewModel(p)).ToList();
            }
        }
    }
}