using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class DeclarationListViewModel
    {
        public string Title { get; set; }
        public List<ItemViewModel> Items { get; set; }
    }
}