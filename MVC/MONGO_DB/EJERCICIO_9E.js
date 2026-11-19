/* Ejercicio 9.E */

use OUTATIME

db.Encuestas.aggregate([
    {
        // Agrupar por país
        $group: {
            _id: "$pais",
            cantidadEncuestas: {
                $sum: 1
            }
        }
    },
    {
        // Ordenar de mayor a menor
        $sort: {
            cantidadEncuestas: -1
        }
    }
])