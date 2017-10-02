using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NeoConnect;

namespace Gran_Torismo_API.Controllers
{
    public class SenpaiController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // Edita el nombre de una categoria
        [Route("api/Categorias/Edit")]
        [HttpPost]
        public IHttpActionResult EditCategoria(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ret = db.PR_EditCategory(category.Name, category.IdCategory);
            return Ok(ret);
        }

        // Edita el nombre de una categoria
        [Route("api/Categorias/Create")]
        [HttpPost]
        public IHttpActionResult CreateCategoria(Category category)
        {
            var ret = db.PR_CreateCategory(category.Name);
            return Ok(ret);
        }

        // Elimina una categoria
        [Route("api/Categorias/Delete")]
        [HttpPost]
        public IHttpActionResult DeleteCategoria(Category category)
        {
            var ret = db.PR_DeleteCategory(category.IdCategory);
            return Ok(ret);
        }

        //Get all admins  
        [Route("api/Administradores/Get")]
        
        public IHttpActionResult GetAdmins()
        {
            var ret = db.AdminDetails.ToList();
            return Ok(ret);
        }

        // Elimina un administrador
        [Route("api/Administradores/Delete/{idCard}")]
        [HttpPost]
        public System.Web.Mvc.ActionResult DeleteAdministrador(long idCard)
        {
            var ret = db.PR_DeleteAdmin(idCard);
             return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.OK);
        }


        // Crea un nuevo administrador
        [Route("api/Administradores/Create")]
        [HttpPost]
        public IHttpActionResult CreateAdministrador(User admin)
        {
            ObjectParameter output = new ObjectParameter("responseMessage", typeof(string));
            var ret = db.PR_CreateAdmin(admin.IdCard, admin.Username, admin.Password, admin.FirstName, admin.MiddleName, admin.LastName,
                admin.SecondLastName, output);
            RegisterResponse response = new RegisterResponse() { Response = output.Value.ToString() };
            return Ok(ret);
        }


    }
}
