using FacturacionConsola.Domain;
using FacturacionConsola.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IProductRepository _productRepo;

        public InvoiceService(IInvoiceRepository invoiceRepo, IProductRepository productRepo)
        {
            _invoiceRepo = invoiceRepo;
            _productRepo = productRepo;
        }

        public Invoice CreateEmptyInvoice()
        {
            var inv = new Invoice
            {
                Number = _invoiceRepo.NextSequentialNumber().ToString()
            };
            return inv;
        }

        public void AddItem(Invoice invoice, int productId, int quantity)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor que cero.");

            var product = _productRepo.GetById(productId)
                ?? throw new ArgumentException("Producto no encontrado.");

            invoice.Items.Add(new InvoiceItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantity,
                IsTaxExempt = product.IsTaxExempt
            });
        }

        public void Save(Invoice invoice)
        {
            if (invoice.Items.Count == 0)
                throw new InvalidOperationException("No se puede guardar una factura sin ítems.");
            _invoiceRepo.Add(invoice);
        }

        // El calculador se pasa por parámetro (para reflejar cambios al vuelo)
        public IEnumerable<(Invoice invoice, decimal subtotal, decimal tax, decimal total)>
            ListInvoicesWithTotals(ITaxCalculator taxCalculator)
        {
            foreach (var inv in _invoiceRepo.GetAll().OrderBy(i => i.Date))
            {
                var subtotal = inv.Subtotal();
                var tax = inv.Tax(taxCalculator);
                var total = subtotal + tax;
                yield return (inv, subtotal, tax, total);
            }
        }
    }
}
