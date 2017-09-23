using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RedisConnect;

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

        // Agrega producto al carrito
        [Route("api/Cart/{userId}/{productId}")]
        [HttpPost]
        public IHttpActionResult AddCart(int userId, int productId)
        {
            Redis.AddToCart(userId, productId);
            return Ok(1);
        }

        [Route("api/Cart/{userId}/{productId}")]
        [HttpDelete]
        public IHttpActionResult DeleteItem(int userId, int productId)
        {
            Redis.DeleteFromCart(userId, productId);
            return Ok(1);
        }

        [Route("api/Cart/{userId}")]
        [ResponseType(typeof(Categories))]
        [HttpGet]
        public IHttpActionResult GetCart(int userId)
        {
            List<int> cart = Redis.GetCart(userId);
            return Ok(cart);
        }
    }
}
