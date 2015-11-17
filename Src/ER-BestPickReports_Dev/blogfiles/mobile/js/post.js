(function() {
'use strict';

    $(function() {
        handleAuthor();
        handleLocalBestContractors();

        if ($('.hIsInMarketUser').val() === 'false') {
            $('.post-local-best').hide();
        }
    });


    function handleAuthor() {
        var authorTag = $('.post-content>p[style]').first(),
        	authorTagText = $('.post-content>p[style]').first().text(),
        	dividerPosition = authorTagText.indexOf('|'),
        	byPosition = authorTagText.indexOf('By');

        var authorName = authorTagText.substring(3, dividerPosition - 1),
            authorTitle = authorTagText.substring(dividerPosition + 2),
            authorNameTag = $('.post-author-name'),
            authorTitleTag = $('.post-author-title');

        var newAuthorTextExist = authorNameTag.text().trim() !== ""|| authorTitleTag.text().trim() !== "";
        if (newAuthorTextExist) {
            if (dividerPosition > -1 && byPosition > -1) {
                // tag exists
                authorTag.hide();
            };
        } else {
            if (dividerPosition > -1 && byPosition > -1) {
                // tag exists
                authorTag.hide();
                authorNameTag.text(authorName);
                authorTitleTag.text(authorTitle);
            } else {
                authorNameTag.hide();
                authorTitleTag.hide();
            };
        };

    }

    function handleLocalBestContractors() {
    	var catlink = $('.post .catlink'),
    		localBest = $('.post-local-best'),
    		localBestText = $('.post-local-best-text');
    	if (catlink && localBestText && localBest) {
    		localBestText.append(catlink);
    		localBest.show();
    	};
    }
})();
