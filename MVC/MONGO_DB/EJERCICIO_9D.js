/* Ejercicio 9.D */

use OUTATIME

db.Encuestas.aggregate([
    {
        // Solo encuestas de oficinas
        $match: {
            tipoEvaluacion: "OFICINA"
        }
    },
    {
        // Agrupar por oficina
        $group: {
            _id: "$oficina.nombre",
            totalEncuestas: { $sum: 1 },
            noRecomendadas: {
                $sum: {
                    $cond: [
                        { $eq: ["$respuestas.recomendacion", false] },
                        1,
                        0
                    ]
                }
            }
        }
    },
    {
        // Calcular porcentaje
        $project: {
            oficina: "$_id",
            totalEncuestas: 1,
            noRecomendadas: 1,
            porcentajeNoRecomendadas: {
                $multiply: [
                    { $divide: ["$noRecomendadas", "$totalEncuestas"] },
                    100
                ]
            }
        }
    },
    {
        // Más del 30%
        $match: {
            porcentajeNoRecomendadas: { $gt: 30 }
        }
    },
    {
        $project: {
            _id: 0
        }
    }
])