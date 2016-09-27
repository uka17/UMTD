using System.Web.Mvc;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class HomeController : uController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}