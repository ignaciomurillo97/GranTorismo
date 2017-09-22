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
            var user = new { IdCard = idUsuario };
            graphClient.Cypher
                .Merge("(user:user {IdCard: {IdCard}})")
                .OnCreate()
                .Set("user = {user}")
                .WithParams(new
                {
                    IdCard = user.IdCard,
                    user
                })
                .ExecuteWithoutResults();
        }

        // Agrega un producto
        public void AddProduct(int idProducto)
        {
            var product = new { IdProduct = idProducto };
            graphClient.Cypher
                .Merge("(product:product {IdProduct: {IdProduct}})")
                .OnCreate()
                .Set("product = {product}")
                .WithParams(new
                {
                    IdProduct = product.IdProduct,
                    product
                })
                .ExecuteWithoutResults();
        }

        // Agrega una relacion usuario producto de vista
        public void AddView(int idUsuario, int idProducto)
        {
            graphClient.Cypher
                .Match("(a:user), (b:product)")
                .Where((NeoUser a) => a.IdCard == idUsuario)
                .AndWhere((NeoProduct b) => b.IdProduct == idProducto)
                .Create("(a)-[:viewed]->(b)")
                .ExecuteWithoutResults();
        }

        // Agrega una relacion usuario producto de compra
        public void AddPurchase(int idUsuario, int idProducto)
        {
            graphClient.Cypher
                .Match("(a:user), (b:product)")
                .Where((NeoUser a) => a.IdCard == idUsuario)
                .AndWhere((NeoProduct b) => b.IdProduct == idProducto)
                .Create("(a)-[:purchased]->(b)")
                .ExecuteWithoutResults();
        }

        public void addFollowing(int followerId, int followedId)
        {
            graphClient.Cypher
                .Match("(a:user), (b:user)")
                .Where((NeoUser a) => a.IdCard == followerId)
                .AndWhere((NeoUser b) => b.IdCard == followedId)
                .CreateUnique("(a)-[:follows]->(b)")
                .ExecuteWithoutResults();
        }

        public void removeFollowing(int followerId, int followedId)
        {
            graphClient.Cypher
                .Match("(follower:user)-[r:follows]->(followed:user)")
                .Where((NeoUser follower) => follower.IdCard == followerId)
                .AndWhere((NeoUser followed) => followed.IdCard == followedId)
                .Delete("r")
                .ExecuteWithoutResults();
        }

        public List<NeoUser> GetUserFollows(int userId)
        {
            List<NeoUser> follows = graphClient.Cypher
                .Match("(user1:user)-[:follows]->(user2:user)")
                .Where((NeoUser user1) => user1.IdCard == userId)
                .Return(user2 => user2.As<NeoUser>())
                .Results.ToList();
            return follows;
        }

        // Devuelve todos los usuarios
        public List<NeoUser> GetAllUsers()
        {
            List<NeoUser> res = graphClient.Cypher
                .Match("(user:user)")
                .Return(user => user.As<NeoUser>())
                .Results.ToList();
            return res;
        }

        // Devueve todos los productos
        public List<NeoProduct> GetAllProducts()
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(product:product)")
                .Return(product => product.As<NeoProduct>())
                .Results.ToList();
            return res;
        }

        public List<NeoProduct> getRecomendationsByViews(int userId)
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(user1:user) -[:viewed]->(product1:product)<-[:viewed]-(user2:user)-[views:viewed]->(product2:product)")
                .Where((NeoUser user1) => user1.IdCard == userId)
                .AndWhere("not (user1)-[:viewed]->(product2)")
                .With("product2, count(views) as viewCount")
                .OrderByDescending("viewCount")
                .ReturnDistinct(product2 => product2.As<NeoProduct>())
                .Results.ToList();
            return res;
        }

        public List<NeoProduct> GetRecomendationsByCurrentView(int userId, int idProduct)
        {
            List<NeoProduct> res = graphClient.Cypher
                .Match("(user1:user), (product1:product)<-[:viewed]-(user2:user)-[views:viewed]->(product2:product)")
                .Where((NeoProduct producto1) => producto1.IdProduct == idProduct)
                .AndWhere((NeoUser user1) => user1.IdCard == userId)
                .AndWhere("not (user1)-[:viewed]->(product2)")
                .With("product2, count(views) as viewCount")
                .OrderByDescending("viewCount")
                .ReturnDistinct(product2 => product2.As<NeoProduct>())
                .Results.ToList();
            return res;
        }
    }
}
