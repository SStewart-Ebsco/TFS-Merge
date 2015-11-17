var fancyboxVideoSettings = {
	maxWidth: 800,
	maxHeight: 600,
	fitToView: false,
	width: '70%',
	height: '70%',
	autoSize: false,
	closeClick: false,
	openEffect: 'none',
	closeEffect: 'none'
};

function subMenuCall(linkside, curItem) {
    var thisUrl = curItem.attr("href");
    $(".nav .active").removeClass("active");
    if (linkside == "inner") {
        $(".submenu-inner.active").removeClass("active");
    } else {
        $(".submenu-inner.active").removeClass("active");
        $(".submenu-opener.active").removeClass("active");
    }
    $("#areas .menu ul").stop(true, true).fadeOut(0);
    $("#areas .menu a").removeClass('active');
    var tallestPar = $("#areas .menu").height();
    $("#areas").height(tallestPar + 71);
    $("#areas .container").height(tallestPar)
    $("#areas.sub-menu .container").css({ "background-position": "-5000px 0px" })
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
	initializeTestimonialSlider();

	if (readCookie("bprpreferences") != null) {
		loadAvailableAreaSkylineImage();
    }

    // Using jQuery Event API v1.3
    $('#MainContent_EmailOK').on('click', function () {
        ga('send', 'event', 'button', 'click', 'webleadSubmit');
    });

	$(".sub-menu").each(function () {
		$(this).show().height($(this).height() + 7).animate({ marginTop: -$(this).height() }, 0);
	});

	$(".videoButton").fancybox(fancyboxVideoSettings);

	$(".checkbox input").button();

	$(".btn-close").click(function () {
		$(".overlay, #popup, #popup-reg, #popup-sorry").fadeOut(0, function () { $(this).remove() })
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

	$("#header, #slider, #content, #footer").click(function () {
		$("#areas.sub-menu.clicked, #about.sub-menu.clicked, #more.sub-menu.clicked, #zip.sub-menu.clicked, #categories.sub-menu.clicked").each(function () {
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

	/*$(".filter-tools select").selectmenu();*/
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

	$("#areas .menu>li>a").click(function () {
		var $subMenu = $(this).next('ul');
		if ($subMenu.length) {
			$("#areas .menu ul").stop(true, true).fadeOut(200);
			$("#areas .menu a").removeClass('active');
			$(this).addClass('active');
			$subMenu.hide().stop(true, true).fadeIn(200);
			$("#areas.sub-menu .container").css({ "background-position": "-1000px 0" })
			list = []
			$subMenu.children("li").children("a").each(function () {
				val = Number($(this).width());
				list.push(val);
			})

			list2 = []
			$("#areas .menu>li").children("a").each(function () {
				val = Number($(this).width());
				list2.push(val);
			})

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
			$("#areas.sub-menu .container").css({ "background-position": Math.max.apply(Math, list) + 110 + $(".sub-menu ul.menu").width() + 75 })
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

});

function initializeTestimonialSlider() {
	if ($('.testimonials .slides').length) {
		$('.testimonials .slides').carouFredSel({
			direction: "up",
			height: "variable",
			items: {
				visible: 1,
				height: "variable"
			},
			scroll: {
				items: 1,

				fx: "directscroll"
			},
			auto: 10000
		});
	};
}

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

function loadAvailableAreaSkylineImage() {
	var imageName = $("#hdnAreaSkylineImageFileName").val();
	if (imageName !== "") {
		$("#header").css("background-image", "url(/images/bg_body/" + imageName + ")");
	}
}

$(window).load(function () {
	initializeSlider();	
});

function initializeSlider() {
	var initSlideDelay = 7000;
	var slideShowSpeed = 8000;

	$.when($("#slider").flexslider({
		controlNav: false,
		touch: false,
		initDelay: initSlideDelay,
		slideshowSpeed: slideShowSpeed
	}))
	.done(function () {
		wrapFirstSlideWithVideoLink(initSlideDelay + slideShowSpeed);
	});
}

function requestZip() {
    $(".hidden-popup").show();
}

function notFound() {
    $(".hidden-popup2").show();
}

function wrapFirstSlideWithVideoLink(msec) {
	/// <summary>Wraps the first slide with the link of video button</summary>
	/// <param name="msec" type="Number">timeout in msec when the link is unwrapped</param>
	var $slider = $("#slider");
	var $firstSlide = $slider.find(".item:first-child");
	var href = $slider.find(".videoButton").attr("href");
	$firstSlide.wrap("<a class='fancybox.iframe' href='" + href + "'></a>");
	$firstSlide.parent().fancybox(fancyboxVideoSettings);
}
