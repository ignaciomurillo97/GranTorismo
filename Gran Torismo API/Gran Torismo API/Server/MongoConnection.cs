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

        public List<ServiciosModel> getServicios(int idEstablishment)
        {
            var collection = mongoDb.GetCollection<ServiciosModel>("Servicios");
            var filter = Builders<ServiciosModel>.Filter.Eq("idEstablishment", idEstablishment);
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

        public List<Establecimientos> getTodosEstablecimientos()
        {
            var collection = mongoDb.GetCollection<Establecimientos>("Establecimientos");
            var filter = Builders<Establecimientos>.Filter.Empty;
            List<Establecimientos> result = collection.Find(filter).ToList();
            return result;
        }

    }
}