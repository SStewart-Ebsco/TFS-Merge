'use strict';

$(function () {
    registerPostExampleAnimation();
    addMobileAppHandlers();
});

function registerPostExampleAnimation() {
    var post = $('.post'),
        isAnimationActive = false;
    post.on("click", function () {
        if (isAnimationActive) {
            return;
        }
        isAnimationActive = true;
        post.addClass("animated swing");
        post.one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            post.removeClass("animated swing");
            isAnimationActive = false;
        });
    });
}

function addMobileAppHandlers() {
    var buttons = $("#HomeLogo, #DownloadButton");
    buttons.off('click');
    buttons.on("click", function (e) {
        e.preventDefault();
        if (isMobile.Android()) {
            window.location = "https://play.google.com/store/apps/details?id=com.brotherfish";
        } else if (isMobile.iOS()) {
            window.location = "http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8";
        } else {
            console.log("There is no mobile application for you OS");
        };
    });
}

var isMobile = {
    Android: function() {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }

};
