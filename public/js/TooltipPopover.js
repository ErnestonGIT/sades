$(document).ready(function () {

    infoToolStart();
});

function infoToolStart() {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
    const popoverList = [...popoverTriggerList].map(popoverTriggerEl => {
        const popover = new bootstrap.Popover(popoverTriggerEl);

        // Agregar un evento click al botón para cerrar el popover manualmente
        popoverTriggerEl.addEventListener('click', () => {
            popover.hide();
        });

        return popover;
    });
}  