using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Domain
{
    public class InvoiceItem
    {
        // Snapshot para mantener histórico aunque cambie el producto después
        public int ProductId { get; set; }         
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsTaxExempt { get; set; }

        public decimal LineSubtotal() => UnitPrice * Quantity;

    }

}
