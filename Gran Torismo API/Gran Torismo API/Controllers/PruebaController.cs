using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using Gran_Torismo_API.Models;
using Gran_Torismo_API.NeoHelper;
using NeoConnect;
using RedisConnect;

namespace Gran_Torismo_API.Controllers
{
    public class PruebaController : ApiController
    {

        [Route("api/Usuario/Crear")]
        public IHttpActionResult CrearUsuario(int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            neo.AddUser(idUsuario);
            return Ok(idUsuario);
        }

        [Route("api/Producto/Crear")]
        public IHttpActionResult CrearProducto(int idProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            neo.AddProduct(idProducto);
            return Ok(idProducto);
        }

        [Route("api/Producto/Registrar/Vista")]
        public IHttpActionResult RegistrarVista(int idUsuario, int idProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            neo.AddView(idUsuario, idProducto);
            return Ok("orkas");
        }

        [Route("api/Producto/Registrar/Compra")]
        public IHttpActionResult RegistrarCompra(int idUsuario, int idProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            neo.AddPurchase(idUsuario, idProducto);
            return Ok("orkas");
        }

        [Route("api/Usuario")]
        public IHttpActionResult getAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoUser> res = neo.GetAllUsers();
            return Ok(res);
        }

        [Route("api/Producto")]
        public IHttpActionResult getAllProducts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.GetAllProducts();
            return Ok(res);
        }

        [Route("api/Usuario/Follows")]
        [HttpPost]
        public IHttpActionResult getFollows(int idUsuario)
        {
            var neo = NeoConnection.Instance;
            List<NeoUser> follows = neo.GetUserFollows(idUsuario);
            return Ok(follows);
        }

        [Route("api/Usuario/AddFollower")]
        [HttpPost]
        public IHttpActionResult addFollow(int idFollower, int idFollowed)
        {
            if (idFollower != idFollowed)
            {
                var neo = NeoConnection.Instance;
                neo.addFollowing(idFollower, idFollowed);
                return Ok(1);
            }
            return Ok(0);
        }

        [Route("api/Usuario/DeleteFollower")]
        [HttpPost]
        public IHttpActionResult DeleteFollow(int idFollower, int idFollowed)
        {
            if (idFollower != idFollowed)
            {
                var neo = NeoConnection.Instance;
                neo.removeFollowing(idFollower, idFollowed);
                return Ok(1);
            }
            return Ok(0);
        }

        [Route ("api/Producto/RecomendationsByViews")]
        [HttpPost]
        public IHttpActionResult GetRecomendationsByView (int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.getRecomendationsByViews(idUsuario);
            return Ok(res);
        }

        [Route("api/Producto/RecomendationsByCurrentView")]
        [HttpPost]
        public IHttpActionResult GetRecomendationsByCurrentView(int idProducto, int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.GetRecomendationsByCurrentView(idProducto, idUsuario);
            return Ok(res);
        }
        [Route("api/Cart/Add")]
        [HttpPost]
        public IHttpActionResult AddCart(int userId, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Redis.AddToCart(userId, productId);
            return Ok(1);
        }

        [Route("api/Cart/DeleteItem")]
        [HttpPost]
        public IHttpActionResult DeleteItem(int userId, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Redis.DeleteFromCart(userId, productId);
            return Ok(1);
        }

        [Route("api/Cart/")]
        [HttpPost]
        public IHttpActionResult GetCart(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<int> cart = Redis.GetCart(userId);
            return Ok(cart);
        }

    }
}
