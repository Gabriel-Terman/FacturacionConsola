using FacturacionConsola.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Services
{
    public interface ITaxCalculator
    {
        decimal CalculateTax(IEnumerable<InvoiceItem> items);
        decimal Rate { get; }
        void UpdateRate(decimal newRate);
    }
}
