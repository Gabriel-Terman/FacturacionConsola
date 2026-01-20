using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Domain
{
    public class Invoice
    {
        public int Id { get; set; } = 0; // lo asigna el repositorio 
        public string Number { get; set; } = ""; // Asigna un valor inicial vacío a la propiedad cuando se crea una instancia de la clase (número correlativo simple)
        public DateTime Date { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();
    
        public decimal Subtotal() => Items.Sum(i => i.LineSubtotal());
    
        public decimal Tax(Services.ITaxCalculator calculator) => calculator.CalculateTax(Items);
    
        public decimal Total(Services.ITaxCalculator calculator) => Subtotal() + Tax(calculator);
    }
}

