using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public class DocumentationComment
    {
        public string Summary { get; set; }
        public string Returns { get; set; }

        public List<ParameterComment> Parameters { get; set; }
    }

    public class ParameterComment
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
