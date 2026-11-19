
CREATE DATABASE OUTATIME_INC
GO
USE OUTATIME_INC
GO

CREATE TABLE paises(
codPais INT  NOT NULL,
descripcion VARCHAR(100),
CONSTRAINT PK_paises PRIMARY KEY (codPais)
);
GO

CREATE TABLE oficinas(
codOficina INT NOT NULL,
descripcion VARCHAR(50),
codPais INT NOT NULL,
direccion varchar(100),
CONSTRAINT PK_oficinas PRIMARY KEY (codOficina),
CONSTRAINT FK_oficinas_paises FOREIGN KEY (codPais) REFERENCES paises(codPais)
);
GO

CREATE TABLE modeloVehiculo(
codModelo int NOT NULL,
descripcion varchar(50),
consumoGWViaje decimal(8,2),
realizaVuelo bit,
tipoCombustible varchar(50),
CONSTRAINT PK_modelo_vehiculo PRIMARY KEY (codModelo),
CONSTRAINT CK_modelo_vehiculo_consumo
	CHECK(consumoGWViaje % 1.21 = 0),
CONSTRAINT CK_modelo_vehiculo_combustible
	CHECK(tipoCombustible in ('Plutonio','Residuos Orgánicos','Rocas Volcánicas','H2O'))
);
GO

CREATE TABLE vehiculos(
patente varchar(10),
codPais int NOT NULL,
codModelo int NOT NULL,
oficinaAsignado int NOT NULL,
CONSTRAINT PK_vehiculos PRIMARY KEY (patente),
CONSTRAINT FK_vehiculos_pais FOREIGN KEY (codPais) REFERENCES paises(codPais),
CONSTRAINT FK_vehiculos_modelo FOREIGN KEY (codModelo) REFERENCES modeloVehiculo(codModelo),
CONSTRAINT FK_vehiculos_oficina FOREIGN KEY (oficinaAsignado) REFERENCES oficinas(codOficina)
);
GO

CREATE TABLE destinos(
codDestino int NOT NULL,
descripcion varchar(100),
fecha_hora datetime,
CONSTRAINT PK_destinos PRIMARY KEY (codDestino)
);
GO

CREATE TABLE choferes(
codChofer int NOT NULL,
apellido varchar(50),
nombre varchar(50),
codPais int NOT NULL,
fnacimiento date,
oficinaAsignado int NOT NULL,
CONSTRAINT PK_choferes PRIMARY KEY (codChofer),
CONSTRAINT FK_choferes_pais FOREIGN KEY (codPais) REFERENCES paises(codPais),
CONSTRAINT FK_choferes_oficina FOREIGN KEY (oficinaAsignado) REFERENCES oficinas(codOficina)
);
GO

CREATE TABLE clientes(
codCliente int NOT NULL ,
apellido varchar(50),
nombre varchar(50),
codPais int NOT NULL,
LimiteCreditoMax decimal(8,2),
CONSTRAINT PK_clientes PRIMARY KEY (codCliente),
CONSTRAINT FK_clientes_pais FOREIGN KEY (codPais) REFERENCES paises(codPais)
);
GO

CREATE TABLE viajes(
codViaje int NOT NULL,
codCliente int NOT NULL,
codOficina int NOT NULL,
fechaHoraContratacion datetime,
fechaHora_salida datetime,
destino_codDestino int NOT NULL,
fechaHora_vuelta datetime,
costo money  check (costo > 0),
estado varchar(10) check (estado in ('RESERVADO','REALIZADO','CANCELADO','SUSPENDIDO','EN VIAJE')),
codChoferAsignado int NOT NULL,
patenteVehiculoAsignado varchar(10) NOT NULL,

CONSTRAINT PK_viajes PRIMARY KEY (codViaje),
CONSTRAINT FK_viajes_cliente FOREIGN KEY (codCliente) REFERENCES clientes(codCliente),
CONSTRAINT FK_viajes_oficina FOREIGN KEY (codOficina) REFERENCES oficinas(codOficina),
CONSTRAINT FK_viajes_destino FOREIGN KEY (destino_codDestino) REFERENCES destinos(codDestino),
CONSTRAINT FK_viajes_chofer FOREIGN KEY (codChoferAsignado) REFERENCES choferes(codChofer),
CONSTRAINT FK_viajes_vehiculo FOREIGN KEY (patenteVehiculoAsignado) REFERENCES vehiculos(patente),

CONSTRAINT UK_DESTINO UNIQUE(codCliente,destino_codDestino),
CONSTRAINT CH_FECHAS_VIAJE CHECK(fechaHora_salida < fechaHora_vuelta),
CONSTRAINT CH_FECHAS_CONTRATO CHECK(fechaHoraContratacion < fechaHora_vuelta AND fechaHoraContratacion < fechaHora_salida)
);
GO

CREATE TABLE pagos(
codPago int NOT NULL,
codViaje int NOT NULL,
metodo varchar(30) CHECK (metodo in ('CREDITO','DEBITO','EFECTIVO','TRANSFERENCIA')),
cuotas int check (cuotas > 0),
montoTotal money check (montoTotal > 0),
CONSTRAINT PK_pagos PRIMARY KEY (codPago),
CONSTRAINT FK_pagos_viaje FOREIGN KEY (codViaje) REFERENCES viajes (codViaje)
);
GO

CREATE TABLE devoluciones(
codDevolucion int IDENTITY(1,1) NOT NULL,
codPago int NOT NULL,
fechaDevolucion datetime, 
montoDevuelto money NOT NULL,
motivo varchar(100),
CONSTRAINT PK_devoluciones PRIMARY KEY (codDevolucion),
CONSTRAINT FK_devoluciones_pago FOREIGN KEY (codPago) REFERENCES pagos(codPago),
CONSTRAINT CK_devoluciones_montoDevuelto
	CHECK(montoDevuelto  >0)
);
GO


DELETE FROM devoluciones

DELETE FROM PAGOS

DELETE FROM VIAJES
