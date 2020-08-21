using System.Collections.Generic;

namespace Parser
{
    //This DTO can be translated into a JSON or entity to be saved anywhere.
    public class TransactionDTO{
        public string Date { get; set; }
        public List<decimal> Amounts { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
