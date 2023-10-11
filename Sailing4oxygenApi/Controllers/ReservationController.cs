using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appwrite.Models;
using Appwrite;
using Appwrite.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sailing4oxygenApi.Controllers
{
    [Route("[controller]")]
    public class ReservationController : Controller
    {
        // GET: api/values
        [HttpGet]
        public async Task<DocumentList> Get()
        {

            var query = new List<string>
            {
            Query.OrderAsc("Return"),
            Query.GreaterThan("Return", DateTime.Now.AddDays(-1)),
            Query.Equal("userId", "6510a4573cf9dbd4ba6d"),
            Query.Select(new List<string> { { "$id" }, { "$createdAt" }, { "$updatedAt" }, { "$permissions" }, { "portsfrom.*" } })
            };
        

            var reservations = await Helpers.InitClient.databases.ListDocuments(Helpers.Envs.DatabaseId, Helpers.Envs.ReservationCollectionId, query);

            //Verify a valid user is currently logged in using Appwrite Authentication
            //Get all reservations where uiserid == logged in user and planned return date is in future

            return reservations;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Document Get(int id)
        {
            //Verify a valid user is currently logged in using Appwrite Authentication
            //

            return null;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

