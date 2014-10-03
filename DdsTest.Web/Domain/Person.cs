using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DdsTest.Web.Domain
{
    [DataContract]
    public class Person
    {
        [DataMember, Key]
        public Guid Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
    }
}