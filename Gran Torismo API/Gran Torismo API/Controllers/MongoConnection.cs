using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using Gran_Torismo_API.Models;

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
                mongoDb = mongoServer.GetDatabase("Gran Torismo");
                //var mongoDb = mongoClient.GetDatabase("Gran Torismo");
            }
            catch (Exception e)
            {
                Console.WriteLine("IOException source: {0}", e);
            }
        }

        public List<Establecimientos> getPersonas()
        {
            List<Establecimientos> listaEst = mongoDb.GetCollection<Establecimientos>("Establecimientos").FindAll().ToList();
            return listaEst;
        }
    }
}