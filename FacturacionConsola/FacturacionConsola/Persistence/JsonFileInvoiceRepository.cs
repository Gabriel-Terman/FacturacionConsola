using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FacturacionConsola.Domain;

namespace FacturacionConsola.Persistence
{
    public class JsonFileInvoiceRepository : IInvoiceRepository
    {
        private readonly string _path;
        private List<Invoice> _cache;

        public JsonFileInvoiceRepository(string path)
        {
            _path = path;
            _cache = FileStorage.LoadList<Invoice>(_path);
        }

        public void Add(Invoice invoice)
        {
            _cache.Add(invoice);
            Persist();
        }

        public IEnumerable<Invoice> GetAll() => _cache.ToList();

        public int NextSequentialNumber()
        {
            //Numero correlativo simple basado en conteo (no es thread-safe)
            var max = _cache
                .Select(i => int.TryParse(i.Number, out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();
            return max + 1;
        }

        private void Persist() => FileStorage.SaveList(_path, _cache);
    }
}
