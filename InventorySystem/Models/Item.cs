using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        private static int _idBase = 1000000000;

        public static int NewId()
        {
            var id = _idBase;
            _idBase += 10;
            return id;
        }
    }
}
