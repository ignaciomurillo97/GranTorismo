using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Gran_Torismo_API.Controllers
{
    public class ShopController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // POST: api/Login
        [Route("api/Shop/Categories")]
        [ResponseType(typeof(Categories))]
        public IHttpActionResult GetCategories()
        {
            return Ok(db.Categories);
        }
    }
}
