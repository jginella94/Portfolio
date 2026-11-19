USE OUTATIME_INC
GO

-- Ejercicio 3 - (A) --    

SELECT TOP 3 *
FROM
-- TOTAL AÑOS VIAJADOS ACUMULADOS POR CADA CHOFER (AniosPorChofer) --
(SELECT
        c.codChofer,
        c.nombre,
        c.apellido,
        SUM(
            ABS(DATEDIFF(YEAR,
                         v.fechaHora_salida,
                         d.fecha_hora))
        ) AS TotalAnios
    FROM choferes c
        JOIN viajes v
            ON v.codChoferAsignado = c.codChofer
        JOIN destinos d
            ON d.codDestino = v.destino_codDestino
    WHERE v.estado = 'REALIZADO'
    GROUP BY
        c.codChofer,
        c.nombre,
        c.apellido
) AS AniosPorChofer

WHERE
    TotalAnios
-- Mayor que --

>

-- PROMEDIO DE AÑOS VIAJADOS ACUMULADOS POR CHOFER --

(SELECT AVG(CAST(TotalAnios AS DECIMAL(10,2))) AS Promedio
    FROM 
-- TOTAL AÑOS VIAJADOS POR CADA CHOFER (AniosPorChofer)
    (SELECT
        c.codChofer,
        c.nombre,
        c.apellido,
        SUM(
            ABS(DATEDIFF(YEAR,
                         v.fechaHora_salida,
                         d.fecha_hora))
        ) AS TotalAnios

    FROM choferes c
        JOIN viajes v
            ON v.codChoferAsignado = c.codChofer
        JOIN destinos d
            ON d.codDestino = v.destino_codDestino
    WHERE v.estado = 'REALIZADO'
    GROUP BY
        c.codChofer,
        c.nombre,
        c.apellido
) AS AniosPorChofer)
--LOS 3 CHOFERES ORDENADOS DESCENDIENTE--
ORDER BY TotalAnios DESC;


-- Ejercicio 3 - (B) --    

SELECT TOP 4 v.patente, SUM (mv.consumoGWViaje) AS consumoAcumuladoViajes
	FROM vehiculos v
		JOIN modeloVehiculo mv ON v.codModelo = mv.codModelo
		JOIN viajes vi ON v.patente = vi.patenteVehiculoAsignado
	WHERE vi.estado = 'REALIZADO'
	GROUP BY v.patente
	ORDER BY consumoAcumuladoViajes DESC


	-- Ejercicio 3 - (C) --  

SELECT TOP 1 d.codDestino, d.descripcion, COUNT(v.codViaje) AS CantidadViajes
	FROM destinos d
		JOIN viajes v ON d.codDestino =v.destino_codDestino
		JOIN pagos p ON v.codViaje = p.codViaje
	WHERE p.metodo = 'EFECTIVO'
	GROUP BY d.codDestino, d.descripcion
	ORDER BY CantidadViajes DESC
	
	-- Ejercicio 3 - (D) -- 

SELECT c.nombre,
	   c.apellido, 
	   (
			SELECT COUNT (DISTINCT vi.codViaje)
				FROM pagos p
					JOIN viajes vi ON p.codViaje = vi.codViaje
				WHERE vi.codCliente = c.codCliente
				AND p.metodo IN ('CREDITO', 'DEBITO')
		) AS CantidadViajesPagadosConTarjeta,
		(	
			SELECT COUNT (vi.codViaje)
				FROM viajes vi
				WHERE vi.codCliente = c.codCliente
				AND vi.estado IN ('CANCELADO', 'SUSPENDIDO')
				) AS cantViajesNoRealizados
FROM clientes c
			

            -- Ejercicio 3 - (E) --

  SELECT 
    v.patente,
    SUM(
        CASE 
            -- Si el viaje ya se realizó o está en curso, calculamos el salto temporal absoluto al destino
            WHEN vi.estado IN ('REALIZADO', 'EN VIAJE') 
                 THEN ABS(DATEDIFF(year, vi.fechaHora_salida, d.fecha_hora))
            ELSE 0
        END
    ) AS AniosAcumulados
FROM 
    vehiculos v
INNER JOIN 
    modeloVehiculo m ON v.codModelo = m.codModelo
INNER JOIN 
    viajes vi ON v.patente = vi.patenteVehiculoAsignado
INNER JOIN
    destinos d ON vi.destino_codDestino = d.codDestino
WHERE 
    m.realizaVuelo = 1
    AND vi.estado IN ('REALIZADO', 'EN VIAJE')
GROUP BY 
    v.patente
HAVING 
    SUM(
        CASE 
            WHEN vi.estado IN ('REALIZADO', 'EN VIAJE') 
                 THEN ABS(DATEDIFF(year, vi.fechaHora_salida, d.fecha_hora))
            ELSE 0
        END
    ) > 457;

    -- Ejercicio 3 - (F) --

    SELECT 
    o.codOficina,
    o.descripcion AS Oficina,
    SUM(v.costo) AS TotalDineroGanado
FROM 
    oficinas o
INNER JOIN 
    viajes v ON o.codOficina = v.codOficina
WHERE 
    o.codOficina NOT IN (
        SELECT DISTINCT codOficina 
        FROM viajes 
        WHERE estado IN ('CANCELADO', 'SUSPENDIDO')
    )
GROUP BY 
    o.codOficina, 
    o.descripcion;

    -- Ejercicio 3 - (G) --
    SELECT 
    c.codChofer,
    c.nombre,
    c.apellido,
    p.descripcion AS PaisOrigen
FROM 
    choferes c
INNER JOIN 
    paises p ON c.codPais = p.codPais
WHERE 
    c.codChofer NOT IN (
        SELECT DISTINCT codChoferAsignado 
        FROM viajes 
        WHERE estado = 'REALIZADO'
    )
    AND c.codPais IN (
        SELECT DISTINCT o.codPais
        FROM oficinas o
        INNER JOIN viajes v ON o.codOficina = v.codOficina
        WHERE v.estado = 'RESERVADO'
    );

    -- Ejercicio 3 - (H) --

    SELECT 
    m.tipoCombustible,
    SUM(v.costo) AS TotalIngresos,
    SUM(m.consumoGWViaje) AS TotalGWConsumidos,
    CAST(SUM(v.costo) / SUM(m.consumoGWViaje) AS DECIMAL(10,2)) AS RendimientoEconomico
FROM 
    viajes v
INNER JOIN 
    vehiculos veh ON v.patenteVehiculoAsignado = veh.patente
INNER JOIN 
    modeloVehiculo m ON veh.codModelo = m.codModelo
WHERE 
    v.estado = 'REALIZADO'
GROUP BY 
    m.tipoCombustible
ORDER BY 
    RendimientoEconomico DESC;