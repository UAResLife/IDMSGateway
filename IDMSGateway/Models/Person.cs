using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatCardGateway.Models
{
    public class Person
    {
        public int CardCount { get; set; }
        public List<CardHistory> CardHistory { get; set; }
        public string Customer { get; set; }
        public string Email { get; set; }
        public string EmplId { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string Index { get; set; }
        public string IsoNumber { get; set; }
        public string LastName { get; set; }
        public string ListName { get; set; }
        public string NetId { get; set; }
        public string Notes { get; set; }
        public string Photo { get; set; }
        public DateTime PhotoDate { get; set; }
        public string Signature { get; set; }
    }
}
