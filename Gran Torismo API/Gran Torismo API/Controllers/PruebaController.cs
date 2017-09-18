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
            neo.agregarUsuario(idUsuario);
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
            neo.agregarProducto(idProducto);
            return Ok(idProducto);
        }

        [Route("api/Producto/Registrar/Vista")]
        public IHttpActionResult VerProducto(int idUsuario, int idProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var neo = NeoConnection.Instance;
            neo.verProducto(idUsuario, idProducto);
            return Ok("orkas");
        }

        [Route("api/Usuario")]
        public IHttpActionResult getAllUsers()
        {
            var neo = NeoConnection.Instance;
            List<NeoUser> res = neo.getAllUsers();
            return Ok(res);
        }

        [Route("api/Producto")]
        public IHttpActionResult getAllProducts()
        {
            var neo = NeoConnection.Instance;
            List<NeoProduct> res = neo.getAllProducts();
            return Ok(res);
        }
    }
}
