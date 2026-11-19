// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Preferencia visual local: no depende de sesión ni de la API.
document.addEventListener("DOMContentLoaded", function () {
    const boton = document.getElementById("btnTema");
    const icono = document.getElementById("iconoTema");
    const texto = document.getElementById("textoTema");

    if (!boton || !icono || !texto) return;

    function actualizarBoton(tema) {
        const esOscuro = tema === "dark";
        icono.textContent = esOscuro ? "☀️" : "🌙";
        texto.textContent = esOscuro ? "Modo claro" : "Modo oscuro";
        boton.setAttribute("aria-label", esOscuro ? "Activar modo claro" : "Activar modo oscuro");
    }

    actualizarBoton(document.documentElement.getAttribute("data-bs-theme"));

    boton.addEventListener("click", function () {
        const actual = document.documentElement.getAttribute("data-bs-theme");
        const siguiente = actual === "dark" ? "light" : "dark";
        document.documentElement.setAttribute("data-bs-theme", siguiente);
        localStorage.setItem("tema", siguiente);
        actualizarBoton(siguiente);
    });
});
