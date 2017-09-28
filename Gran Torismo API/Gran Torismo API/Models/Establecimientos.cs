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

            private IEnumerable<ServiciosModel> _addresses;
            public ObjectId _id { get; set; }

            [BsonElement("idDistrito")]
            public double Distrito { get; set; }

            [BsonIgnoreIfNull]
            public ServiciosModel Servicios { get; set; }

            [BsonElement("Fotos")]
            public BsonDocument fotos { get; set; }

            [BsonElement("Latitud")]
            public string latitud { get; set; }

            [BsonElement("Longitud")]
            public string longitud { get; set; }
    }

    public class ServiciosModel
    {

        [BsonElement("ID")]
        public double ID { get; set; }

        [BsonElement("Nombre")]
        public string nombre { get; set; }

        [BsonElement("Descuento")]
        public string Descuento { get; set; }

        [BsonElement("Precio")]
        public double Precio { get; set; }

        [BsonElement("Foto")]
        public BsonDocument Fotos { get; set; }
    }
}