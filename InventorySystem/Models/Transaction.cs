using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public enum TransactionStatus
    {
        Processed,
        Processing,
        Pending,
        Failed
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
