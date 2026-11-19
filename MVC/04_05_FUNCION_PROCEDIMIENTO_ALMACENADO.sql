
-- Ejercicio 4 - (A) --  

USE OUTATIME_INC
GO

CREATE PROCEDURE ...

CREATE FUNCTION dbo.fnCategoriaCliente
(
    @codCliente INT
)
RETURNS VARCHAR(20)
AS
BEGIN

    DECLARE @cantidadViajes INT
    DECLARE @aniosAcumulados INT
    DECLARE @categoria VARCHAR(20)
	 
	--Calculo cantidad de viajes REALIZADOS por el cliente @codCliente
	--Calculo el tiempo de viaje acumulado (en valor absoluto) en total
	--Necesito los destinos para la fecha de llegada y calcular el salto trmporal
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
    WHERE v.codCliente = @codCliente
		AND v.estado = 'REALIZADO'


	--Clasifico
    IF @cantidadViajes = 0
        RETURN 'EXPLORADOR'

    SET @categoria =
        CASE
            WHEN @cantidadViajes <= 2
                 AND @aniosAcumulados < 30
                THEN 'TENIENTE'

            WHEN @cantidadViajes BETWEEN 3 AND 5
                 AND @aniosAcumulados BETWEEN 31 AND 60
                THEN 'COMANDANTE'

            WHEN @cantidadViajes > 6
                 AND @aniosAcumulados > 61
                THEN 'CORONEL'

            ELSE 'A REVISAR'
        END

    RETURN @categoria

END
GO

-- Ejercicio 5 - (A) --

USE OUTATIME_INC
GO


CREATE PROCEDURE dbo.spProcesarDevoluciones
(
    @estado VARCHAR(10),
    @cantidadDevoluciones INT OUTPUT,
    @mensaje VARCHAR(200) OUTPUT
)
AS
BEGIN

    SET @cantidadDevoluciones = 0
    SET @mensaje = ''
    
        -- Validamos el estado que recibimos
        IF @estado NOT IN ('RESERVADO', 'SUSPENDIDO')
        BEGIN
            SET @mensaje = 'Error: el estado debe ser RESERVADO o SUSPENDIDO'
            RETURN
        END

        -- Calculamos la cantidad de devoluciones a realizar
        SELECT @cantidadDevoluciones = COUNT(*)
        FROM pagos p
            JOIN viajes v ON p.codViaje = v.codViaje
        WHERE v.estado = @estado
		--Contrataciones con mas de 6 meses
          AND DATEDIFF(MONTH,
                       v.fechaHoraContratacion,
                       GETDATE()) > 6
          AND v.codOficina IN
		  --Subconsulta que devuelve las 4 oficinas con menos viajes en los ultimos 5 meses
          (
                SELECT TOP 4
                    v2.codOficina
                FROM viajes v2
                WHERE v2.estado = 'REALIZADO'
                  AND v2.fechaHora_salida >= DATEADD(MONTH, -5, GETDATE())
                GROUP BY v2.codOficina
                ORDER BY COUNT(*) ASC
          )

        -- Generamos una devolución del 35% para cada pago asociado
        INSERT INTO devoluciones
        (
            codPago,
            fechaDevolucion,
            montoDevuelto,
            motivo
        )
        SELECT
            p.codPago,
            GETDATE(),
            p.montoTotal * 0.35,
            'Devolucion automatica por politica de viajes antiguos'
        FROM pagos p
            JOIN viajes v
                ON p.codViaje = v.codViaje
        WHERE v.estado = @estado
          AND DATEDIFF(MONTH,
                       v.fechaHoraContratacion,
                       GETDATE()) > 6
          AND v.codOficina IN
          (
                SELECT TOP 4
                    v2.codOficina
                FROM viajes v2
                WHERE v2.estado = 'REALIZADO'
                  AND v2.fechaHora_salida >= DATEADD(MONTH, -5, GETDATE())
                GROUP BY v2.codOficina
                ORDER BY COUNT(*) ASC
          )

        -- Cancelamos los viajes afectados
        UPDATE viajes
        SET estado = 'CANCELADO'
        WHERE estado = @estado
          AND DATEDIFF(MONTH,
                       fechaHoraContratacion,
                       GETDATE()) > 6
          AND codOficina IN
          (
                SELECT TOP 4
                    v2.codOficina
                FROM viajes v2
                WHERE v2.estado = 'REALIZADO'
                  AND v2.fechaHora_salida >= DATEADD(MONTH, -5, GETDATE())
                GROUP BY v2.codOficina
                ORDER BY COUNT(*) ASC
          )

        SET @mensaje = 'Proceso ejecutado correctamente'

END
GO