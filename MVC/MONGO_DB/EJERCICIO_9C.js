/* Ejercicio 9.C */

use OUTATIME

db.Encuestas.aggregate([
    {
        // Filtramos las encuestas de vehículos
        $match: {
            tipoEvaluacion: "VEHICULO"
        }
    },
    {
        // Agrupamos por vehículo
        $group: {
            _id: "$vehiculo.modelo",
            promedioLimpieza: {
                $avg: "$respuestas.limpieza"
            }
        }
    },
    {
        // Solo vehículos con promedio menor a 3
        $match: {
            promedioLimpieza: {
                $lt: 3
            }
        }
    },
    {
        // De peor a mejor
        $sort: {
            promedioLimpieza: 1
        }
    }
])