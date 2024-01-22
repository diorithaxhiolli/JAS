document.addEventListener("DOMContentLoaded", function () {
    var navbar = document.getElementById("navbar");
    var targetElement = document.getElementById("navbar");

    function getNavbarHeight() {
        return navbar.offsetHeight;
    }

    window.addEventListener("scroll", function () {
        var navbarHeight = getNavbarHeight();

        if (window.scrollY > navbarHeight) {
            targetElement.classList.add("fixed", "top-0", "w-full");
            document.body.style.paddingTop = navbarHeight + "px";
        } else {
            targetElement.classList.remove("fixed", "top-0", "w-full");
            document.body.style.paddingTop = "0";
        }
    });
});