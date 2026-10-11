# 🧩 MS-CONTRATO-FACTURACION

Modelo de dominio para la gestión de **contratos y facturación** del sistema.

---

## 📘 Descripción general

Este modelo define los agregados principales y sus relaciones dentro del contexto de facturación médica o de servicios.

### Agregados principales

- **Contrato (Aggregate Root)**
  - Gestiona la contratación de servicios.
  - Define qué servicios se han contratado, por cuánto tiempo y en qué condiciones.

- **Factura (Aggregate Root)**
  - Gestiona la facturación y los cobros.
  - Controla los detalles de las facturas y los pagos realizados.

---

## 🧱 Entidades y Value Objects

### Contrato
- `Contratold: Guid`
- `NumeroContrato: NumeroContrato`
- `FechaContrato: DateTime`
- `PeriodoContrato: PeriodoContrato`
- `Estado: EstadoContrato`
- Métodos:
  - `CrearContrato()`
  - `ActivarContrato()`
  - `Renovar()`
  - `AgregarServicio()`
  - `QuitarServicio()`
  - `CalcularMontoTotal()`
  - `DuracionDias(): Int`
  - `EstaVigente(): Bool`

### Factura
- `Facturald: Guid`
- `NumeroFactura: NumeroFactura`
- `FechaEmision: DateTime`
- `FechaVencimiento: DateTime`
- `EstadoFactura: EstadoFactura`
- Métodos:
  - `AgregarDetalle()`
  - `RegistrarPago()`
  - `CalcularSaldo()`
  - `DeterminarEstado()`
  - `Anular()`

---

## 💰 Objetos de valor

- **Monto**
  - `Valor: Decimal`
  - `Referencia: String`
  - Métodos: `CalcularCambio()`, `ValidarMetodoPago()`

- **PeriodoContrato**
  - `FechaInicio: DateTime`
  - `FechaFin: DateTime`

- **ServicioReferencia**
  - `Serviciold: String`
  - `Descripcion: String`

---

## ⚙️ Enumeraciones

| Enum | Valores |
|------|----------|
| **EstadoContrato** | Activo, Suspendido, Finalizado, Anulado |
| **EstadoFactura** | Pendiente, Pagada, Parcial, Anulada, Vencida |
| **MetodoPago** | Efectivo, Transferencia, Qr, Tarjeta, Otros |

---

## 🧾 Relaciones principales

- Un **Contrato** puede originar varias **Facturas**.
- Cada **Factura** puede tener múltiples **DetallesFactura**.
- Los **Pagos** se asocian a una **Factura** mediante `Facturald`.

---

## 🧠 Responsabilidades

- **Contrato:** define servicios, duración y condiciones.
- **Factura:** controla cobros, pagos y estados.

---

## 🧩 Autor

**Joel César Barco**  
Modelo UML: `MS-CONTRATO-FACTURACION.pdf`

---

# ms-Nutricenter-ContractsBilling
Microservicio de Contratos y Facturación para el proyecto Nutricenter, Modulo Testing
