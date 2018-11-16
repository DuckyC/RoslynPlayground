using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class Property : ItemDeclaration
    {
        public bool HasGetter { get; set; }

        public bool HasSetter { get; set; }

        public TypeReference Type { get; set; }
    }
}
