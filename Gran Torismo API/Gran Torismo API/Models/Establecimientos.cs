using MongoDB.Bson;
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

            [BsonElement("Nombre del establecimiento")]
            public string Nombre { get; set; }

            [BsonElement("Pueblo")]
            public string Pueblo { get; set; }

            [BsonElement("Distrito")]
            public string Distrito { get; set; }

            [BsonElement("Canton")]
            public string Canton { get; set; }

            [BsonElement("Provincia")]
            public string Provincia { get; set; }

            [BsonElement("Descripcion")]
            public string Descripcion { get; set; }

            [BsonElement("Fotos")]
            public string Fotos { get; set; }
    }
}