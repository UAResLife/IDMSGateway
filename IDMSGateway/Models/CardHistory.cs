using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatCardGateway.Models
{
    public class CardHistory
    {
        public int CardId { get; set; }
        public string CardLayout { get; set; }
        public int CardVariableNr { get; set; }
        public string IsoNumber { get; set; }
        public int PrintCount { get; set; }
        public List<StatusHistory> StatusHistory { get; set; }
    }
}
