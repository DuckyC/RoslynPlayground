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



        public ActionResult Index(string FullName)
        {
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

            throw new HttpException($"Type unsupported - ${FullName}(${item.GetType()})");
        }
    }
}