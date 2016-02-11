//<!-- MOBILE NAVIGATION -->
var showRight_1 = document.getElementById("showRight_1"),
showRight_2 = document.getElementById("showRight_2"),
hideRight = document.getElementById('hideRight'),
menuRight = document.getElementById('cbp-spmenu-s2'),
body = document.body;

showRight_1.onclick = function (event) {
    "use strict";
    event.preventDefault();
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    disableOther('showRight_1');
    return false;
};

showRight_2.onclick = function (event) {
    "use strict";
    event.preventDefault();
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    disableOther('showRight_2');
    return false;
};

hideRight.onclick = function (event) {
    "use strict";
    event.preventDefault();
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    disableOther('showRight_2');
    return false;
};


//<!-- Nav Bar show/hide after scrolling -->
var $head = $('#menu_bar');
$('.menu_bar-waypoint').each(function (i) {
    var $el = $(this),
        animClassDown = $el.data('animateDown'),
        animClassUp = $el.data('animateUp');


    $el.waypoint(function (direction) {
        if (direction === 'down' && animClassDown) {
            $head.attr('class', 'menu_bar ' + animClassDown);
        }
        else if (direction === 'up' && animClassUp) {
            $head.attr('class', 'menu_bar ' + animClassUp);
        }
    }, {
        offset: function () {
            navbarheight = $("#menu_bar").outerHeight() + 1;
            return navbarheight;
        }
    });
});
