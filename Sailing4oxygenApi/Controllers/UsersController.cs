using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appwrite;
using Appwrite.Services;
using Appwrite.Models;
using Appwrite.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//ToDo: DRAGONS UPON US! THERE IS ZERO SECURITY HERE.
//ToDo: DRAGONS UPON US! THERE IS ZERO SECURITY HERE.
//ToDo: DRAGONS UPON US! THERE IS ZERO SECURITY HERE.
//ToDo: DRAGONS UPON US! THERE IS ZERO SECURITY HERE.

namespace Sailing4oxygenApi.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        //// GET: Users
        //[HttpGet]
        //public async Task<UserList> Get()
        //{
        //    Users users = new Users(Helpers.InitClient.AppwriteClient);
        //    return await users.List();
        //}

        //// GET users/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    //restrict to special cases
        //    return "none";
        //}


        // POST Users
        [HttpPost]
        public async Task<Dictionary<string, object>> Post([FromBody] Models.NewUser newUser)
        {
            var result = new Dictionary<string, object>();

            try
            {
                Users users = new Users(Helpers.InitClient.AppwriteClient);

                User user = await users.Create(
                    ID.Unique(),
                    email: newUser.Email,
                    name: newUser.Firstname + " " + newUser.Name,
                    password: newUser.Password
                    );

                result.Add("User", user);

            }
            catch (Exception ex)
            {
                Helpers.Envs.Messages.Add(ex.Message);
                Helpers.Envs.MessagesDump();
            }

            
            return result;

            //
            // ==> SERVER SITE Function needs to listen to creation events and
            //      a) initiate Verificationmail
            //      b) enlist registration for manual clearing
        }

        //// PUT Users/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        ////PUT Users/Me
        //[HttpPut("Me")]
        //public string Get([FromBody] string id, [FromBody] string value)
        //{

        //    Console.WriteLine("Me triggered");

        //    //verify logged in User and return the current users UserID
        //    return "you changed";
        //}

        //// DELETE Users/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

