(function ($) {
    app.modals.CreateMessageComponentContentEntityModal = function () {
        var _modalManager;
        var campaignTypeEventRegistrationStatusId = null;
        var editorContent = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
        }

        $('.user-interaction__tile').click(function (e) {
            const ctid = $(e.currentTarget).data('id');
            campaignTypeEventRegistrationStatusId = ctid;

            $('.user-interaction__tile').addClass('d-none');
            $('#chooseMessageType__tiles').load('/App/MessageComponentContents/ChooseMessageType/', { campaignTypeEventRegistrationStatusId: campaignTypeEventRegistrationStatusId});
            $('#chooseMessageType__tiles').show();
        });

        this.save = function () {
            _modalManager.setBusy(true);

            const container = $('#editor__container');
            var children = container[0].children;

            $.each(children, function (i, val) {
                var id = $(this).attr('data-message-component-id');
                var messageTypeName = $(this).attr('data-message-component-type-name');
                var content = val.lastElementChild.children[0].innerHTML;
                editorContent.push({
                    messageComponentId : id,
                    content: content,
                    messageTypeName: messageTypeName
                });
            });
            const ctid = container.data('campaign-type-event-registration-status-id') || campaignTypeEventRegistrationStatusId;

            if (container === undefined) {
                console.error('Cannot proceed with saving new message component contents: [container] is undefined.');
                return;
            } else if (ctid === undefined) {
                console.error('Cannot proceed with saving new message component contents: [ctid] is undefined.');
                return;
            }

            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                url: 'MessageComponentContents/AddMessageComponentContents/',
                data: JSON.stringify({
                    CampaignTypeEventRegistrationStatusId: ctid,
                    MessageComponentDictionary: editorContent
                }),
                complete: function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditMessageComponentContentModalSaved');
                    _modalManager.setBusy(false);
                    window.location.reload();
                }
            });
        };

        this.destroyEditor = function (selector) {
            if ($(selector)[0]) {
                let content = $(selector).find('.ql-editor').html();
                $(selector).html(content);

                $(selector).siblings('.ql-toolbar').remove();
                $(selector + '*[class*=\'ql-\']').removeClass(function (index, css) {
                    return (css.match(/(^|\s)ql-\S+/g) || []).join(' ');
                });
                $(selector + '[class*=\'ql-\']').removeClass(function (index, css) {
                    return (css.match(/(^|\s)ql-\S+/g) || []).join(' ');
                });
            } else {
                console.error('Editor does not exist and therefore cannot be destroyed.');
            }
        }
    };
})(jQuery);