using FacturacionConsola.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Persistence
{
    public interface IProductRepository
    {
        void Add(Product product);
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        void Update(Product product);
        void Delete(int id);
    }
}
