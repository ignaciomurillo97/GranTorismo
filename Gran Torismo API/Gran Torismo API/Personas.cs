using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Gran_Torismo_API
{
    public class Personas
    {
        public ObjectId _id;
        public string Nombre;
        public string Apellido;

        public Personas()
        {

        }

    }
}