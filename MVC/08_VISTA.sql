-- Ejercicio 8 --  

USE OUTATIME_INC
GO


CREATE VIEW vw_DestinosDestacados
AS

SELECT
    d.codDestino,
    d.descripcion,

    -- Cantidad de viajes REALIZADOS al destino
    SUM(
        CASE
            WHEN v.estado = 'REALIZADO' THEN 1
            ELSE 0 --si no es REALIZADO no se suma
        END
    ) AS cantidadViajesRealizados,

    -- Cant de choferes distintos que participaron
    COUNT(DISTINCT v.codChoferAsignado) AS cantidadChoferes,

    -- Cantidad de vehículos distintos que participaron
    COUNT(DISTINCT v.patenteVehiculoAsignado) AS cantidadVehiculos

FROM destinos d
    JOIN viajes v ON d.codDestino = v.destino_codDestino

WHERE dbo.fnCategoriaCliente(v.codCliente) <> 'EXPLORADOR'

GROUP BY
    d.codDestino,
    d.descripcion

HAVING COUNT(*) >
(
    SELECT AVG(CantidadReservados * 1.0)--*1 para que el promedio quede float, decimal
    FROM
    (
        --Calculamos cuantos viajes tiene cada chofer en estado RESERVADO, --
		-- para en la consulta externa, calcular el promedio de esas cantidades.
		SELECT
            COUNT(*) AS CantidadReservados
        FROM viajes
        WHERE estado = 'RESERVADO'
        GROUP BY codChoferAsignado
    ) AS PromedioChoferes
);
GO