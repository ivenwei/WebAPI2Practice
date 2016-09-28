using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2WithFormAuth.Controllers
{
    public class Person
    {
        public string name { get; set; }
        public string age { get; set; }
    }

    public class ModelBindController : ApiController
    {
        // Use query string call webapi and return message
        // PostMan test use GET method
        // PostMan test URL : http://localhost:49169/ModelBind1?name=Ivan&age=30
        [Route("ModelBind1")]
        public IHttpActionResult GetModelBind1(string name, string age)
        {
            return Ok(name + "-" + age);
        }

        // Use query string call webapi with body content and return message
        // PostMan test use POST method
        // PostMan test Content-Type: application/json
        // PostMan test body : "30"
        // PostMan test URL : http://localhost:49169/ModelBind2?name=Ivan
        [Route("ModelBind2")]
        public IHttpActionResult PostModelBind2(string name, [FromBody]string age)
        {
            return Ok(name + "-" + age);
        }


        // Use input json body and return message
        // PostMan test use POST method
        // PostMan test URL : http://localhost:49169/ModelBind3
        // PostMan test Content-Type: application/json
        // PostMan test body : { "age": 30, "name": "Ivan" }
        //=================================================================
        // PostMan test use POST method
        // PostMan test URL : http://localhost:49169/ModelBind3
        // PostMan test Content input method: application/x-www-form-urlencoded
        // PostMan test body : key:age vlaue:30 &  key:name vlaue:Ivan
        [Route("ModelBind3")]
        public IHttpActionResult PostModelBind3(Person person)
        {
            return Ok(person.name + "-" + person.age);
        }


        // Set object as query string call webapi and return message
        // PostMan test use POST method
        // PostMan test Content-Type: application/json
        // PostMan test URL : http://localhost:49169/ModelBind4?age=30&name=Ivan
        [Route("ModelBind4")]
        public IHttpActionResult PostModelBind4([FromUri]Person person)
        {
            return Ok(person.name + "-" + person.age);
        }

    }
}
