﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.Models
{
        public class Establecimientos
        {

            public ObjectId _id { get; set; }
            public long idEstablishment { get; set; }
            public string nombre { get; set; }
            public string descripcion { get; set; }
            public double idDistrito { get; set; }
            public double latitud { get; set; }
            public double longitud { get; set; }
            public int idOwner { get; set; }

    }

    public class ServiciosModel
    {

        [BsonElement("_id")]
        public ObjectId _id { get; set; }

        [BsonElement("idService")]
        public int idService { get; set; }

        [BsonElement("idEstablishment")]
        public long idEstablishment { get; set; }

        [BsonElement("nombre")]
        public string nombre { get; set; }

        [BsonElement("descripcion")]
        public string descripcion { get; set; }

        [BsonElement("precio")]
        public int precio { get; set; }

        [BsonElement("fotos")]
        public BsonArray fotos { get; set; }
    }

    public class ServiciosSQLModel
    {
        [BsonElement("idService")]
        public int idService { get; set; }

        [BsonElement("nombre")]
        public string nombre { get; set; }        

        [BsonElement("precio")]
        public double precio { get; set; }

        public Boolean status { get; set; }

    }

}