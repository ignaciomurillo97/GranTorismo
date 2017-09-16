﻿using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Gran_Torismo_API.Controllers
{
    public class AccountController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // POST: api/Users
        [Route("api/Register/User")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ObjectParameter output = new ObjectParameter("responseMessage", typeof(string));
            db.PR_CreateClient(user.User.IdCard, user.User.Username, user.User.Password, user.User.FirstName, user.User.MiddleName, user.User.LastName,
                user.User.SecondLastName, user.AccountNumber, output);
            RegisterResponse response = new RegisterResponse() { Response = output.Value.ToString() };
            return Ok(response);
        }

        // POST: api/Login
        [Route("api/Login/Authenticate")]
        [ResponseType(typeof(LoginRequest))]
        public IHttpActionResult PostClientLogin(LoginRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ObjectParameter Output = new ObjectParameter("responseMessage", typeof(string));
            ObjectParameter idCard = new ObjectParameter("idCard", typeof(int));
            db.PR_ClientLogin(loginModel.Username, loginModel.Password, Output, idCard);
            var r = new LoginResponse() { Success = false, Msg = Output.Value.ToString() };
            if (!(idCard.Value is DBNull))
            {
                r.IdCard = Convert.ToInt32(idCard.Value);
            }

            if (Output.Value.ToString().Equals("User successfully logged in"))
            {
                r.Success = true;
            }

            return Ok(r);
        }
    }
}