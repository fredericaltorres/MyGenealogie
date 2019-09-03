using System;
using System.Collections.Generic;

namespace MyGenealogie
{
    public class DeleteImageInfo
    {
        public Guid Guid { get; set; }
        public string ImageFileName { get; set; }
    }

    public class PersonProperties
    {
        public string _Username;
        public string _PasswordHash;

        public string Sexe;
        public string BirthCity;
        public string BirthCountry;
        public string DeathCity;
        public string DeathCountry;
        public string Comment;

        public string LastName;
        public string MaidenName;
        public string FirstName;
        public string MiddleName;


        public Guid Guid;
        public Guid? FatherGuid;
        public Guid? MotherGuid;
        public Guid? SpouseGuid;
        public PersonDate CreationDate;
        public PersonDate DeathDate;
        public PersonDate BirthDate;

        public List<PersonImage> Images;
    }
}


