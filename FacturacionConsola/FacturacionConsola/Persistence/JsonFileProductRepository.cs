using FacturacionConsola.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionConsola.Persistence
{
    public class JsonFileProductRepository : IProductRepository
    {
        private readonly string _path;
        private List<Product> _cache;

        public JsonFileProductRepository(string path)
        {
            _path = path;
            _cache = FileStorage.LoadList<Product>(_path);
        }

        public void Add(Product product)
        {

            if (product.Id == 0)
                product.Id = NextId();

            _cache.Add(product);
            Persist();
        }

        private int NextId()
        {
            var max = _cache.Select(p => p.Id).DefaultIfEmpty(0).Max();
            return max + 1;
        }


        public Product? GetById(int id) => _cache.FirstOrDefault(p => p.Id == id);

        public IEnumerable<Product> GetAll() => _cache.OrderBy(p => p.Name).ToList();

        public void Update(Product product)
        {
            var idx = _cache.FindIndex(p => p.Id == product.Id);
            if (idx >= 0)
            {
                _cache[idx] = product;
                Persist();
            }
        }

        public void Delete(int id)
        {
            _cache = _cache.Where(p => p.Id != id).ToList();
            Persist();
        }

        private void Persist() => FileStorage.SaveList(_path, _cache);
    }

}
