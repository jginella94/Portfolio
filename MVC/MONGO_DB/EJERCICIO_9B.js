/* Ejercicio 9.B */

use OUTATIME

db.Encuestas.aggregate([
    {
        //Filtramos 
        $match: {
            tipoEvaluacion: "OFICINA"
        }
    },
    {
        //Agrupamos por oficna
        $group: {
            _id: "$oficina.nombre",
            promedioAtencion: {
                $avg: "$respuestas.atencion"
            }
        }
    },
    {
        //Ordenamos de mayor a menor
        $sort: {
            promedioAtencion: -1
        }
    }
])