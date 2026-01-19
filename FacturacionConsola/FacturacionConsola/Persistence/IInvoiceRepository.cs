using System;
using System.Collections.Generic;
using System.Text;
using FacturacionConsola.Domain;

namespace FacturacionConsola.Persistence
{
    public interface IInvoiceRepository
    {
        void Add(Invoice invoice);
        IEnumerable<Invoice> GetAll();
        int NextSequentialNumber(); // Para número simple de factura
    }
}
