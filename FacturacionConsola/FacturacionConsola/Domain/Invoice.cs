using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Domain
{
    public class Invoice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Number { get; set; } = ""; // podría ser secuencial simple
        public DateTime Date { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();

        public decimal Subtotal() => Items.Sum(i => i.LineSubtotal());

        public decimal Tax(Services.ITaxCalculator calculator) => calculator.CalculateTax(Items);

        public decimal Total(Services.ITaxCalculator calculator) => Subtotal() + Tax(calculator);
    }
}
