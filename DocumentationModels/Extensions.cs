using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationModels
{
    public static class Extensions
    {
        public static ItemDeclaration FindByFullName(this List<Namespace> Namespaces, string fullName)
        {
            foreach (var ns in Namespaces)
            {
                var rtn = ns.FindByFullName(fullName);
                if (rtn != null) return rtn;
            }
            return null;
        }

        public static ItemDeclaration FindByFullName(this ItemDeclaration item, string fullName)
        {
            if (item.FullName.Equals(fullName, StringComparison.InvariantCultureIgnoreCase))
            {
                return item;
            }

            if (item is DeclarationContainer containing)
            {
                if (fullName.StartsWith(containing.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var childItem in containing.Declarations)
                    {
                        var rtn = FindByFullName(childItem, fullName);
                        if (rtn != null) return rtn;
                    }

                }
            }

            return null;
        }

        public static List<ItemDeclaration> SearchTree(this ItemDeclaration item, Func<ItemDeclaration, bool> func)
        {
            var foundList = new List<ItemDeclaration>();
            if (func(item))
            {
                foundList.Add(item);
            }

            if(item is DeclarationContainer container)
            {
                foreach (var childItem in container.Declarations)
                {
                    var moreFound = SearchTree(childItem, func);
                    if(moreFound != null)
                    {
                        foundList.AddRange(moreFound);
                    }
                }
            }

            return foundList;
        }


    }
}
