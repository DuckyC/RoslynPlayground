using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class Enum : ItemDeclaration
    {
        public List<EnumValue> Values { get; set; } = new List<EnumValue>();
    }

    public class EnumValue : ItemDeclaration
    {
        public string Value { get; set; }
    }
}
