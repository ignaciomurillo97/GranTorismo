using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using NeoConnect;
using Gran_Torismo_API.Models;
using Gran_Torismo_API.NeoHelper;

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

        [Route ("api/Producto/RecomendationsByView")]
        [HttpPost]
        public IHttpActionResult GetRecomendationsByView (int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.GetRecomendationsByView(idUsuario);
            return Ok(res);
        }
    }
}
