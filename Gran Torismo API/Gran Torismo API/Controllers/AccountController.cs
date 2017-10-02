using Gran_Torismo_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NeoConnect;

namespace Gran_Torismo_API.Controllers
{
    public class AccountController : ApiController
    {
        private SQLEntities db = new SQLEntities();

        // POST: api/Users
        [Route("api/Register/User/Client")]
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
            var neo = NeoConnection.Instance;
            neo.AddUser(Decimal.ToInt32(user.User.IdCard));
            RegisterResponse response = new RegisterResponse() { Response = output.Value.ToString() };
            return Ok(response);
        }

        // POST: api/Users
        [Route("api/Register/User/Owner")]
        [ResponseType(typeof(Owner))]
        public IHttpActionResult PostOwner(Owner user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ObjectParameter output = new ObjectParameter("responseMessage", typeof(string));
            db.PR_CreateOwner(user.User.IdCard, user.User.Username, user.User.Password, user.User.FirstName, user.User.MiddleName, user.User.LastName,
                user.User.SecondLastName, output);
            RegisterResponse response = new RegisterResponse() { Response = output.Value.ToString() };
            return Ok(response);
        }
        [Route("api/Register/User/Admin")]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult PostAdmin(Admin user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ObjectParameter output = new ObjectParameter("responseMessage", typeof(string));
            db.PR_CreateAdmin(user.User.IdCard, user.User.Username, user.User.Password, user.User.FirstName, user.User.MiddleName, user.User.LastName,
                user.User.SecondLastName, output);
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
            ObjectParameter role = new ObjectParameter("rol", typeof(string));
            db.PR_UsersLogin(loginModel.Username, loginModel.Password, Output, idCard, role);
            var r = new LoginResponse() { Success = false, Msg = Output.Value.ToString(), Role = role.Value.ToString()};
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

        // POST: api/User/5
        [Route("api/User/{id}")]
        [ResponseType(typeof(PR_GetUser_Result))]
        public IHttpActionResult GetUser(int id)
        {
            var loginModel = db.PR_GetUser(id).FirstOrDefault();
            if (loginModel == null)
            {
                return NotFound();
            }

            if (loginModel.AccountNumber == null)
            {
                loginModel.AccountNumber = 0;
            }

            return Ok(loginModel);
        }
    }
}
