using DocumentationModels;
using DocumentationViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentationViewer.Controllers
{
    public class DocumentationController : Controller
    {
        public static List<Namespace> Namespaces { get; set; } = new List<Namespace>();

        static DocumentationController()
        {
            Namespaces = DiskProvider.Load("C:\\Users\\mj\\Documents\\docout\\");
        }


        [ValidateInput(false)]
        public ActionResult Index(string FullName)
        {
            if(string.IsNullOrWhiteSpace(FullName))
            {
                return View("NamespacesView", Namespaces); 
            }

            var item = Namespaces.FindByFullName(FullName);
            if (item == null) { return HttpNotFound(); }

            if (item is Namespace namespaceInstance)
            {
                return View("NamespaceView", new NamespaceViewModel(namespaceInstance));
            }
            else if (item is Class classInstance)
            {
                return View("ClassView", new ClassViewModel(classInstance));
            }
            else if(item is Interface interfaceInstance)
            {
                return View("InterfaceView", new InterfaceViewModel(interfaceInstance));
            }
            else if(item is Method methodInstance)
            {
                return View("MethodView", new MethodViewModel(methodInstance));
            }
            else if(item is DocumentationModels.Enum enumInstance)
            {
                return View("EnumView", new EnumViewModel(enumInstance));
            }
            else if(item is Property propertyInstance)
            {
                return View("PropertyView", new PropertyViewModel(propertyInstance));
            }
            else if(item is Field fieldInstance)
            {
                return View("FieldView", new FieldViewModel(fieldInstance));
            }
            throw new HttpException($"Type unsupported - ${FullName}(${item.GetType()})");
        }

        [ValidateInput(false)]
        public ActionResult Search(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return new EmptyResult();
            }

            var foundList = new List<ItemDeclaration>();

            foreach (var ns in DocumentationController.Namespaces)
            {
                foundList.AddRange(ns.SearchTree(i => i.FullName.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) >= 0));
                foundList.AddRange(ns.SearchTree(i => i.FilePath?.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) >= 0));
                foundList.AddRange(ns.SearchTree(i => i.Attributes?.Any(a => a.Literal.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) >= 0) == true));
                foundList.AddRange(ns.SearchTree(i => i.DocumentationComment?.Summary?.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) >= 0));
            }

            var list = foundList.Select(i => new SearchItemViewModel { DisplayName = i.Name, FullName = i.FullName }).ToList();
            return PartialView("~/Views/Shared/_SearchResults.cshtml", list);
        }
    }
}