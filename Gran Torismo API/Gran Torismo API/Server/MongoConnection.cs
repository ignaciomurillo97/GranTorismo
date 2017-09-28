using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using Gran_Torismo_API.Models;
using System.Web.Http;
using System.Web.Http.Description;

namespace MongoConnect
{
    public class MongoConnection
    {

        MongoClient mongoClient;
        IMongoDatabase mongoDb;

        public MongoConnection()
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

        public string getPersonas()
        {
            string listaEst = mongoDb.GetCollection<string>("Establecimientos").Find("Nombre").ToString();
            return listaEst;
        }
    }
}