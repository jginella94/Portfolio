USE OUTATIME_INC
GO

CREATE INDEX IDX_oficinas_codPais
ON oficinas(codPais);
GO

CREATE INDEX IDX_vehiculos_codPais
ON vehiculos(codPais);
GO

CREATE INDEX IDX_vehiculos_modelo
ON vehiculos(codModelo);
GO

CREATE INDEX IDX_vehiculos_oficina
ON vehiculos(oficinaAsignado);
GO

CREATE INDEX IDX_choferes_codPais
ON choferes(codPais);
GO

CREATE INDEX IDX_choferes_oficinaAsignado
ON choferes(oficinaAsignado);
GO

CREATE INDEX IDX_clientes_codPais
ON clientes(codPais);
GO

CREATE INDEX IDX_viajes_codCliente
ON viajes(codCliente);
GO

CREATE INDEX IDX_viajes_codOficina
ON viajes(codOficina);

CREATE INDEX IDX_viajes_destino_codDestino
ON viajes(destino_codDestino);
GO

CREATE INDEX IDX_viajes_codChoferAsignado
ON viajes(codChoferAsignado);
GO

CREATE INDEX IDX_viajes_patenteVehiculoAsignado
ON viajes(patenteVehiculoAsignado);
GO

CREATE INDEX IDX_pagos_codViaje
ON pagos(codViaje);
GO

CREATE INDEX IDX_devoluciones_codPago
ON devoluciones(codPago);
GO

CREATE INDEX IDX_viajes_fecha_salida
ON viajes(fechaHora_salida);
GO

CREATE INDEX IDX_viajes_fecha_vuelta
ON viajes(fechahora_vuelta);
GO