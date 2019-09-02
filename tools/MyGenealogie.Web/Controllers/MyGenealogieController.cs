using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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


        //[HttpPost("[action]/{personGuid?}")]
        //public IActionResult UploadImage(string personGuid)

        // https://dotnetcoretutorials.com/2017/03/12/uploading-files-asp-net-core/
        [HttpPost("[action]")]
        public IActionResult UploadImage(IFormFile file)
        {
            var personGuid = this.HttpContext.Request.Headers["guid"];
            var person = this.personDB.GetPersonByGuid(Guid.Parse(personGuid));
            if (person == null)
                return BadRequest($"Person guid:{personGuid} not found in backend memory");

            var fileNameOnly = $"{personGuid}.{file.FileName}";
            var tmpPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), fileNameOnly);
            if (System.IO.File.Exists(tmpPath))
                System.IO.File.Delete(tmpPath);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(tmpPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var personImage = person.BuildPersonImage(tmpPath);
                this.personDB.UploadImage(person, personImage);

                person.Properties.Images.Add(personImage);
                if (this.personDB.UpdatePerson(person.Properties))
                    return new OkObjectResult(person.Properties);
            }
            return new NotFoundObjectResult(personGuid); // TODO: Improve
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
