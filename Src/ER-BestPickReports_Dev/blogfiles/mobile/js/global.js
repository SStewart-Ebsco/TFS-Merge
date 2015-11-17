'use strict';

$(function () {
    var service = new ER_BestPickReports_Dev.BlogService();

    enableMenu();
    enableSingleEmailButtons();
    enableContractorsList();
    enableEmailForm(service);
    enableRequestTopicForm();
    enableSuccessModals();
    enableMenuButtons(service);
    enableFooter();
    enableOutOfMarketModal();

    $('#SearchBtn').click(function (e) {
        e.preventDefault();
    });
});

function enableMenu() {
	var menuItems = $('#menu>.menu-item');
	$.each(menuItems, function(index, item) {
		var label = $(item.children[0]);
		label.click(function(e) {
			$(item).toggleClass('selected');
		});
	});

	var menuButton = $('#menu-button'),
		menu = $('#menu');
	menuButton.click(function(event){
		event.preventDefault();
		menu.toggleClass('menu-hidden');
		menuButton.toggleClass('pressed');
	});


	var zipCodeChange = $('#ZipCodeChange'),
		zipCodeChangeButton = $('#ZipCodeChangeButton');
	zipCodeChange.on('keypress', function(e){
		var code = e.keyCode || e.which; 
		if (code == 13) {
			e.preventDefault();
			zipCodeChangeButton.click();
		};
	});
}

function enableSingleEmailButtons() {
	// SingleEmail_CLick
	$('#menu').on('click', '.company-email-link', function(event) {
	    var company = {
	        name: $('.company-name', this.parentNode).text(),
	        email: $('.company-email', this.parentNode).val(),
	        cid: $('.company-cid', this.parentNode).val(),
	        isMultiple: false
	    };
		showEmailform(company);
	});
}

function enableContractorsList() {
	// BG, Close button - close
	$("#menu").on('click', '#ChooseEmailPanel .modal-win-close, #ChooseEmailPanel .modal-win-bg', function(e){
		var contractorsModal = $('#ChooseEmailPanel');
		contractorsModal.hide();
	});

	// SAVE TIME button - open win
	$("#menu").on('click', '.savetime-background', function(e){
		var contractorsModal = $('#ChooseEmailPanel');
		contractorsModal.show();
	});

	// Show More Button
	$("#menu").on('click', '.category-more', function(e){
		e.preventDefault();
		var contractors = $(this).parent().find('ul.companies>li');
		if (contractors.length > 3){
			var isCollapsed = contractors[3].style['display'] === 'none',
				newLineStyle = isCollapsed ? 'block' : 'none';
			for (var i = contractors.length - 1; i >= 3; i--) {
				contractors[i].style['display'] = newLineStyle;
			};
		}
	});

	// Continue button - Save Selected Contractors and Open Email Form
	// server desktop: ContinueButton_Click
	$("#menu").on('click', '#ChooseEmailPanel #ContractorsModalContinue', function (e) {
	    e.preventDefault();
	    var contractorsModal = $('#ChooseEmailPanel');
	    // get selected IDs/names
	    var checkboxes = $('ul.modal-win-list input[type=checkbox]', contractorsModal),
			emailIds = [],
			emailFormTitle = null,
			contractorName = null; // used only if one checkbox checked
	    checkboxes.each(function (index, checkbox) {
	        if (checkbox.checked) {
	            var emailId = $(checkbox).parent().parent().find('input[type=hidden]').val();
	            contractorName = $(checkbox).parent().parent().find('.js-contractor-label').text()
	            emailIds.push(emailId);
	        };
	    });

	    if (emailIds.length > 0) {
	        var company = {
	            name: emailIds.length === 1 ? contractorName : 'Multiple Companies',
	            multipleEmailIds: emailIds.join(','),
	            isMultiple: true
	        };
	        contractorsModal.hide();
	        showEmailform(company);
	    };

	});
}

function enableEmailForm(service) {
	var emailform = $('#ModalEmailForm'),
		companies = $('.company'),
		cancelButton = $('#cancelButton'),
		submitButton = $('#SubmitEmailformButton_DISABLED'),
		successEmailModal = $('#SuccessEmailModal');

	cancelButton.click(function(event){
		event.preventDefault();
		$('body').toggleClass('on-modal');
		emailform.hide();
	});
	submitButton.click(function(event){
		// TODO add submit actions
		var validator = emailform.validate();
		if (validator.valid()) {
			//$('body').toggleClass('on-modal');
			//emailform.hide();
			//successEmailModal.show();
			// TODO sendEmailRequest

			/*
			var data = emailform.serializeArray().reduce(function(obj, item) {
			    obj[item.name] = item.value;
			    return obj;
			}, {});;
			data.contractorEmails = ['a', 'b'];
			service.DoWork(data, function(response){
				debugger;
			});
			*/
		} else {
			event.preventDefault();
		};

	});

	emailform.on('keypress', function(e){
		var code = e.keyCode || e.which,
			type = e.target.type; 
			if (code == 13 && type !== "textarea") {
				e.preventDefault();
				submitButton.click();
			};
	});
}

function enableRequestTopicForm() {

    var emailform = $('#RequesTopicEmailForm'),
		cancelButton = $('#EmailCancel'),
		submitButton = $('#ReqEmailOK'),
		successEmailModal = $('#EmailRequestComplete'),
        requestBtn = $('#RequestTopic');

   	requestBtn.off('click');
    requestBtn.click(function (event) {
        event.preventDefault();
        showRequestTopicEmailForm();
        $('body').toggleClass('on-modal');
    });

    cancelButton.click(function (event) {
        event.preventDefault();
        emailform.hide();
        $('body').toggleClass('on-modal');
    });

    emailform.on('keypress', function (e) {
        var code = e.keyCode || e.which,
			type = e.target.type;
        if (code == 13 && type !== "textarea") {
            e.preventDefault();
            submitButton.click();
        };
    });

	if (successEmailModal) {
		var closeControls = successEmailModal.find('.modal-win-close, .modal-win-bg, .modal-button');
		$.each(closeControls, function(index, control){
			$(control).click(function(e){
				e.preventDefault();
				successEmailModal.hide();
				//$('body').toggleClass('on-modal');
			});
		});
	};

}

function enableSuccessModals() {
	var successEmailModal = $('#SuccessEmailModal'),
		successModal = $('#EmailComplete');

	if (successEmailModal) {
		var closeControls = successEmailModal.find('.modal-win-close, .modal-win-bg, .modal-button');
		$.each(closeControls, function(index, control){
			$(control).click(function(e){
				e.preventDefault();
				successEmailModal.hide();
				//$('body').toggleClass('on-modal');
			});
		});
	};

	if (successModal) {
		var closeControls = successModal.find('.modal-win-close, .modal-win-bg, .modal-button');
		$.each(closeControls, function(index, control){
			$(control).click(function(e){
				e.preventDefault();
				successModal.hide();
				//$('body').toggleClass('on-modal');
			});
		});
	};
}

function showEmailform(company) {
	if (company === null || company === undefined) {
		return;
	};
	var emailform = $('#ModalEmailForm'),
		emailformCompanyName = $('.emailform-header-company'),
		companyNameHidden = $('#EmailFormContractorName');

	// set selected Contractor Info
	// this info used in OK_EmailClick
	if (company.isMultiple) {
		$('#MultipleEmailIDs').val(company.multipleEmailIds);
		$('#EmailContractorID').val('');
		$('#ContractorEmail').val('');
	} else {
		$('#EmailContractorID').val(company.cid);
	    $('#ContractorEmail').val(company.email);
		$('#MultipleEmailIDs').val('');
	};
	var inputs = $('input[type=text], input[type=phone], input[type=email], textarea', emailform);
	$.each(inputs, function(index, input){
		input.value = "";
	});
	emailform.show();
	$('body').toggleClass('on-modal');
	
	companyNameHidden.val(company.name);
	emailformCompanyName.text(company.name);
}

function showRequestTopicEmailForm() {
    var emailform = $('#RequesTopicEmailForm');

    var inputs = $('input[type=text], input[type=email], textarea', emailform);
    $.each(inputs, function (index, input) {
        input.value = "";
    });
    emailform.show();
}

function enableMenuButtons(service) {
	var subscribeButton = $('#subscribe-button'),
		subscribeEmail = $('#subscribe-email'),
		successModal = $('#EmailRequestComplete'),
		responseMessage = successModal.find('.modal-win-text'),
		responseHeader = successModal.find('.modal-win-header');

	var changeZipButton = $('#ZipCodeChangeButton'),
		changeZipField = $('#ZipCodeChange');

	subscribeButton.click(function(e){
		var isValid = subscribeEmail[0].validity.valid,
			isNotEmpty = subscribeEmail.val().length > 0;
		if (isValid) {
			e.preventDefault();
			if (isNotEmpty) {
			    var cookie = getCookieValues($.cookie('bprpreferences'));
			    var areaId = 0;
                var cityId = 0;
			    if (cookie) {
			        areaId = cookie['areaid'];
			        cityId = cookie['cityid'];
			    }
				service.NewsletterSubscribe({
					email: subscribeEmail.val(),
					areaId: areaId,
					cityId: cityId
				}, function(response){
					if (!response.success) {
						responseHeader.text('Error');
					};
					responseMessage.text(response.message);
					successModal.show();
				});
			};
		} else {
		};
	});


	changeZipButton.click(function(el){
		var zipExp = /^[0-9]{5}$/;
		var isValid = zipExp.test(changeZipField[0].value);
		if (!isValid) {
			e.preventDefault();
		};
	});
}

function enableOutOfMarketModal() {
    $("#MsgPopup .modal-win-close").on('click', hideNotFoundZipModal);
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

function enableFooter() {
	var fullLink = $('#viewFullSiteLink');

	if (fullLink) {
		fullLink.click(function(e){
			e.preventDefault();
			$.cookie('nomobile', 'true', { expires: 1, path: "/blog" });
			location.reload();
		});
	};
}

//Turning on event triggers on UpdatePanel content reload
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_endRequest(function() {
});

function showNotFoundZipModal() {
    var notFoundZipModal = $('#MsgPopup');
    $('body').toggleClass('on-popup');
    notFoundZipModal.show();
}

function hideNotFoundZipModal() {
    var notFoundZipModal = $('#MsgPopup');
    $('body').toggleClass('on-popup');
    notFoundZipModal.hide();
}