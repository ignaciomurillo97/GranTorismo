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
using MongoConnect;

namespace Gran_Torismo_API.Controllers
{
    public class ShopController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // POST: api/Login (nani?)
        [Route("api/Shop/Categories")]
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategories()
        {
            return Ok(db.Categories);
        }

        // Agrega item al carrito del usuario
        [Route("api/Cart/{userId}/{productId}")]
        [HttpPost]
        public IHttpActionResult AddCart(int userId, int productId)
        {
            Redis.AddToCart(userId, productId);
            return Ok(1);
        }

        // Borra item del carrito del usuario
        [Route("api/Cart/{userId}/{productId}")]
        [HttpDelete]
        public IHttpActionResult DeleteItem(int userId, int productId)
        {
            Redis.DeleteFromCart(userId, productId);
            return Ok(1);
        }

        // Retorna todo el carrito
        [Route("api/Cart/{userId}")]
        [ResponseType(typeof(Category))]
        [HttpGet]
        public IHttpActionResult GetCart(int userId)
        {
            var mongoConnection = MongoConnection.Instance;
            List<int> cart = Redis.GetCart(userId);
            List<ServiciosModel> services = new List<ServiciosModel>();
            foreach (int r in cart)
            {
                services.Add(mongoConnection.getServicio(r));
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

        [Route("api/Product/Recomendations/View/{idProducto}")]
        [HttpPost]
        public IHttpActionResult GetRecomendationsByCurrentView(int idProducto)
        {
            var neo = NeoConnection.Instance;
            var mongoConnection = MongoConnection.Instance;
            List<int> res = neo.GetRecomendationsByCurrentView(idProducto);
            List<ServiciosModel> services = new List<ServiciosModel>();
            foreach (int r in res)
            {
                services.Add(mongoConnection.getServicio(r));
            }
            return Ok(services);
        }
    }
}