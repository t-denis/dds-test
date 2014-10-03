using System;
using System.Collections.Generic;
using System.Linq;
using DdsTest.Web.Domain;
using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;

namespace DdsTest.Web.Services
{
    [EnableClientAccess]
    public class PeopleService : DomainService
    {
        private readonly List<Person> _people;

        public PeopleService()
        {
            _people = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "Bill",
                    LastName = "Gates"
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Steve",
                    LastName = "Jobs"
                },
            };
        }
        public IQueryable<Person> GetPeople()
        {
            return _people.AsQueryable();
        }

        public void InsertPerson(Person person) { }
        public void UpdatePerson(Person person) { }
        public void DeletePerson(Person person) { }
    }
}