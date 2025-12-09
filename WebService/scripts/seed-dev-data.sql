-- Poblar datos de prueba para Gestiones
SET NOCOUNT ON;

USE Gestiones;
GO

INSERT INTO PlanVentas (Nombre, Descripcion, CantidadCuotas, FechaAlta)
VALUES
('Plan Básico', 'Plan a 6 cuotas', 6, GETDATE()),
('Plan Premium', 'Plan a 12 cuotas', 12, GETDATE());
GO

INSERT INTO Usuarios (Nombre, Apellido, Email, FechaAlta, Contraseña, Rol, FechaUltimoAcceso, Estado)
VALUES
('Ana', 'Vega', 'ana.vega@example.com', GETDATE(), 'hash1', 'Administrador', GETDATE(), 1),
('Luis', 'Pérez', 'luis.perez@example.com', GETDATE(), 'hash2', 'Vendedor', GETDATE(), 1);
GO

INSERT INTO Clientes (Nombre, Apellido, Email, FechaAlta, TipoDocumento, NumeroDocumento, Telefono, Direccion)
VALUES
('Carla', 'Gómez', 'carla.gomez@example.com', GETDATE(), 1, '12345678', '+54 11 5555-1234', 'Calle Falsa 123'),
('Diego', 'Suárez', 'diego.suarez@example.com', GETDATE(), 1, '23456789', '+54 11 5555-5678', 'Av. Principal 456'),
('Lucía', 'Martínez', 'lucia.martinez@example.com', GETDATE(), 1, '34567890', '+54 11 5555-9012', 'Calle 9 de Julio 789'),
('Marcos', 'Ramírez', 'marcos.ramirez@example.com', GETDATE(), 2, 'AR1234567', '+54 11 5555-3456', 'Av. Libertador 1500'),
('Sofía', 'López', 'sofia.lopez@example.com', GETDATE(), 1, '45678901', '+54 11 5555-7890', 'Calle Corrientes 320'),
('Julián', 'Pereyra', 'julian.pereyra@example.com', GETDATE(), 1, '56789012', '+54 11 5555-2345', 'Av. Belgrano 980'),
('Valentina', 'Rossi', 'valentina.rossi@example.com', GETDATE(), 3, 'UY2345678', '+54 11 5555-6789', 'Calle San Martín 455'),
('Matías', 'Ibáñez', 'matias.ibanez@example.com', GETDATE(), 1, '67890123', '+54 11 5555-0123', 'Calle Mendoza 210'),
('Paula', 'Fernández', 'paula.fernandez@example.com', GETDATE(), 1, '78901234', '+54 11 5555-4567', 'Av. Santa Fe 1234'),
('Gabriel', 'Torres', 'gabriel.torres@example.com', GETDATE(), 2, 'AR7654321', '+54 11 5555-8910', 'Calle Mitre 760'),
('Camila', 'Sosa', 'camila.sosa@example.com', GETDATE(), 1, '89012345', '+54 11 5555-1111', 'Calle Moreno 640');
GO

INSERT INTO Pagos (ClientId, FechaPago, FechaAlta, [decimal(18,2)], NumeroCuota, PlanVentaId, UsuarioId, MetodoPagoId, Observaciones, Estado)
VALUES
(1, DATEADD(day, -2, GETDATE()), DATEADD(day, -1, GETDATE()), 12000.00, 1, 1, 1, 1, 'Pago inicial', 'Pagado'),
(2, DATEADD(day, -3, GETDATE()), DATEADD(day, -2, GETDATE()), 8000.00, 1, 2, 2, 1, 'Pago con tarjeta', 'Pagado'),
(1, DATEADD(day, -10, GETDATE()), DATEADD(day, -9, GETDATE()), 12000.00, 2, 1, 1, 2, 'Pago segunda cuota', 'Pagado'),
(2, DATEADD(day, -12, GETDATE()), DATEADD(day, -11, GETDATE()), 8000.00, 2, 2, 2, 2, 'Pago segunda cuota', 'Pagado'),
(1, DATEADD(day, -20, GETDATE()), DATEADD(day, -19, GETDATE()), 12000.00, 3, 1, 2, 1, 'Pago tercera cuota', 'Pagado'),
(2, DATEADD(day, -23, GETDATE()), DATEADD(day, -22, GETDATE()), 8000.00, 3, 2, 1, 3, 'Pago tercera cuota', 'Pagado'),
(1, DATEADD(day, -30, GETDATE()), DATEADD(day, -29, GETDATE()), 12000.00, 4, 1, 1, 2, 'Pago cuarta cuota', 'Pagado'),
(2, DATEADD(day, -33, GETDATE()), DATEADD(day, -32, GETDATE()), 8000.00, 4, 2, 2, 1, 'Pago cuarta cuota', 'Pagado'),
(1, DATEADD(day, -40, GETDATE()), DATEADD(day, -39, GETDATE()), 12000.00, 5, 1, 2, 3, 'Pago quinta cuota', 'Pagado'),
(2, DATEADD(day, -43, GETDATE()), DATEADD(day, -42, GETDATE()), 8000.00, 5, 2, 1, 2, 'Pago quinta cuota', 'Pagado'),
(1, DATEADD(day, -55, GETDATE()), DATEADD(day, -54, GETDATE()), 12000.00, 6, 1, 1, 1, 'Pago sexta cuota', 'Pagado'),
(2, DATEADD(day, -58, GETDATE()), DATEADD(day, -57, GETDATE()), 8000.00, 6, 2, 2, 1, 'Pago sexta cuota', 'Pagado'),
(1, DATEADD(day, -70, GETDATE()), DATEADD(day, -69, GETDATE()), 12000.00, 7, 1, 2, 2, 'Pago séptima cuota', 'Pagado'),
(2, DATEADD(day, -73, GETDATE()), DATEADD(day, -72, GETDATE()), 8000.00, 7, 2, 1, 3, 'Pago séptima cuota', 'Pagado'),
(1, DATEADD(day, -85, GETDATE()), DATEADD(day, -84, GETDATE()), 12000.00, 8, 1, 1, 3, 'Pago octava cuota', 'Pagado'),
(2, DATEADD(day, -88, GETDATE()), DATEADD(day, -87, GETDATE()), 8000.00, 8, 2, 2, 2, 'Pago octava cuota', 'Pagado'),
(1, DATEADD(day, -100, GETDATE()), DATEADD(day, -99, GETDATE()), 12000.00, 9, 1, 2, 1, 'Pago novena cuota', 'Pendiente'),
(2, DATEADD(day, -103, GETDATE()), DATEADD(day, -102, GETDATE()), 8000.00, 9, 2, 1, 2, 'Pago novena cuota', 'Pendiente'),
(1, DATEADD(day, -120, GETDATE()), DATEADD(day, -119, GETDATE()), 12000.00, 10, 1, 1, 1, 'Pago décima cuota', 'Pendiente'),
(2, DATEADD(day, -123, GETDATE()), DATEADD(day, -122, GETDATE()), 8000.00, 10, 2, 2, 3, 'Pago décima cuota', 'Pendiente'),
(1, DATEADD(day, -150, GETDATE()), DATEADD(day, -149, GETDATE()), 12000.00, 11, 1, 2, 2, 'Pago onceavo cuota', 'Pendiente'),
(2, DATEADD(day, -153, GETDATE()), DATEADD(day, -152, GETDATE()), 8000.00, 11, 2, 1, 1, 'Pago onceavo cuota', 'Pendiente');
GO
