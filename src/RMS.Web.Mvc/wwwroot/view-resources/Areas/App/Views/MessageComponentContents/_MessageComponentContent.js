(function () {
    $(function () {
        var _messageContentTranslationService = abp.services.app.messageContentTranslations;

        $('.btn__message-component-contents--edit').click(function (e) {
            const id = $(e.currentTarget).attr('data-message-component-content-id');
            const messageType = $(e.currentTarget).attr('data-message-type');
            if (id === undefined || !id) {
                console.error(`[${this}]: Cannot edit message component content: [id] is missing.`);
                return;
            }
            editEntityModal(id, messageType);
        });

        $('.btn__message-component-contents--delete').click(function (e) {
            const id = $(e.currentTarget).attr('data-message-component-content-id');
            const messageTranslationId = $(e.currentTarget).attr('data-message-content-translation-id');
            if (messageTranslationId === undefined || !messageTranslationId) {
                console.error(`[${this}]: Cannot delete message content translation: [messageTranslationId] is missing.`);
                return;
            }
            deleteMessageComponentContent(messageTranslationId);
        });

        function editEntityModal(messageComponentContentId, messageType) {
            if (messageComponentContentId === undefined || !messageComponentContentId) {
                console.error(`[${this}]: there is no ID to work with.`);
                return;
            }

            new app.ModalManager({
                viewUrl: abp.appPath + 'App/MessageComponentContents/EditEntityModal',
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_EditEntityModal.js',
                modalClass: 'EditMessageComponentContentEntityModal'
            }).open({ id: messageComponentContentId, messageType: messageType });
        }

        function deleteMessageComponentContent(id) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageContentTranslationService.delete({
                            id: id
                        }).done(function () {
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            window.location.reload();
                        });
                    }
                }
            );
        }
    });
})();