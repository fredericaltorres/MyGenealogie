using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyGenealogie.Console;

namespace MyGenealogie.Web.Controllers
{
    [Route("api/[controller]")]
    public class MyGenealogieController : Controller
    {
        private readonly PersonDB personDB;

        public MyGenealogieController(IPersonDB personDB)
        {
            this.personDB = personDB as PersonDB;
        }

        [HttpGet("[action]")]
        public List<PersonProperties> GetPersons()
        {
            var l = new List<PersonProperties>();
            l = this.personDB.Persons.Select(p => p.Properties).ToList();
            return l;
        }

        [HttpPut("[action]")]
        public IActionResult UpdatePerson([FromBody]PersonProperties personProperties)
        {
        
            var person = this.personDB.GetPersonByGuid(personProperties.Guid);
            if (person == null)
                return BadRequest($"Person guid:{personProperties.Guid}, Lastname:{personProperties.LastName}, FirstName:{personProperties.FirstName} not found in backend memory");
            
            if(this.personDB.UpdatePerson(personProperties))
                return Ok();
            else
                return new NotFoundObjectResult(personProperties.Guid); // TODO: Improve
        }

        [HttpDelete("[action]")]
        public IActionResult DeletePerson([FromBody] string guid)
        {
            if (this.personDB.DeletePerson(Guid.Parse(guid)))
                return Ok();
            else
                return new NotFoundObjectResult(guid); // TODO: Improve
        }

        [HttpPost("[action]")]
        public IActionResult NewPerson()
        {
            var person = this.personDB.NewPerson();
            return new OkObjectResult(person.Properties);
        }
    }
}
