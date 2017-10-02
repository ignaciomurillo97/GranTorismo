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
using System.IO;
using System.Drawing;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Web;
using MongoConnect;
using System.Diagnostics;
using System.Collections;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using MongoDB.Bson;

namespace Gran_Torismo_API.Controllers
{
    public class JhordanController : ApiController
    {
        private SQLEntities db = new SQLEntities();


        // Devuelve todos los distritos
        [Route("api/District/")]
        [HttpGet]
        public IHttpActionResult GetDistricts()
        {
            return Ok(db.PR_GetDistricts().ToList());
        }

        public void savePhoto(String Photo, String path)
        {
            byte[] bytes = Convert.FromBase64String(Photo);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image image = Image.FromStream(ms);
                String filePath = HostingEnvironment.MapPath(path);
                image.Save(filePath);
            }
        }

        // Registra un establecimiento
        [Route("api/Establecimiento/Create")]
        [HttpPost]
        public IHttpActionResult CreateEstablecimiento(Establecimientos establecimiento)
        {
            var mongoConnection = MongoConnection.Instance;
            establecimiento.idEstablishment = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var success = mongoConnection.createEstablecimiento(establecimiento);

            return Ok(success);
        }

        // Muestra los establecimientos
        [Route("api/get/Establecimiento/{IdEstablecimiento}")]
        [HttpGet]
        public IHttpActionResult GetEstablecimientos(int IdEstablecimiento)
        {
            var mongoConnection = MongoConnection.Instance;
            return Ok(mongoConnection.getEstablecimiento(IdEstablecimiento));
        }

        // Muestra los establecimientos
        [Route("api/Establecimiento/Index/{IdOwner}")]
        [HttpGet]
        public IHttpActionResult GetEstablecimiento(int IdOwner)
        {
            var mongoConnection = MongoConnection.Instance;
            return Ok(mongoConnection.getTodosEstablecimientos(IdOwner));
        }

        // Muestra todos los servicios de un establecimiento
        [Route("api/Servicios/{IdEstablecimiento}")]
        [HttpGet]
        public IHttpActionResult GetServicio(int IdEstablecimiento)
        {
            var mongoConnection = MongoConnection.Instance;
            return Ok(mongoConnection.getServicios(IdEstablecimiento));
        }

        // Registra un Servicio
        [Route("api/Servicio/Create")]
        public IHttpActionResult CreateService(ServiciosModel service)
        {
            var mongoConnection = MongoConnection.Instance;
            int sqlService = Convert.ToInt32(db.PR_CreateService().ToList()[0]);
            service.fotos = new BsonArray();
            service.idService = sqlService;
            var success = mongoConnection.createServicio(service);
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Images/" + sqlService + "/"));
            return (success) ? Ok(sqlService) : Ok(-1);
        }

        // Edita un Servicio
        [Route("api/Servicio/Edit/")]
        public IHttpActionResult EditService(ServiciosModel service)
        {
            var mongoConnection = MongoConnection.Instance;
            var success = mongoConnection.editServicio(service);
            return Ok(success);
        }

        [Route("api/Upload")]
        public async Task<HttpResponseMessage> PostSurveys()
        {
            var idService = HttpContext.Current.Request.Form["idService"];
            BsonArray filesNames = MongoConnection.Instance.getServicio(Convert.ToInt32(idService)).fotos;
            string uploadFolder = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(uploadFolder);
            MultipartFileStreamProvider multipartFileStreamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
            string StoragePath = "~/Images/" + idService + "/";
            String filePath = HostingEnvironment.MapPath(StoragePath);
            foreach (MultipartFileData fileData in streamProvider.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                }
                string fileName =  fileData.Headers.ContentDisposition.FileName;
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(fileName);
                Debug.WriteLine("----------------------------------------------------------");
                Debug.WriteLine(fileName);
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');

                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }
                if (fileName != "")
                {
                    var tmp_name = Convert.ToString((DateTime.UtcNow.Subtract(new DateTime(2017, 1, 1))).TotalSeconds);
                    tmp_name = tmp_name.Trim('.');
                    fileName = tmp_name + "_" + fileName;
                    filesNames.Add(fileName);
                    File.Move(fileData.LocalFileName, Path.Combine(filePath, fileName));
                }
            }
            MongoConnection.Instance.addPhotos(filesNames, idService);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // Get Service
        [Route("api/get/Service/{IdServicio}")]
        [HttpGet]
        public IHttpActionResult getAPIServicio(int IdServicio)
        {
            var mongoConnection = MongoConnection.Instance;
            return Ok(mongoConnection.getServicio(IdServicio));
        }

        [Route("api/Show/Image/{name}/{idService}")]
        [HttpGet]
        public HttpResponseMessage GetPhoto(string name, int idService)
        {
            if (!name.Contains("."))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            string ImagePath = "~/Images/" + idService + "/";
            String filePath = HostingEnvironment.MapPath(ImagePath + name);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Image image = Image.FromStream(fileStream);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            result.Content = new ByteArrayContent(memoryStream.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return result;
        }



    }




}
