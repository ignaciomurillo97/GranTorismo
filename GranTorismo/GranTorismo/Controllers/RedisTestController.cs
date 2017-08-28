using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GranTorismo.Controllers
{
    public class RedisTestController : Controller
    {
        // GET: RedisTest
        public ActionResult Index(string llave, string valor)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            if (cache.KeyExists(llave))
            {
                ViewBag.Message = "Valor:" + cache.StringGet(llave);
            }
            else
            {
                cache.StringSet(llave, valor);
                ViewBag.Message = "Valor Guardado!";
            }
            return View();
        }
    }
}