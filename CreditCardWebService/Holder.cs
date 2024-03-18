using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CreditCardWebService
{
    public class Holder
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string billingaddress { get; set; }
        public Holder() { }
        public Holder(string email, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string billingaddress)
        {
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dateOfBirth = dateOfBirth;
            this.phoneNumber = phoneNumber;
            this.billingaddress = billingaddress;
        }
    }
}