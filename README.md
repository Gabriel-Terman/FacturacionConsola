
# 🧾 Sistema de Facturación – Consola en C#

Proyecto final de la semana de validación de **Programación Orientada a Objetos**.  
El objetivo es implementar un sistema de facturación básico con **C#**, usando **POO**, persistencia en **archivos JSON** y ejecución 100% en **consola**.

---

## 🚀 Características Principales

✔ Registrar productos  
✔ Emitir facturas  
✔ Agregar múltiples productos a una factura  
✔ Calcular Subtotal, Impuestos y Total  
✔ Configurar la tasa de impuesto en tiempo real  
✔ Listar facturas existentes  
✔ Guardado automático mediante archivos JSON  
✔ Arquitectura con alto nivel de cohesión y bajo acoplamiento  
✔ Sistema completamente funcional desde la consola  

---

## 🛠️ Tecnologías Utilizadas

- **C# .NET 6+**
- **Aplicación de Consola**
- **JSON para persistencia**
- Librerías estándar (`System.Text.Json`, `System.IO`)

Sin bases de datos y sin frameworks externos.

---

## 🧩 Arquitectura del Sistema (POO)

El sistema está dividido en capas claras:

### **1️⃣ Dominio**
Modela los objetos principales:

- `Product`
- `Invoice`
- `InvoiceItem`

### **2️⃣ Servicios (Reglas de Negocio)**

- `ProductService`
- `InvoiceService`
- `ITaxCalculator`
- `FixedRateTaxCalculator`

### **3️⃣ Persistencia**

Repositorios que leen/escriben JSON:

- `JsonFileProductRepository`
- `JsonFileInvoiceRepository`
- `ConfigRepository`

### **4️⃣ Consola (UI)**

- `Program.cs`
- Muestra menú, recibe entradas y usa los servicios.

---

## 📦 Estructura de Archivos

``
FacturacionConsola/
├─ Domain/
│   ├─ Product.cs
│   ├─ InvoiceItem.cs
│   └─ Invoice.cs
├─ Services/
│   ├─ ProductService.cs
│   ├─ InvoiceService.cs
│   ├─ ITaxCalculator.cs
│   └─ FixedRateTaxCalculator.cs
├─ Persistence/
│   ├─ FileStorage.cs
│   ├─ JsonFileProductRepository.cs
│   ├─ JsonFileInvoiceRepository.cs
│   └─ ConfigRepository.cs
├─ Program.cs
└─ (Archivos generados)
├─ products.json
├─ invoices.json
└─ config.json

## 🔄 Flujo de Funcionamiento

### **Registro de Producto**
El usuario ingresa:  
- nombre  
- precio  
- si es exento de impuestos  

### **Creación de Factura**
1. Se crea una factura vacía  
2. Se agregan productos (ID + cantidad)  
3. El sistema genera:
   - subtotal  
   - impuestos  
   - total  
4. El usuario confirma si desea guardar  
5. Se almacena en `invoices.json`

---

## 📊 Cálculos

Los cálculos se realizan dentro de las clases del dominio:

- `Invoice.Subtotal()`
- `Invoice.Tax(taxCalculator)`
- `Invoice.Total(taxCalculator)`

El impuesto se calcula mediante `ITaxCalculator`, lo que permite intercambiar la estrategia de cálculo sin modificar la factura.

---

## 💾 Persistencia (JSON)

Los repositorios utilizan `System.Text.Json` para serializar y deserializar listas:

- `products.json`: lista de productos  
- `invoices.json`: lista de facturas  
- `config.json`: tasa de impuesto  

Este método permite mantener un historial aun cuando se cierre la aplicación.

---

## ▶️ Ejecución

### Requisitos
- .NET 6+

### Comandos
```bash
dotnet build
dotnet run

🧪 Ejemplo de Uso
Registrar Producto
Nombre: Laptop
Precio: 35000
¿Exento de impuesto (s/n)?: n

Crear Factura
Ingrese ID de producto: 1
Cantidad: 2
¿Deseas agregar otro producto? (s/n): n

Resultado
Subtotal: 70000
Impuestos (18%): 12600
Total: 82600


📈 Mejoras Futuras

Agregar clientes
Generación de PDF
Descuentos por línea
Reportes de ventas
Validaciones y sanitización de entrada
Historias de usuario más completas


👤 Autor
Gabriel Terman
Proyecto desarrollado como parte del módulo de Programación Orientada a Objetos.
