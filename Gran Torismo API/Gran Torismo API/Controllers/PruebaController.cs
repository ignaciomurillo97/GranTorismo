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
using MongoConnect;

namespace Gran_Torismo_API.Controllers
{
    public class PruebaController : ApiController
    {
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
                neo.AddFollowing(idFollower, idFollowed);
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
                neo.RemoveFollowing(idFollower, idFollowed);
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
        
        [Route("api/Caca")]
        [HttpGet]
        public IHttpActionResult PruebaMongo()
        {
            MongoConnection caca = new MongoConnection();
            return Ok(caca.getPersonas());
        }

    }
}
