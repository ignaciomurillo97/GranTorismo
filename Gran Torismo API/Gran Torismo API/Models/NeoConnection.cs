using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using Gran_Torismo_API.Models;
using Gran_Torismo_API.NeoHelper;

namespace NeoConnect
{
    public class NeoConnection
    {
        GraphClient graphClient;

        private static NeoConnection instance;
        private NeoConnection()
        {
            graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "caca");
            graphClient.Connect();
        }

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
        public void agregarUsuario(int idUsuario)
        {
            var usuario = new { IdCard = idUsuario };
            graphClient.Cypher.Create("(n:usuario{usuario})")
                .WithParam("usuario", usuario)
                .ExecuteWithoutResults();
        }

        // Agrega un producto
        public void agregarProducto(int idProducto)
        {
            var producto = new { IdProduct = idProducto };
            graphClient.Cypher.Create("(n:producto{producto})")
                .WithParam("producto", producto)
                .ExecuteWithoutResults();
        }

        // Agrega un producto 
        public void verProducto(int idUsuario, int idProducto)
        {
            Dictionary<String, Object> parametrosRelacion = new Dictionary<string, object>();

            graphClient.Cypher
                .Match("(a:usuario), (b:producto)")
                .Where((NeoUser a) => a.IdCard == idUsuario)
                .AndWhere((NeoProduct b) => b.IdProduct == idProducto)
                .Create("(a)-[:vio]->(b)")
                .ExecuteWithoutResults();
        }

        public List<NeoUser> getAllUsers()
        {
            List<NeoUser> res = graphClient.Cypher
                .Match("(user:usuario)")
                .Return(user => user.As<NeoUser>())
                .Results.ToList();
            return res;
        }

        public List<NeoProduct> getAllProducts()
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(product:producto)")
                .Return(product => product.As<NeoProduct>())
                .Results.ToList();
            return res;
        }
    }
}
