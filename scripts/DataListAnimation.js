$(document).ready(function () {
    $(".maps-template").css("opacity", "0");
    $(".maps-template").each(function (index) {
        $(this).css("animation", "slideIn 1s ease " + (index * 0.15) + "s forwards");
        /*$(this).delay(250 * index).animate({ opacity: 1 }, 1000);*/
    });
});