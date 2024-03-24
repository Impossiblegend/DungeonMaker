using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditCardWebService
{
    public class Card /*: Holder*/
    {
        public string number { get; set; }
        public Holder holder { get; set; }
        public string provider { get; set; }
        public int CVV { get; set; }
        public DateTime validity { get; set; }
        public int balance { get; set; }
        public int limit { get; set; }
        public Card() { }
        //public Card(string email, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string billingaddress, string number, 
        //    string provider, int CVV, string validity, int balance, int limit) : base(email, firstName, lastName, dateOfBirth, phoneNumber, billingaddress)
        //{
        //    this.number = number;
        //    this.provider = provider;
        //    this.CVV = CVV;
        //    this.validity = validity;
        //    this.balance = balance;
        //    this.limit = limit;
        //}
        public Card(string number, Holder holder, string provider, int CVV, DateTime validity, int balance, int limit)
        {
            this.number = number;
            this.holder = holder;
            this.provider = provider;
            this.CVV = CVV;
            this.validity = validity;
            this.balance = balance;
            this.limit = limit;
        }
        public Card(string number, Holder holder, string provider, int CVV, DateTime validity)
        {
            this.number = number;
            this.holder = holder;
            this.provider = provider;
            this.CVV = CVV;
            this.validity = validity;
        }
        public bool Equals(Card c) { return number == c.number && provider.Contains(c.provider) && CVV == c.CVV && c.validity == validity; }
    }
}