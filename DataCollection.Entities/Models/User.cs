using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Entities.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public List<ContactInformation> Contact { get; set; }
    }
    public class ContactInformation
    {
        public string UserId { get; set; }
        public ContactType Type { get; set; }
        public bool Allow { get; set; }
        /// <summary>
        /// E-Mail or Phone information
        /// </summary>
        public string Value { get; set; }
    }
}
