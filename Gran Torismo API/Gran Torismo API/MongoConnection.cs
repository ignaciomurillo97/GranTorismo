using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gran_Torismo_API
{
    public class MongoConnection
    {
        MongoDatabase mongoDb;
        MongoServer mongoServer;
        MongoClient mongoClient;

        public MongoConnection()
        {

            try
            {
                mongoClient = new MongoClient("mongodb://localhost:27017");
                mongoServer = mongoClient.GetServer();
                mongoDb = mongoServer.GetDatabase("Prueba");
            }
            catch (Exception e)
            {
                Console.WriteLine("IOException source: {0}", e);
            }
        }

        public List<Personas> getPersonas()
        {
            List<Personas> listaPersonas = mongoDb.GetCollection<Personas>("Personas").FindAll().ToList();
            return listaPersonas;
        }
    }
}