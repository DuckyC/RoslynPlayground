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

        public string DeclarationLiteral
        {
            get
            {
                var attributes = string.Join("\n", Instance.Attributes.Select(a => "[" + a.Literal + "]\n")) + "\n";

                return attributes + Instance.Modifiers + " class " + Instance.Name;
            }
        }

        public List<ItemViewModel<Constructor>> Constructors
        {
            get
            {
                var constructorList = Instance.Declarations.OfType<Constructor>().Select(c => new ItemViewModel<Constructor>(c)).ToList();
                if (constructorList.Count > 0)
                {
                    return constructorList;
                }

                if (Instance.Modifiers.ToLowerInvariant().Contains("static")) { return new List<ItemViewModel<Constructor>>(); }

                return new List<ItemViewModel<Constructor>>
                {
                    new ItemViewModel<Constructor>(new Constructor { ParentFullName = Instance.FullName, Name = Instance.Name }),
                };
            }
        }
        public List<ItemViewModel<Field>> Fields => Instance.Declarations.OfType<Field>().Select(f => new ItemViewModel<Field>(f)).ToList();
        public List<ItemViewModel<Property>> Properties => Instance.Declarations.OfType<Property>().Select(p => new ItemViewModel<Property>(p)).ToList();
        public List<MethodViewModel> Methods => Instance.Declarations.OfType<Method>().Select(m => new MethodViewModel(m)).ToList();
    }
}