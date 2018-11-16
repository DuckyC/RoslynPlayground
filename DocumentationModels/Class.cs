using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class Class : DeclarationContainer
    {
        public List<TypeReference> Inheritance { get; set; }
        public List<TypeReference> Implements { get; set; }
        public List<TypeParameters> TypeParameters { get; set; }


    }
}
