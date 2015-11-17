'use strict';

$(function() {
    var service = new ER_BestPickReports_Dev.BlogService();

    var ZIP_CODE_CHANGE_BUTTON_2_ID = '#ZipCodeChangeButton2';
    var ZIP_CODE_CHANGE_2 = '#ZipCodeChange2';
    var CHANGE_ZIP_POPUP = '#ChangeZipPopup';
    var ZIP_REQUEST_TYPES = {
        SEARCH: '1',
        CATEGORY: '2'
    };

    enableMenu();
    enableEmailForm(service);
    enableRequestTopicForm(service);
    enableModals();
    //enableMenuButtons(service);
    enableFooter();

    enableContractorsList();
    enableSingleEmailButtons();
    enableNewsletterSignUpForm(service);

    $('#SearchBtn').click(function(e) {
        var preferences = getCookieValues($.cookie("bprpreferences"));
        if (preferences && preferences.areaid && preferences.cityid) {
            // submiting..
        } else {
            e.preventDefault();
            $(CHANGE_ZIP_POPUP).show();
            $('body').toggleClass('on-popup');
            setZipRequestType(ZIP_REQUEST_TYPES.SEARCH);
        }
    });

    $('#go-to-post-btn').on('click', function() {
        window.location = '/blog?redirect=false';
    });

    $(ZIP_CODE_CHANGE_BUTTON_2_ID).on('click', function(e) {
        e.preventDefault();

        var zipCode = $(ZIP_CODE_CHANGE_2).val().trim();
        service.ChangeZipCoded(zipCode, function(response) {
            if (response.success) {
                var requestType = getZipRequestType();
                switch (requestType) {
                case ZIP_REQUEST_TYPES.SEARCH:
                    $('#SearchBtn').click();
                    break;
                case ZIP_REQUEST_TYPES.CATEGORY:
                    window.location = "/categories";
                    break;
                }
            } else {
                $(CHANGE_ZIP_POPUP).hide();
                showNotFoundZipModal();
            }
        });
    });

    function setZipRequestType(value) {
        $(ZIP_CODE_CHANGE_BUTTON_2_ID).attr('data-action-id', value);
    }

    function getZipRequestType() {
        return $(ZIP_CODE_CHANGE_BUTTON_2_ID).attr('data-action-id');
    }

    function enableMenu() {
        var menuItems = $('#menu>.menu-item.menu-item-submenu');
        //TODO filter menu items without sub-section
        $.each(menuItems, function(index, item) {
            var label = $(item.children[0]);
            label.click(function(e) {
                $(item).toggleClass('selected');
            });
        });

        var menuButton = $('#menu-button'),
            menu = $('#menu');
        menuButton.click(function(event) {
            event.preventDefault();
            menu.toggleClass('menu-hidden');
            menuButton.toggleClass('pressed');
        });

        var categoryLink = $("#menu-caterory-link, #home-category-link"),
            changeZipPopup = $(CHANGE_ZIP_POPUP);
        categoryLink.off("click");
        categoryLink.click(function(e) {
            e.preventDefault();
            var preferences = getCookieValues($.cookie("bprpreferences"));
            if (preferences && preferences.areaid && preferences.cityid) {
                window.location = "/categories";
            } else {
                changeZipPopup.show();
                $('body').toggleClass('on-popup');
                setZipRequestType(ZIP_REQUEST_TYPES.CATEGORY);
            }
        });

        var zipCodeChange = $('#ZipCodeChange'),
            zipCodeChangeButton = $('#ZipCodeChangeButton');
        zipCodeChange.on('keypress', function(e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                e.preventDefault();
                zipCodeChangeButton.click();
            };
        });

        var zipCodeChange2 = $(ZIP_CODE_CHANGE_2),
            zipCodeChangeButton2 = $(ZIP_CODE_CHANGE_BUTTON_2_ID);
        zipCodeChange2.on('keypress', function(e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                e.preventDefault();
                zipCodeChangeButton2.click();
            };
        });
    }

    function enableSingleEmailButtons() {
        // SingleEmail_CLick
        $(document).on('click', '.contractor-email-link', function(event) {
            var companyItem = $(this).closest('.contractor-item');
            var company = {
                name: $('.contractor-name', companyItem).text(),
                email: $('.contractor-email', companyItem).val(),
                cid: $('.contractor-cid', companyItem).val(),
                isMultiple: false
            };
            showEmailform(company);
        });
    }

    function enableContractorsList() {
        // BG, Close button - close
        $('#ModalWindows').on('click', '#ChooseEmailPanel .modal-win-close, #ChooseEmailPanel .modal-win-bg', function(e) {
            var contractorsModal = $('#ChooseEmailPanel');
            contractorsModal.hide();
            $('body').toggleClass('on-popup');
        });

        // SAVE TIME button - open win
        $(document).on('click', '.savetime-background', function(e) {
            var contractorsModal = $('#ChooseEmailPanel');
            $('#ChooseEmailPanel #errornote').hide();
            $('body').toggleClass('on-popup');
            contractorsModal.show();
        });

        // Continue button - Save Selected Contractors and Open Email Form
        // server desktop: ContinueButton_Click
        $('#ModalWindows').on('click', '#ChooseEmailPanel #ContractorsModalContinue', function(e) {
            e.preventDefault();
            var contractorsModal = $('#ChooseEmailPanel');
            // get selected IDs/names
            var checkboxes = $('ul.modal-win-list input[type=checkbox]', contractorsModal),
                emailIds = [],
                emailFormTitle = null,
                contractorName = null; // used only if one checkbox checked
            checkboxes.each(function(index, checkbox) {
                if (checkbox.checked) {
                    var emailId = $(checkbox).parent().find('input[type=hidden]').val();
                    contractorName = $(checkbox).parent().find('.js-contractor-label').text()
                    emailIds.push(emailId);
                };
            });

            if (emailIds.length > 0) {
                var company = {
                    name: emailIds.length === 1 ? contractorName : 'Multiple Companies',
                    multipleEmailIds: emailIds.join(','),
                    isMultiple: true
                }
                contractorsModal.hide();
                $('body').toggleClass('on-popup');
                showEmailform(company);
            } else {
                $('#ChooseEmailPanel #errornote').show();
            };

        });
    }

    function enableEmailForm(service) {
        var emailform = $('#ModalEmailForm'),
            companies = $('.company'),
            cancelButton = $('#cancelButton'),
            submitButton = $('#SubmitEmailformButton'),
            successEmailModal = $('#EmailComplete');

        cancelButton.click(function(event) {
            event.preventDefault();
            $('.emailform-validator', emailform).css('visibility', 'hidden');
            $('body').toggleClass('on-modal');
            emailform.hide();
        });
        submitButton.off('click');
        submitButton.click(function(event) {

            if (Page_ClientValidate('EmailFormGroup')) {
                // Disable buttons during processing form
                cancelButton.attr('disabled', 'disabled');
                submitButton.attr('disabled', 'disabled');

                var formData = {};
                var contractorIds = $('#EmailContractorID').val();
                formData.contractorEmails = [contractorIds];
                if (contractorIds == '') {
                    contractorIds = $('#MultipleEmailIDs').val();
                    formData.contractorEmails = contractorIds.split(',');
                }
                formData.cityId = $('#HiddenListCityId').val(),
                    formData.areaId = $('#HiddenListAreaId').val(),
                    formData.categoryId = $('#HiddenListCategoryId').val(),
                    formData.isPpc = $('#HiddenListIsPpc').val().toLowerCase() === 'true';

                $('#ModalEmailForm .emailform-field').toArray().forEach(function(item, index) {
                    switch (item.type) {
                    case "checkbox":
                        formData[item.getAttribute('fieldName')] = item.checked;
                        break;
                    default:
                        formData[item.getAttribute('fieldName')] = item.value;
                        break;
                    }
                });

                service.SubmitEmailForm(formData, function(response) {
                    console.log(response);

                    cancelButton.removeAttr('disabled');
                    submitButton.removeAttr('disabled');

                    if (response.success) {
                        // Close form and show success pop-up
                        $('body').toggleClass('on-modal');
                        emailform.hide();
                        $('body').toggleClass('on-popup');
                        successEmailModal.show();
                    } else {
                        //TODO highlight wrong fields
                    }
                });
                return false;
            } else {
                event.preventDefault();
            };

        });

        emailform.on('keypress', function(e) {
            var code = e.keyCode || e.which,
                type = e.target.type;
            if (code == 13 && type !== "textarea") {
                e.preventDefault();
                submitButton.click();
            };
        });
    }

    function enableRequestTopicForm(service) {

        var emailform = $('#RequesTopicEmailForm'),
            cancelButton = $('#EmailCancel'),
            submitButton = $('#ReqEmailOK'),
            successEmailModal = $('#EmailRequestComplete'),
            requestBtn = $('#RequestTopic, #RequestAGuide, #FooterLinkRequestGuide');

        requestBtn.off('click');
        requestBtn.click(function(event) {
            event.preventDefault();

            // Show form
            $('body').toggleClass('on-modal');
            showRequestTopicEmailForm();
        });

        cancelButton.click(function(event) {
            event.preventDefault();
            $('.emailform-validator', emailform).css('visibility', 'hidden');
            emailform.hide();
            $('body').toggleClass('on-modal');
        });

        submitButton.off('click');
        submitButton.click(function(event) {

            if (Page_ClientValidate('RequestTopicFormGroup')) {
                // Disable buttons during processing form
                cancelButton.attr('disabled', 'disabled');
                submitButton.attr('disabled', 'disabled');

                var formData = {};
                var cookie = getCookieValues($.cookie('bprpreferences'));
                if (cookie && cookie.areaid && cookie.cityid) {
                    formData.areaId = cookie['areaid'];
                    formData.cityId = cookie['cityid'];
                }
                $('#RequesTopicEmailForm .emailform-field').toArray().forEach(function(item, index) {
                    switch (item.type) {
                    case "checkbox":
                        formData[item.getAttribute('fieldName')] = item.checked;
                        break;
                    default:
                        formData[item.getAttribute('fieldName')] = item.value;
                        break;
                    }
                });
                service.RequestTopicForm(formData, function(response) {
                    console.log(response);

                    cancelButton.removeAttr('disabled');
                    submitButton.removeAttr('disabled');

                    if (response.success) {
                        // Close form and show success pop-up
                        $('body').toggleClass('on-modal');
                        emailform.hide();
                        $('body').toggleClass('on-popup');
                        successEmailModal.show();
                    } else {
                        //TODO highlight wrong fields
                    }
                });
                return false;
            } else {
                event.preventDefault();
            }
        });

        emailform.on('keypress', function(e) {
            var code = e.keyCode || e.which,
                type = e.target.type;
            if (code == 13 && type !== "textarea") {
                e.preventDefault();
                submitButton.click();
            };
        });

        if (successEmailModal) {
            var closeControls = successEmailModal.find('.modal-win-close, .modal-win-bg, .modal-button');
            $.each(closeControls, function(index, control) {
                $(control).click(function(e) {
                    e.preventDefault();
                    successEmailModal.hide();
                    $('body').toggleClass('on-popup');
                    //$('body').toggleClass('on-modal');
                });
            });
        };

    }

    function enableNewsletterSignUpForm(service) {

        var emailform = $('#NewsletterSignUpEmailForm'),
            cancelButton = $('#NewsEmailCancel'),
            submitButton = $('#NewsEmailOK'),
            successEmailModal = $('#NewsletterSignUpComplete'),
            requestBtn = $('#MenuNewsletterSignUp, #NewsletterSignUp, #FooterLinkNewsletter');

        requestBtn.off('click');
        requestBtn.click(function(event) {
            event.preventDefault();

            // Show form
            $('body').toggleClass('on-modal');
            showNewsletterSignUpEmailForm(service);
        });

        cancelButton.click(function(event) {
            event.preventDefault();
            $('.emailform-validator', emailform).css('visibility', 'hidden');
            emailform.hide();
            $('body').toggleClass('on-modal');
        });

        emailform.on('keypress', function(e) {
            var code = e.keyCode || e.which,
                type = e.target.type;
            if (code == 13 && type !== "textarea") {
                e.preventDefault();
                submitButton.click();
            };
        });


        if (successEmailModal) {
            var closeControls = successEmailModal.find('.modal-win-close, .modal-win-bg, .modal-button');
            $.each(closeControls, function(index, control) {
                $(control).click(function(e) {
                    e.preventDefault();
                    successEmailModal.hide();
                    $('body').toggleClass('on-popup');
                    //$('body').toggleClass('on-modal');
                });
            });
        };

    }

    function enableModals() {
        var successEmailModal = $('#SuccessEmailModal'),
            msgModal = $('#MsgPopup'),
            zipCodeChangeModal = $(CHANGE_ZIP_POPUP),
            notFoundZipModal = $('#ZipNotFoundModal');

        if (successEmailModal) {
            var closeControls = successEmailModal.find('.modal-win-close, .modal-win-bg, .modal-button');
            $.each(closeControls, function(index, control) {
                $(control).click(function(e) {
                    e.preventDefault();
                    successEmailModal.hide();
                    $('body').toggleClass('on-popup');
                    //$('body').toggleClass('on-modal');
                });
            });
        };

        //	if (successModal) {
        //	    // TODO is successEmailModal needed?
        //		var closeControls = successModal.find('.modal-win-close, .modal-win-bg, .modal-button');
        //		$.each(closeControls, function(index, control){
        //			$(control).click(function(e){
        //				e.preventDefault();
        //				successModal.hide();
        //				$('body').toggleClass('on-popup');
        //				//$('body').toggleClass('on-modal');
        //			});
        //		});
        //    };

        initModal(zipCodeChangeModal);
        initModal(notFoundZipModal);
        initModal(msgModal);

        function initModal(modalWindow) {
            if (modalWindow) {
                var closeControls = modalWindow.find('.modal-win-close, .modal-win-bg, .modal-button');
                $.each(closeControls, function(index, control) {
                    $(control).click(function(e) {
                        e.preventDefault();
                        modalWindow.hide();
                        $('body').toggleClass('on-popup');
                    });
                });
            };
        }

    }

    function showRequestTopicEmailForm() {
        var emailform = $('#RequesTopicEmailForm');

        var inputs = $('input[type=text], input[type=email], textarea', emailform);
        $.each(inputs, function(index, input) {
            input.value = "";
        });
        emailform.show();
    }

    function showNewsletterSignUpEmailForm(service) {
        var emailform = $('#NewsletterSignUpEmailForm');

        var inputs = $('input[type=text], input[type=email], textarea', emailform);
        $.each(inputs, function(index, input) {
            input.value = "";
        });


        // Add service handler
        var subscribeButton = $('#NewsEmailOK'),
            cancelButton = $('#NewsEmailCancel'),
            successEmailModal = $('#NewsletterSignUpComplete');

        subscribeButton.off('click');
        subscribeButton.click(function(event) {
            if (Page_ClientValidate('NewsletterFormGroup')) {
                // Disable buttons during processing form
                cancelButton.attr('disabled', 'disabled');
                subscribeButton.attr('disabled', 'disabled');

                var formData = {};
                $('#NewsletterSignUpEmailForm .emailform-field').toArray().forEach(function(item, index) {
                    switch (item.type) {
                    case "checkbox":
                        formData[item.getAttribute('fieldName')] = item.checked;
                        break;
                    default:
                        formData[item.getAttribute('fieldName')] = item.value;
                        break;
                    }
                });
                var cookie = getCookieValues($.cookie('bprpreferences'));
                if (cookie && cookie.areaid && cookie.cityid) {
                    formData.areaId = cookie['areaid'];
                    formData.cityId = cookie['cityid'];
                }
                service.SubmitNewsletterSignUpForm(formData, function(response) {
                    console.log(response);

                    cancelButton.removeAttr('disabled');
                    subscribeButton.removeAttr('disabled');

                    if (response.success) {
                        // Close form and show success pop-up
                        $('body').toggleClass('on-modal');
                        emailform.hide();
                        $('body').toggleClass('on-popup');
                        successEmailModal.show();
                    } else {
                        //TODO highlight wrong fields
                    }
                });
                return false;
            } else {
                event.preventDefault();
            };
        });

        emailform.show();
    }

    function enableMenuButtons(service) {
        var subscribeButton = $('#subscribe-button'),
            subscribeEmail = $('#subscribe-email'),
            successModal = $('#success-modal'),
            responseMessage = successModal.find('.modal-win-text'),
            responseHeader = successModal.find('.modal-win-header');

        var changeZipButton = $('#ZipCodeChangeButton'),
            changeZipField = $('#ZipCodeChange');

        subscribeButton.click(function(e) {
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
                    }, function(response) {
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


        changeZipButton.click(function(el) {
            var zipExp = /^[0-9]{5}$/;
            var isValid = zipExp.test(changeZipField[0].value);
            if (!isValid) {
                e.preventDefault();
            };
        });
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
            fullLink.click(function(e) {
                e.preventDefault();
                $.cookie('nomobile', 'true', { expires: 1, path: "/" });
                location.reload();
            });
        };
    }

//Turning on event triggers on UpdatePanel content reload
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function() {
    });
});

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
    $.each(inputs, function (index, input) {
        input.value = "";
    });
    emailform.show();
    $('body').toggleClass('on-modal');

    companyNameHidden.val(company.name);
    emailformCompanyName.text(company.name);
}

// Called from SiteMobile.Master
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




/*
* Deprecated
function enableSingleEmailButtons() {
// SingleEmail_CLick
$('#menu').on('click', '.company-email-link', function (event) {
var company = {
name: $('.company-name', this.parentNode).text(),
email: $('.company-email', this.parentNode).val(),
cid: $('.company-cid', this.parentNode).val(),
isMultiple: false
}
showEmailform(company);
});
}

function enableContractorsList() {
// BG, Close button - close
$("#menu").on('click', '#ChooseEmailPanel .modal-win-close, #ChooseEmailPanel .modal-win-bg', function (e) {
var contractorsModal = $('#ChooseEmailPanel');
contractorsModal.hide();
});

// SAVE TIME button - open win
$("#menu").on('click', '.savetime-background', function (e) {
var contractorsModal = $('#ChooseEmailPanel');
contractorsModal.show();
});

// Show More Button
$("#menu").on('click', '.category-more', function (e) {
e.preventDefault();
var contractors = $(this).parent().find('ul.companies>li');
if (contractors.length > 3) {
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
$('#errornote').hide();
var company = {
name: emailIds.length === 1 ? contractorName : 'Multiple Companies',
multipleEmailIds: emailIds.join(','),
isMultiple: true
}
contractorsModal.hide();
showEmailform(company);
} else {
$('#errornote').show();
};

});
}
*/