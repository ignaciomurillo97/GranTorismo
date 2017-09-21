﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gran_Torismo_API.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SQLEntities : DbContext
    {
        public SQLEntities()
            : base("name=SQLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categories> Categories { get; set; }
    
        public virtual int PR_ClientLogin(string username, string password, ObjectParameter responseMessage, ObjectParameter idCard)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_ClientLogin", usernameParameter, passwordParameter, responseMessage, idCard);
        }
    
        public virtual int PR_CreateClient(Nullable<decimal> idCard, string username, string password, string firstName, string middleName, string lastName, string secondLastName, Nullable<decimal> accountNumber, ObjectParameter responseMessage)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var secondLastNameParameter = secondLastName != null ?
                new ObjectParameter("SecondLastName", secondLastName) :
                new ObjectParameter("SecondLastName", typeof(string));
    
            var accountNumberParameter = accountNumber.HasValue ?
                new ObjectParameter("AccountNumber", accountNumber) :
                new ObjectParameter("AccountNumber", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreateClient", idCardParameter, usernameParameter, passwordParameter, firstNameParameter, middleNameParameter, lastNameParameter, secondLastNameParameter, accountNumberParameter, responseMessage);
        }
    
        public virtual ObjectResult<PR_GetUser_Result> PR_GetUser(Nullable<decimal> idCard)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_GetUser_Result>("PR_GetUser", idCardParameter);
        }
    }
}
