using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;

namespace NeoConnect
{
    public class NeoConnection
    {
        GraphClient graphClient = new GraphClient(new Uri("http://localhost:7474"), "neo4j", "caca");

        private static NeoConnection instance;
        private NeoConnection() { }
        public static NeoConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NeoConnection();
                }
                return instance;
            }
        }

        // Agrega un usuario 
        public void agregarUsuario(int idUuario)
        {
            var usuario = new { id = idUuario };
            graphClient.Cypher.Create("n:usuario{usuario}")
                .WithParam("usuario", usuario)
                .ExecuteWithoutResults();
        }

        // Agrega un producto
        public void agregarProducto(int productId)
        {
            var producto = new { id = productId };
            graphClient.Cypher.Create("n:producto{producto}")
                .WithParam("producto", producto)
                .ExecuteWithoutResults();
        }
    }
}
