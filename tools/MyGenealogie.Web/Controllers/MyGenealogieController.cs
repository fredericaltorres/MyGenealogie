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
    }
}
