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
        public void AddUser(int idUsuario)
        {
            var usuario = new { IdCard = idUsuario };
            graphClient.Cypher.Create("(n:usuario{usuario})")
                .WithParam("usuario", usuario)
                .ExecuteWithoutResults();
        }

        // Agrega un producto
        public void AddProduct(int idProducto)
        {
            var producto = new { IdProduct = idProducto };
            graphClient.Cypher.Create("(n:producto{producto})")
                .WithParam("producto", producto)
                .ExecuteWithoutResults();
        }

        // Agrega una relacion usuario producto de vista
        public void AddView(int idUsuario, int idProducto)
        {
            graphClient.Cypher
                .Match("(a:usuario), (b:producto)")
                .Where((NeoUser a) => a.IdCard == idUsuario)
                .AndWhere((NeoProduct b) => b.IdProduct == idProducto)
                .Create("(a)-[:vio]->(b)")
                .ExecuteWithoutResults();
        }

        // Agrega una relacion usuario producto de compra
        public void AddPurchase(int idUsuario, int idProducto)
        {
            graphClient.Cypher
                .Match("(a:usuario), (b:producto)")
                .Where((NeoUser a) => a.IdCard == idUsuario)
                .AndWhere((NeoProduct b) => b.IdProduct == idProducto)
                .Create("(a)-[:compro]->(b)")
                .ExecuteWithoutResults();
        }

        // Devuelve todos los usuarios
        public List<NeoUser> GetAllUsers()
        {
            List<NeoUser> res = graphClient.Cypher
                .Match("(user:usuario)")
                .Return(user => user.As<NeoUser>())
                .Results.ToList();
            return res;
        }

        // Devueve todos los productos
        public List<NeoProduct> GetAllProducts()
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(product:producto)")
                .Return(product => product.As<NeoProduct>())
                .Results.ToList();
            return res;
        }

        public List<NeoProduct> GetRecomendationsByView(int userId)
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(persona1:usuario) -[:vio]->(producto1:producto) <-[:vio]- (persona2:usuario) -[vistas:vio]-> (producto2:producto)")
                .Where("persona1.IdCard = 2")
                .AndWhere("NOT (persona1)-[:vio]->(producto2)")
                .With("producto2, count(vistas) as rels")
                .OrderByDescending("rels")
                .ReturnDistinct(producto2 => producto2.As<NeoProduct>())
                .Results.ToList();
            return res;
        }

    }
}
