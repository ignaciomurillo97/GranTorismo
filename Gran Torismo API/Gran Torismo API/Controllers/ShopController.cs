using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RedisConnect;
using NeoConnect;
using Gran_Torismo_API.RedisHelper;
using MongoConnect;

namespace Gran_Torismo_API.Controllers
{
    public class ShopController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // POST: api/Login (nani?)
        [Route("api/Shop/Categories")]
        [ResponseType(typeof(Categories))]
        public IHttpActionResult GetCategories()
        {
            return Ok(db.Categories);
        }

        // Agrega item al carrito del usuario
        [Route("api/Cart/{userId}/{establishmentId}/{productId}")]
        [HttpPost]
        public IHttpActionResult AddCart(int userId, int establishmentId, int productId)
        {
            Redis.AddToCart(userId, establishmentId, productId);
            return Ok(1);
        }

        // Borra item del carrito del usuario
        [Route("api/Cart/{userId}/{establishmentId}/{productId}")]
        [HttpDelete]
        public IHttpActionResult DeleteItem(int userId, int establishmentId, int productId)
        {
            Redis.DeleteFromCart(userId, establishmentId, productId);
            return Ok(1);
        }

        // Retorna todo el carrito
        [Route("api/Cart/{userId}")]
        [ResponseType(typeof(Categories))]
        [HttpGet]
        public IHttpActionResult GetCart(int userId)
        {
            var mongoConnection = MongoConnection.Instance;
            List<RedisItem> cart = Redis.GetCart(userId);
            List<ServiciosModel> services = new List<ServiciosModel>();
            foreach (RedisItem r in cart)
            {
                services.Add(mongoConnection.getServicio(r.serviceId, r.establishmentId));
            }
            return Ok(services);
        }

        // Agrega producto a las BD
        [Route("api/Product/{productId}")]
        [HttpPost]
        public IHttpActionResult AddItem(int productId)
        {
            // TODO Agregar producto en Mongo y SQL
            var neo = NeoConnection.Instance;
            neo.AddProduct(productId);
            return Ok(1);
        }

        // Elimina producto de las BD
        [Route("api/Product/{productId}")]
        [HttpDelete]
        public IHttpActionResult DeleteItem(int productId)
        {
            // TODO Eliminar producto de Mongo y SQL
            var neo = NeoConnection.Instance;
            neo.RemoveProduct(productId);
            return Ok(1);
        }

        // Registra que un usuario vio un producto
        [Route("api/Product/Views/{userId}/{productId}")]
        [HttpPost]
        public IHttpActionResult ViewProduct(int userId, int productId)
        {
            var neo = NeoConnection.Instance;
            neo.AddView(userId, productId);
            return Ok(1);
        }

        // Registra que un usuario vio un producto
        [Route("api/Product/Purchase/{userId}/{productId}")]
        [HttpPost]
        public IHttpActionResult PurchaseProduct(int userId, int productId)
        {
            // TODO agregar logica de compra
            var neo = NeoConnection.Instance;
            neo.AddPurchase(userId, productId);
            return Ok(1);
        }

        // Devuelve todos los productos
        [Route("api/Product/")]
        [HttpGet]
        public IHttpActionResult GetProducts(int userId, int productId)
        {
            //TODO devolver los productos de mongo
            return Ok(1);
        }
    }
}