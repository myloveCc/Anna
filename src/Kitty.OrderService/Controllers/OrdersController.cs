using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kitty.OrderService.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController:Controller
    {
        // GET api/orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "order1", "order2" };
        }

        // GET api/orders/5
        [HttpGet("{id}/{userid}")]
        public string Get(int id, int userid)
        {
            return "order";
        }

        // POST api/orders
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/orders/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
