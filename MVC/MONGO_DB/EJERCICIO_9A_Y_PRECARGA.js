/* Ejercicio 9.A */

use OUTATIME

/* Reiniciar colección */
/* Faltaron datos en la precarga inicial, para poder 
evaluar todas las consultas.*/

db.Encuestas.drop()

db.createCollection("Encuestas")

/* ========================= */
/* OFICINA - Montevideo      */
/* ========================= */

db.Encuestas.insertOne({
    pais: "Uruguay",
    tipoEvaluacion: "OFICINA",
    fecha: "2026-06-21",
    oficina: {
        codigo: 1,
        nombre: "Sucursal Montevideo"
    },
    respuestas: {
        atencion: 5,
        tiempoEspera: 4,
        instalaciones: 5,
        recomendacion: true
    }
})

db.Encuestas.insertOne({
    pais: "Uruguay",
    tipoEvaluacion: "OFICINA",
    fecha: "2026-06-22",
    oficina: {
        codigo: 1,
        nombre: "Sucursal Montevideo"
    },
    respuestas: {
        atencion: 4,
        tiempoEspera: 3,
        instalaciones: 4,
        recomendacion: false
    }
})

/* ========================= */
/* OFICINA - Buenos Aires    */
/* ========================= */

db.Encuestas.insertOne({
    pais: "Argentina",
    tipoEvaluacion: "OFICINA",
    fecha: "2026-06-21",
    oficina: {
        codigo: 2,
        nombre: "Sucursal Buenos Aires"
    },
    respuestas: {
        atencion: 2,
        tiempoEspera: 2,
        instalaciones: 3,
        recomendacion: false
    }
})

db.Encuestas.insertOne({
    pais: "Argentina",
    tipoEvaluacion: "OFICINA",
    fecha: "2026-06-22",
    oficina: {
        codigo: 2,
        nombre: "Sucursal Buenos Aires"
    },
    respuestas: {
        atencion: 3,
        tiempoEspera: 2,
        instalaciones: 3,
        recomendacion: false
    }
})

/* ========================= */
/* CHOFERES                  */
/* ========================= */

db.Encuestas.insertOne({
    pais: "Estados Unidos",
    tipoEvaluacion: "CHOFER",
    fecha: "2026-06-21",
    chofer: {
        codigo: 10,
        nombre: "Marty McFly"
    },
    respuestas: {
        puntualidad: 5,
        amabilidad: 4,
        conduccionSegura: 5,
        conocimientoDestino: 5
    }
})

db.Encuestas.insertOne({
    pais: "Estados Unidos",
    tipoEvaluacion: "CHOFER",
    fecha: "2026-06-22",
    chofer: {
        codigo: 11,
        nombre: "Doc Brown"
    },
    respuestas: {
        puntualidad: 4,
        amabilidad: 5,
        conduccionSegura: 5,
        conocimientoDestino: 5
    }
})

/* ========================= */
/* VEHICULOS                 */
/* ========================= */

db.Encuestas.insertOne({
    pais: "Estados Unidos",
    tipoEvaluacion: "VEHICULO",
    fecha: "2026-06-21",
    vehiculo: {
        matricula: "DMC-88",
        modelo: "DeLorean"
    },
    respuestas: {
        limpieza: 2,
        estadoMecanico: 5,
        comodidad: 4,
        seguridad: 5
    }
})

db.Encuestas.insertOne({
    pais: "Estados Unidos",
    tipoEvaluacion: "VEHICULO",
    fecha: "2026-06-22",
    vehiculo: {
        matricula: "DMC-88",
        modelo: "DeLorean"
    },
    respuestas: {
        limpieza: 1,
        estadoMecanico: 5,
        comodidad: 4,
        seguridad: 5
    }
})

db.Encuestas.insertOne({
    pais: "Japón",
    tipoEvaluacion: "VEHICULO",
    fecha: "2026-06-22",
    vehiculo: {
        matricula: "TT-2026",
        modelo: "Time Train"
    },
    respuestas: {
        limpieza: 5,
        estadoMecanico: 4,
        comodidad: 4,
        seguridad: 5
    }
})

/* Verificación */
db.Encuestas.countDocuments()