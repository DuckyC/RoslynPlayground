using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public class ClassViewModel : ItemViewModel<Class>
    {
        public ClassViewModel(Class classInstance) : base(classInstance) { }

        public List<ItemViewModel<Constructor>> Constructors => Instance.Declarations.OfType<Constructor>().Select(c=>new ItemViewModel<Constructor>(c)).ToList();
        public List<ItemViewModel<Field>> Fields => Instance.Declarations.OfType<Field>().Select(f=>new ItemViewModel<Field>(f)).ToList();
        public List<ItemViewModel<Property>> Properties => Instance.Declarations.OfType<Property>().Select(p=>new ItemViewModel<Property>(p)).ToList();
        public List<MethodViewModel> Methods => Instance.Declarations.OfType<Method>().Select(m => new MethodViewModel(m)).ToList();
    }
}