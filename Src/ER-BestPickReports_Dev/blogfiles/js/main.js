'use strict';

function subMenuCall(linkside, curItem) {
    var thisUrl = curItem.attr("href");
    $(".nav .active").removeClass("active");
    if (linkside == "inner") {
        $(".submenu-inner.active").removeClass("active");
    } else {
        $("submenu-inner.active").removeClass("active");
        $(".submenu-opener.active").removeClass("active");
    }
    $("#areas .menu ul").stop(true, true).fadeOut(0);
    $("#areas .menu a").removeClass('active');
    var tallestPar = $("#areas .menu").height();
    $("#areas").height(tallestPar + 71);
    $("#areas .container").height(tallestPar);
    $("#areas.sub-menu .container").css({ "background-position": "-5000px 0px" });
    var newHeight = $(thisUrl).height();
    if (linkside == "inner") {
        $(".sub-menu.clicked:not(#zip)").each(function () {
            $(this).animate({ marginTop: -$(this).height() }, 300).addClass("wasclicked");
        });
        if ($(thisUrl).hasClass("clicked")) {
            window.setTimeout(function () {
                $(":not(#zip).clicked").removeClass("clicked");
            }, 300);
            $(".wasclicked").removeClass("wasclicked");
        } else {
            $(thisUrl).addClass("clicked");
            $(thisUrl).animate({ marginTop: 0 }, 300, function () {
                $(".wasclicked").removeClass("wasclicked");
            });
            curItem.addClass("active");
        }
    } else {
        var $categoryPointer = $("#categoryPointer");
        $(".sub-menu.clicked").each(function () {
            $(this).animate({ marginTop: -$(this).height() }, 300).addClass("wasclicked");
        });
        if ($(thisUrl).hasClass("clicked")) {
            window.setTimeout(function () {
                $(".clicked").removeClass("clicked");
            }, 300);
            $(".wasclicked").removeClass("wasclicked");
            $categoryPointer.show();
        } else {
            $(".clicked").removeClass("clicked");
            $(thisUrl).addClass("clicked");
            $(thisUrl).animate({ marginTop: 0 }, 300, function () {
                $(".wasclicked").removeClass("wasclicked");
            });
            curItem.addClass("active");
            $categoryPointer.hide();
        }

    }
}

$(document).ready(function () {
    $(".sub-menu").each(function () {
        $(this).show().height($(this).height() + 7).animate({ marginTop: -$(this).height() }, 0);
    });

    $(".checkbox input").button();

    if ($(".category-posts-header .title").innerHeight() > 45) {
        $(".category-posts-header .title").css({
            "padding-top": 0,
            "padding-bottom": 0
        });
        $(".category-posts-header .counter").css({
            "padding-top": 0,
            "padding-bottom": 0
        });
    }

    $(".btn-close").click(function () {
        $(".overlay, #popup, #popup-reg, #popup-sorry").fadeOut(0, function () { $(this).remove() });
        return false;
    });

    $(".reload").click(function () {
        $("#slider").flexslider(0);
        return false;
    });

    $(".read-more a").click(function () {
        if ($(this).hasClass("active")) {
            $(this).removeClass("active").parent().prev(".hidden").slideUp(300);
        } else {
            $(this).addClass("active").parent().prev(".hidden").slideDown(300);
        }
        return false;
    });

    $(".top-categories ul li").click(function (e) {
        e.preventDefault();
        var selectedItemText = $(this).text().trim();
        $('.choose-category option').filter(function () {
            return ($(this).text() == selectedItemText);
        }).prop('selected', true);
        $(".choose-category").trigger("change");
    });

    $(".view-more-btn, .preview-all").click(function () {
        var img = $(".view-more-btn img");
        if (img.hasClass("open")) {
            img.removeClass("open");
            img.addClass("close");
            $(".contractors ul li").show();
            $(".preview-all").hide();
        } else {
            img.removeClass("close");
            img.addClass("open");
            $(".contractors ul li").each(function (index) {
                if (index > 2) {
                    $(this).css("display", "none");
                }
            });
            $(".preview-all").show();
        }
        return false;
    });

    $("#header, #slider, #content, #footer").click(function () {
        $("#areas.sub-menu.clicked, #zip.sub-menu.clicked, #categories.sub-menu.clicked").each(function () {
            $(this).animate({ marginTop: -$(this).height() }, 100, function () {
                $(".clicked").removeClass("clicked");
            });
        });
        $(".wasclicked").removeClass("wasclicked");
        $("#menu .active").removeClass("active");
    });

    $(".nav a").click(function () {
        if ($("span", this).length) {
            subMenuCall("nav", $(this));
            return false;
        }
    });

    $(".submenu-opener").click(function () {
        subMenuCall("opener", $(this));
        return false;
    });

    if ($('.states').length) {
        $('.states a').click(function () {
            if ($(this).next("ul").length) {
                if ($(this).next().is(":hidden")) {
                    $(this).addClass("active").next("ul").slideDown(400);
                } else {
                    $(this).removeClass("active").next("ul").slideUp(400);
                }
                return false;
            }
        });
    };

    $(".submenu-inner").click(function () {
        subMenuCall("inner", $(this));
        return false;
    });

    var list = [];

    $("#areas .menu>li>a").click(function () {
        var $subMenu = $(this).next('ul');
        if ($subMenu.length) {
            $("#areas .menu ul").stop(true, true).fadeOut(200);
            $("#areas .menu a").removeClass('active');
            $(this).addClass('active');
            $subMenu.hide().stop(true, true).fadeIn(200);
            $("#areas.sub-menu .container").css({ "background-position": "-1000px 0" });
            list = [];
            $subMenu.children("li").children("a").each(function () {
                var val = Number($(this).width());
                list.push(val);
            });

            var list2 = [];
            $("#areas .menu>li").children("a").each(function () {
                var val = Number($(this).width());
                list2.push(val);
            });

            $("#areas.sub-menu .span-div").css({ left: Math.max.apply(Math, list2) + 75 })
            $("#areas.sub-menu ul.menu ul").css({ "left": Math.max.apply(Math, list2) + 110 })
            var tallest = $subMenu.height();
            var tallestPar = $("#areas .menu").height();
            if (tallestPar > tallest) {
                tallest = tallestPar;
            }
            $("#areas").height(tallest + 71);
            $("#areas .container").height(tallest);
            return false;
        }
    });

    $("#areas .menu ul a").click(function () {
        var $subMenu = $(this).next('ul');
        if ($subMenu.length) {
            $("#areas .menu ul ul").stop(true, true).fadeOut(200);
            $("#areas .menu ul>li>a").removeClass('active');
            $(this).addClass('active');
            $subMenu.hide().stop(true, true).fadeIn(200);
            $("#areas.sub-menu .container").css({ "background-position": Math.max.apply(Math, list) + 110 + $(".sub-menu ul.menu").width() + 75 });
            $("#areas .menu ul ul").css({ "left": Math.max.apply(Math, list) + 110 });
            var tallest = $subMenu.height();
            var tallestPar = $("#areas .menu").height();
            var tallestParPar = $("#areas .menu>li>ul:visible").height();
            if (tallestPar > tallest) {
                if (tallestParPar > tallestPar) {
                    tallest = tallestParPar;
                } else {
                    tallest = tallestPar;
                }
            } else {
                if (tallestParPar > tallest) {
                    tallest = tallestParPar;
                }
            }
            $("#areas").height(tallest + 71);
            $("#areas .container").height(tallest);
            return false;
        }
    });

    var email = $("#SubscriberEmail");
    $("#Subscribe").click(function (e) {
        var isValid = email[0].validity.valid,
            isNotEmpty = email.val().length > 0;
        if (isValid) {
            e.preventDefault();
            var responseMessage = $("#subscriptionStatus");
            if (isNotEmpty) {
                var cookie = getCookieValues($.cookie('bprpreferences'));
                var service = new ER_BestPickReports_Dev.BlogService();
                service.NewsletterSubscribe({
                    email: email.val(),
                    areaId: cookie['areaid'],
                    cityId: cookie['cityid']
                }, function (response) {
                    if (!response.success) {
                        responseMessage.addClass('error');
                    }
                    responseMessage.removeClass('error');
                    responseMessage.addClass('success');
                    responseMessage.text(response.message);
                    email.val("");
                });
            } else {
                responseMessage.removeClass('success');
                responseMessage.addClass('error');
                responseMessage.text("Please enter a valid email address.");
            }
        } else {
        }
        ;
    });

    handleLocalBestContractors();

    //Turning on event triggers on UpdatePanel content reload
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        $(".view-more-btn, .preview-all").click(function () {
            var img = $(".view-more-btn img");
            if (img.hasClass("open")) {
                img.removeClass("open");
                img.addClass("close");
                $(".contractors ul li").show();
                $(".preview-all").hide();
            } else {
                img.removeClass("close");
                img.addClass("open");
                $(".contractors ul li").each(function (index) {
                    if (index > 2) {
                        $(this).css("display", "none");
                    }
                });
                $(".preview-all").show();
            }
            return false;
        });

        $(".top-categories ul li").click(function (e) {
            e.preventDefault();
            var selectedItemText = $(this).text().trim();
            $('.choose-category option').filter(function () {
                return ($(this).text() == selectedItemText);
            }).prop('selected', true);
            $(".choose-category").trigger("change");
        });
    });
});
//end Turning on event triggers on UpdatePanel content reload

function readCookie(name) {
    name += "=";

    var cookiesArray = document.cookie.split(';');
    for (var i = 0; i < cookiesArray.length; i++) {
        var cookie = cookiesArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1, cookie.length);
        }
        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length);
        }
    }

    return null;
}

function requestZip() {
    $(".hidden-popup").show();
}

function clickEnterButton(e, btnId) {
    var evnt = e ? e : window.event;
    var btn = document.getElementById(btnId);
    if (btn) {
        if (evnt.keyCode == 13) {
            btn.click();
            return false;
        }
    }
}

function notFound(zipCode) {
    console.log('ZIP code to set: ' + zipCode);
    $("#popup-sorry-zipcode").text(zipCode);
    $("#PopupSorryZipcode").val(zipCode);

    $(".hidden-popup2").show();
}

function redirect_to(url) {
    setTimeout(function() {
        window.location.href = url;
    }, 0);
}

function getCookieValues(cookie) {
	if (!cookie) {
		return;
	};
	cookie = cookie.split('&');
	var cookieObject = {};
	for (var i = cookie.length - 1; i >= 0; i--) {
		var keyValue = cookie[i].split('=');
		if (keyValue.length === 2) {
			cookieObject[keyValue[0]] = keyValue[1];
		};
	};
	return cookieObject;
}


function handleLocalBestContractors() {
    var catlink = $('.post .catlink'),
        //postYear = $('.postYear').val(),
    		localBest = $('.post-local-best'),
    		localBestText = $('.post-local-best-text');
    if (catlink && localBestText && localBest) {
        //localBest.css('background', "url('/blogfiles/images/post/reward_" + postYear + ".png') no-repeat");
        localBestText.append(catlink);
        localBest.show();
    };
}
