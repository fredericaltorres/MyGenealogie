using System;
using System.Collections.Generic;

namespace MyGenealogie
{
    public class PersonProperties
    {
        public string Sexe;
        public string BirthCity;
        public string BirthCountry;
        public string DeathCity;
        public string DeathCountry;
        public string Comment;

        public string LastName;
        public string MaidenName;
        public string FirstName;

        public Guid Guid;
        public Guid? FatherGuid;
        public Guid? MotherGuid;
        public PersonDate CreationDate;
        public PersonDate DeathDate;
        public PersonDate BirthDate;

        public List<PersonImage> Images;
    }
}


