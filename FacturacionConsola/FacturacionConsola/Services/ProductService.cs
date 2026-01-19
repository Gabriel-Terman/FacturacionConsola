using FacturacionConsola.Persistence;
using FacturacionConsola.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }
        public Product Register(string name, decimal price, bool isTaxExempt)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("l nombre del producto es requerido.");
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.");

            var product = new Product
            {
                Name = name.Trim(),
                Price = price,
                IsTaxExempt = isTaxExempt
            };
            _repo.Add(product);
            return product;
        }

        public IEnumerable<Product> ListAll() => _repo.GetAll().OrderBy(p => p.Name);

        public Product? FindById(int id) => _repo.GetById(id);
    }
}
