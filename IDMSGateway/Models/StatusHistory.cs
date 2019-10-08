using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatCardGateway.Models
{
    public class StatusHistory
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string Reason { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}
