using DocumentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentationViewer.Models
{
    public abstract class ItemViewModel {
        public abstract string Summary { get; }
        public abstract string DisplayName { get; }
        public abstract ItemDeclaration ItemInstance { get; }

    }

    public class ItemViewModel<T> : ItemViewModel where T : ItemDeclaration
    {

        public override string Summary => Instance.DocumentationComment?.Summary;
        public override ItemDeclaration ItemInstance => Instance;

        public override string DisplayName => ItemInstance.Name;

        public T Instance { get; private set; }
        public ItemViewModel(T instance)
        {
            Instance = instance;
        }
    }
}