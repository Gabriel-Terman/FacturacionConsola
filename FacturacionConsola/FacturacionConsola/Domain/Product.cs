using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsTaxExempt { get; set; } = false;

        public override string ToString()
            => $"{Id} | {Name} | Precio: {Price:C2} | Exento: {(IsTaxExempt ? "Si" : "No")}";
    }
}
