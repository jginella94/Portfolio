USE OUTATIME_INC
GO

-- Ejercicio 6 --  

--Creamos la tabla de auditoria--
CREATE TABLE auditoriaViajesRechazados
(
    idAuditoria INT IDENTITY(1,1),
    fecha DATETIME,
    usuarioBD VARCHAR(100),
    codChofer INT,
    codViaje INT,
    fechaSalida DATETIME,
    cantidadViajes INT,
    aniosAcumulados INT,

    CONSTRAINT PK_auditoriaViajesRechazados PRIMARY KEY(idAuditoria)
);
GO

CREATE TRIGGER tr_ControlChofer
ON viajes
INSTEAD OF INSERT
AS
BEGIN

    DECLARE @codChofer INT
    DECLARE @codViaje INT
    DECLARE @fechaSalida DATETIME

    DECLARE @cantidadViajes INT
    DECLARE @aniosAcumulados INT

    -- Datos del viaje que se intenta insertar
    SELECT
        @codChofer = i.codChoferAsignado,
        @codViaje = i.codViaje,
        @fechaSalida = i.fechaHora_salida
    FROM inserted i

    -- Cant de viajes y años acumulados actuales del chofer ACTUALMENTE
    SELECT
        @cantidadViajes = COUNT(*),
        @aniosAcumulados =
            ISNULL(
                SUM(
                    ABS(
                        DATEDIFF(
                            YEAR,
                            v.fechaHora_salida,
                            d.fecha_hora
                        )
                    )
                ),
                0
            )
    FROM viajes v
        JOIN destinos d ON d.codDestino = v.destino_codDestino
    WHERE v.codChoferAsignado = @codChofer
      AND v.estado = 'REALIZADO'

    -- Consideramos el viaje que se intenta ingresar
    SET @cantidadViajes = @cantidadViajes + 1

    SELECT
        @aniosAcumulados =
            @aniosAcumulados +
            ABS(
                DATEDIFF(
                    YEAR,
                    i.fechaHora_salida,
                    d.fecha_hora
                )
            )
    FROM inserted i
        JOIN destinos d ON d.codDestino = i.destino_codDestino

    -- Si supera alguno de los límites, auditar y rechazar
    IF @cantidadViajes > 15
       OR @aniosAcumulados > 800
    BEGIN

        INSERT INTO auditoriaViajesRechazados
        (fecha,usuarioBD,codChofer,codViaje,fechaSalida,cantidadViajes,aniosAcumulados)
        VALUES
        (GETDATE(),SYSTEM_USER,@codChofer,@codViaje,@fechaSalida,@cantidadViajes,@aniosAcumulados)

        PRINT 'Chofer excede los limites permitidos. Viaje rechazado.'

    END
    ELSE
    BEGIN

        INSERT INTO viajes
        (codViaje,codCliente,codOficina,fechaHoraContratacion,fechaHora_salida,destino_codDestino,fechaHora_vuelta,costo,estado,codChoferAsignado,patenteVehiculoAsignado)
        SELECT
            codViaje,
            codCliente,
            codOficina,
            fechaHoraContratacion,
            fechaHora_salida,
            destino_codDestino,
            fechaHora_vuelta,
            costo,
            estado,
            codChoferAsignado,
            patenteVehiculoAsignado
        FROM inserted

    END

END
GO


































-- Ejercicio 7 --  

CREATE TRIGGER trDevolverCreditoCliente
ON pagos
AFTER DELETE
AS
BEGIN

    UPDATE c
       SET c.LimiteCreditoMax =
           c.LimiteCreditoMax + d.montoTotal
    FROM clientes c
        JOIN viajes v ON c.codCliente = v.codCliente
        JOIN deleted d ON d.codViaje = v.codViaje
    WHERE v.estado = 'CANCELADO'

END
GO
