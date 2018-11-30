using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class Method : ItemDeclaration
    {
        public TypeReference ReturnType { get; set; }
        public List<MethodParameters> Parameters { get; set; } = new List<MethodParameters>();
        public List<TypeParameters> TypeParameters { get; set; }

    }

    public class MethodParameters : ItemDeclaration
    {
        public TypeReference Type { get; set; }
        public string DefaultValue { get; set; }
        public bool HasDefaultValue { get; set; }

    }

    public class Constructor : Method
    {

    }
}
