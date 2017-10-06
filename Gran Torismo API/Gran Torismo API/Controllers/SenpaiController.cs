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
using MongoConnect;
using System.Diagnostics;

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
        public System.Web.Mvc.ActionResult DeleteAdministrador(int? idCard)
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



      

        // Get all services
        [Route("api/Servicios/Get/all")]
        [HttpGet]
        public IHttpActionResult GetServices()
        {
            var mongoInstance = MongoConnection.Instance;

            ObjectParameter output = new ObjectParameter("responseMessage", typeof(string));
            var SQLServices = db.PR_GetServices().ToList();
            List<ServiciosSQLModel> services = new List<ServiciosSQLModel>();
            foreach (PR_GetServices_Result service in SQLServices)
            {
                ServiciosModel mongoService = mongoInstance.getServicio(service.IdService);
                ServiciosSQLModel currService = new ServiciosSQLModel
                {
                    idService = mongoService.idService,
                    nombre = mongoService.nombre,
                    precio = mongoService.precio,
                    status = service.State
                };

                services.Add(currService);

            }
            return Ok(services);
        }


        // Edit Service state
        [Route("api/Servicios/Edit/Status/")]
        [HttpPost]
        public IHttpActionResult EditServiceStatus(ServiceEstatus service)
        {
            var ret = db.PR_EditService(service.IdService, service.Status);
            return Ok(ret);
        }

        //Create review
        [Route("api/Review/Create")]
        [HttpPost]
        public IHttpActionResult createReview(Review review)
        {
            var ret = db.PR_CreateReview(review.IdClient, review.IdCheck, review.Description, review.Rating);
            return Ok(ret);
        }


        //GetLike
        [Route("api/Like/Get/{IdClient}/{IdService}")]
        [HttpGet]
        public IHttpActionResult getLike(int IdClient, int IdService) {
            var ret = db.PR_GetLike(IdClient, IdService);
            return Ok(ret);
        }

        //Likear un producto/servicio
        [Route("api/Like")]
        [HttpPost]
        public IHttpActionResult CreateLike(Like like)
        {
            var ret = db.PR_CreateLike(like.IdClient, like.IdService);
            return Ok(ret);
        }

        //Dislikear un producto/servicio
        [Route("api/Dislike")]
        [HttpPost]
        public IHttpActionResult DeleteLike(Like like)
        {
            var ret = db.PR_DeleteLike(like.IdClient, like.IdService);
            return Ok(ret);
        }

        [Route("api/Reviews/Get/{IdClient}")]
        [HttpGet]
        public IHttpActionResult GetReviews(int IdClient)
        {
            var ret = db.PR_GetReviews(IdClient);
            return Ok(ret);
        }


    }
}
