using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

using Gran_Torismo_API.Models;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using System.Collections;

namespace MongoConnect
{
    public class MongoConnection
    {

        MongoClient mongoClient;
        IMongoDatabase mongoDb;
        private static MongoConnection instance;

        public static MongoConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MongoConnection();
                }
                return instance;
            }
        }
        
        private MongoConnection()
        {

            try
            {
                mongoClient = new MongoClient("mongodb://localhost:27017");
                mongoDb = mongoClient.GetDatabase("Gran_Torismo");
            }
            catch (Exception e)
            {
                Console.WriteLine("IOException source: {0}", e);
            }
        }

        public Establecimientos getEstablecimiento(int idEstablishment)
        {
            var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
            var filter = Builders<Establecimientos>.Filter.Eq("idEstablishment" , idEstablishment);
            Establecimientos result;
            try {
                result = collection.Find(filter).Single();
            }catch (InvalidOperationException e)
            {
                result = null;
            }
            return result;
        }

        public ServiciosModel getServicio(int idService , int idEstablishment)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idService", idService) & Builders<ServiciosModel>.Filter.Eq("idEstablishment", idEstablishment);
            ServiciosModel result;
            try
            {
                result = collection.Find(filter).Single();
            }
            catch (InvalidOperationException e) {
                result = null;
            }
            return result;
            
        }

        public ServiciosModel getServicio(int idService)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idService", idService);
            ServiciosModel result;
            try
            {
                result = collection.Find(filter).Single();
            }
            catch (InvalidOperationException e)
            {
                result = null;
            }
            return result;

        }

        public List<ServiciosModel> getServicios(int idEstablishment)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idEstablishment", idEstablishment);
            List<ServiciosModel> result = collection.Find(filter).ToList();
            return result;

        }
        public List<ServiciosModel> getServiciosFromList(List<int?> servicios)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.In("idService",servicios);
            List<ServiciosModel> result = collection.Find(filter).ToList();
            return result;
        }

        public List<ServiciosModel> getTodosServicios()
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Empty;
            List<ServiciosModel> result = collection.Find(filter).ToList();
            return result;

        }

        public List<Establecimientos> getEstablecimientos(int IdOwner)
        {
            var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
            var filter = Builders<Establecimientos>.Filter.Eq("idOwner", IdOwner);
            List<Establecimientos> result = collection.Find(filter).ToList();
            return result;
        }

        public List<Establecimientos> getTodosEstablecimientos()
        {
            var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
            var filter = Builders<Establecimientos>.Filter.Empty;
            List<Establecimientos> result = collection.Find(filter).ToList();
            return result;
        }

        public bool createEstablecimiento(Establecimientos establecimiento)
        {
            try
            {
                var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
                collection.InsertOne(establecimiento);
                return true;
            }
            catch {
                return false;
            };
        }

        public bool editServicio(ServiciosModel servicio) {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idService", servicio.idService);
            var update = Builders<ServiciosModel>.Update.Set("fotos", servicio.fotos)
                .Set("nombre", servicio.nombre)
                .Set("descripcion", servicio.descripcion)
                .Set("precio", servicio.precio);
            try
            {
                collection.FindOneAndUpdate(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool editEstablecimiento(Establecimientos establecimiento)
        {
            var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
            var filter = Builders<Establecimientos>.Filter.Eq("idEstablishment", establecimiento.idEstablishment);
            var update = Builders<Establecimientos>.Update.Set("idDistrito", establecimiento.idDistrito)
                .Set("nombre", establecimiento.nombre)
                .Set("descripcion", establecimiento.descripcion)
                .Set("latitud", establecimiento.latitud)
                .Set("longitud", establecimiento.longitud);
            try
            {
                collection.FindOneAndUpdate(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool addPhotos(BsonArray filesNames, string idService)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idService", idService);
            var update = Builders<ServiciosModel>.Update.Set("fotos", filesNames);
            try
            {
                collection.FindOneAndUpdate(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createServicio(ServiciosModel servicio)
        {
            try
            {
                var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
                collection.InsertOne(servicio);
                return true;
            }
            catch
            {
                return false;
            };
        }

    }
}