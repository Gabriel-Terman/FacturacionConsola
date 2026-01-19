using FacturacionConsola.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Services
{
    public class FixedRateTaxCalculator : ITaxCalculator
    {
        private decimal _rate;
        public decimal Rate => _rate;
        public FixedRateTaxCalculator(decimal rate)
        {
            _rate = rate < 0 ? 0 : rate;
        }
        public void UpdateRate(decimal newRate)
        {
            _rate = newRate < 0 ? 0 : newRate;
        }
        public decimal CalculateTax(IEnumerable<InvoiceItem> items)
        {
            var taxableSubtotal = items
                .Where(i => !i.IsTaxExempt)
                .Sum(i => i.LineSubtotal());

            return decimal.Round(taxableSubtotal * _rate, 2);
        }
    }
}
