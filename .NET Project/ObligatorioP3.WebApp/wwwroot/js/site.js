// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Alterna entre el tema claro y el oscuro y recuerda la elección del usuario.
document.addEventListener("DOMContentLoaded", function () {
    const botonTema = document.getElementById("btnTema");
    const iconoTema = document.getElementById("iconoTema");
    const textoTema = document.getElementById("textoTema");

    function aplicarTema(tema) {
        document.documentElement.setAttribute("data-bs-theme", tema);
        localStorage.setItem("tema", tema);

        if (tema === "dark") {
            iconoTema.textContent = "☀️";
            textoTema.textContent = "Modo claro";
            botonTema.setAttribute("aria-label", "Activar modo claro");
        } else {
            iconoTema.textContent = "🌙";
            textoTema.textContent = "Modo oscuro";
            botonTema.setAttribute("aria-label", "Activar modo oscuro");
        }
    }

    // Sin preferencia guardada, se utiliza el modo claro.
    const temaInicial = localStorage.getItem("tema") || "light";
    aplicarTema(temaInicial);

    botonTema.addEventListener("click", function () {
        const temaActual = document.documentElement.getAttribute("data-bs-theme");
        aplicarTema(temaActual === "dark" ? "light" : "dark");
    });
});
