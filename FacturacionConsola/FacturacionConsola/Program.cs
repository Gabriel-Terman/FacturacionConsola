// See https://aka.ms/new-console-template for more information
using FacturacionConsola.Config;
using FacturacionConsola.Domain;
using FacturacionConsola.Persistence;
using FacturacionConsola.Services;
using System.Globalization;


namespace FacturacionConsola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Paths
            string productsPath = "products.json";
            string invoicesPath = "invoices.json";
            string configPath   = "config.json";

            // Cargar config y dependencias
            var configRepo = new ConfigRepository(configPath);
            var config = configRepo.Load();

            var productRepo = new JsonFileProductRepository(productsPath);
            var invoiceRepo = new JsonFileInvoiceRepository(invoicesPath);

            ITaxCalculator taxCalculator = new FixedRateTaxCalculator(config.TaxRate);

            var productService = new ProductService(productRepo);
            var invoiceService = new InvoiceService(invoiceRepo, productRepo);

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            while (true)
            {
                Console.WriteLine("==== Sistema de Facturación Terman ====");
                Console.WriteLine("1) Registrar producto");
                Console.WriteLine("2) Listar productos");
                Console.WriteLine("3) Crear factura");
                Console.WriteLine("4) Listar facturas");
                Console.WriteLine("5) Configurar tasa de impuesto (actual: " + (taxCalculator.Rate * 100).ToString("0.##") + "%)");
                Console.WriteLine("0) Salir");
                Console.Write("Seleccione una opción: ");
                var opt = Console.ReadLine();

                try
                {
                    switch (opt)
                    {
                        case "1":
                            RegisterProduct(productService);
                            break;
                        case "2":
                            ListProducts(productService);
                            break;
                        case "3":
                            CreateInvoice(invoiceService, productService, taxCalculator);
                            break;
                        case "4":
                            ListInvoices(invoiceService, taxCalculator);
                            break;
                        case "5":
                            ConfigureTax(configRepo, taxCalculator);   
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción inválida.\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
            }
        }

        static void RegisterProduct(ProductService productService)
        {
            Console.Write("Nombre: ");
            var name = (Console.ReadLine() ?? "").Trim();

            Console.Write("Precio: ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine("Precio inválido.\n");
                return;
            }

            Console.Write("¿Exento de impuesto? (s/n): ");
            var exento = (Console.ReadLine() ?? "").Trim().ToLower() == "s";

            var p = productService.Register(name, price, exento);
            Console.WriteLine($"Producto registrado: {p}\n");
        }

        static void ListProducts(ProductService productService)
        {
            var all = productService.ListAll();
            if (!System.Linq.Enumerable.Any(all))
            {
                Console.WriteLine("No hay productos.\n");
                return;
            }
            Console.WriteLine("Productos:");
            foreach (var p in all)
                Console.WriteLine(p);
            Console.WriteLine();
        }

        static void CreateInvoice(InvoiceService invoiceService, ProductService productService, ITaxCalculator taxCalculator)
        {
            var inv = invoiceService.CreateEmptyInvoice();
            Console.WriteLine($"Creando factura #{inv.Number} - {inv.Date}");
            Console.WriteLine("Tip: escribe 'g' para ir a guardar, 'c' para cancelar en cualquier momento.");

            while (true)
            {
                Console.Write("Ingrese ID de producto (ENTER, 'g' o 'c'): ");
                var input = (Console.ReadLine() ?? "").Trim().ToLower();

                if (string.IsNullOrEmpty(input) || input == "g") break;     
                if (input == "c") { Console.WriteLine("Factura cancelada.\n"); return; }

                if (!int.TryParse(input, out var productId))
                {
                    Console.WriteLine("Entrada inválida. Debe ser un ID numérico, 'g' o 'c'.\n");
                    continue;
                }

                var product = productService.FindById(productId);
                if (product == null)
                {
                    Console.WriteLine("Producto no encontrado. Use 'Listar productos' para ver IDs.\n");
                    continue;
                }

                Console.Write("Cantidad: ");
                if (!int.TryParse(Console.ReadLine(), out var qty) || qty <= 0)
                {
                    Console.WriteLine("Cantidad inválida.\n");
                    continue;
                }

                invoiceService.AddItem(inv, productId, qty);

                Console.WriteLine($"Producto agregado: {product.Name} x{qty} - Subtotal línea: {(product.Price * qty):N2}");

                Console.Write("¿Deseas agregar otro producto? (s/n): ");
                var mas = (Console.ReadLine() ?? "").Trim().ToLower();
                if (mas != "s") break;
            }

            if (inv.Items.Count == 0)
            {
                Console.WriteLine("Factura vacía. No se creó ninguna factura.\n");
                return;
            }

            // Resumen
            var subtotal = inv.Subtotal();
            var tax      = inv.Tax(taxCalculator);
            var total    = subtotal + tax;

            Console.WriteLine($"\nResumen Factura #{inv.Number}");
            Console.WriteLine("--------------------------------");
            foreach (var it in inv.Items)
                Console.WriteLine($"{it.ProductName} x{it.Quantity} @ {it.UnitPrice:N2} = {it.LineSubtotal():N2}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Subtotal: {subtotal:N2}");
            Console.WriteLine($"Impuestos ({taxCalculator.Rate:P0}): {tax:N2}");
            Console.WriteLine($"Total: {total:N2}");

            Console.Write("¿Guardar factura? (s/n): ");
            var save = (Console.ReadLine() ?? "").Trim().ToLower() == "s";
            if (save)
            {
                invoiceService.Save(inv);
                Console.WriteLine("Factura guardada.\n");
            }
            else
            {
                Console.WriteLine("Factura cancelada.\n");
            }
        }

        static void ListInvoices(InvoiceService invoiceService, ITaxCalculator taxCalculator)
        {
            var list = invoiceService.ListInvoicesWithTotals(taxCalculator);
            int count = 0;
            foreach (var (inv, subtotal, tax, total) in list)
            {
                count++;
                Console.WriteLine($"Factura #{inv.Number} - {inv.Date}");
                foreach (var it in inv.Items)
                    Console.WriteLine($"  - {it.ProductName} x{it.Quantity} @ {it.UnitPrice:N2} = {it.LineSubtotal():N2}");
                Console.WriteLine($"  Subtotal: {subtotal:N2} | Impuestos: {tax:N2} | Total: {total:N2}\n");
            }
            if (count == 0) Console.WriteLine("No hay facturas.\n");
        }

        // Acepta 0.18 (18%) o 18 (=> 0.18) y actualiza el MISMO objeto taxCalculator
        static void ConfigureTax(ConfigRepository configRepo, ITaxCalculator taxCalculator)
        {
            Console.Write("Nueva tasa de impuesto (ej. 0.18 para 18%): ");
            var raw = (Console.ReadLine() ?? "").Trim();

            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var input)
                || input < 0)
            {
                Console.WriteLine("Tasa inválida.\n");
                return;
            }

            decimal rate = input > 1 ? input / 100M : input; // 18 => 0.18

            // 1) Persistimos
            var newConfig = new FacturacionConsola.Config.Config { TaxRate = rate };
            configRepo.Save(newConfig);

            // 2) Actualizamos la instancia existente (en caliente)
            taxCalculator.UpdateRate(rate);

            Console.WriteLine("Tasa actualizada.\n");
        }

    }


}
