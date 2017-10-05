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
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;

namespace Gran_Torismo_API.Controllers
{
    public class PruebaController : ApiController
    {
        [Route("api/Producto/Registrar/Compra/{idUsuario}/{idProducto}")]
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

        [Route("prueba/Usuario/{id}")]
        [HttpPost]
        public IHttpActionResult addUser(int id)
        {
            var neo = NeoConnection.Instance;
            neo.AddUser(id);
            return Ok("orkas");
        }

        [Route("prueba/Product/{id}")]
        [HttpPost]
        public IHttpActionResult addProduct(int id)
        {
            var neo = NeoConnection.Instance;
            neo.AddProduct(id);
            return Ok("orkas");
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

        [Route("api/Producto/RecomendationsByViews")]
        [HttpPost]
        public IHttpActionResult GetRecomendationsByView(int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.getRecomendationsByViews(idUsuario);
            return Ok(res);
        }

        [Route("api/Caca")]
        [HttpGet]
        public IHttpActionResult PruebaMongo()
        {
            var client = new MongoClient();

            // Access database named 'test'
            var database = client.GetDatabase("Gran_Torismo");

            // Access collection named 'restaurants'
            var collection = database.GetCollection<Establecimientos>("Establecimientos");
            // 3. Query
            var filter = Builders<Establecimientos>.Filter.Empty;
            var result = collection.Find(filter).ToList();
            return Ok(result);
        }

        [Route("api/Caca2")]
        [HttpGet]
        public IHttpActionResult PruebaMongo2()
        {
            var client = MongoConnection.Instance;
            var result = client.getTodosServicios();
            return Ok(result);

        }

    }
}
