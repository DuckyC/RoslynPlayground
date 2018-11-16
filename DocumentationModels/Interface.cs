using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class Interface : DeclarationContainer
    {
        public List<TypeParameters> TypeParameters { get; set; }

    }
}
