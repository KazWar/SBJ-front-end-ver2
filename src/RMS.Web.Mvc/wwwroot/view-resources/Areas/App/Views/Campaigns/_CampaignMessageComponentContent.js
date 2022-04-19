(function () {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();

        function editEntityModal(messageComponentContentId, messageType) {
            if (messageComponentContentId === undefined || !messageComponentContentId) {
                console.error(`[${this}]: there is no ID to work with.`);
                return;
            }

            new app.ModalManager({
                viewUrl: abp.appPath + 'App/MessageComponentContents/EditEntityModal',
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_EditEntityModal.js',
                modalClass: 'EditMessageComponentContentEntityModal'
            }).open({ id: messageComponentContentId, localeId: $('#localeId').val(), messageType: messageType });
        }

        $('.btn__message-component-contents--edit').click(function (e) {
            // Using currentTarget instead of target, so that it won't matter where on the element is being clicked (i.e. the cog icon)
            const id = $(e.currentTarget).attr('data-message-component-content-id');
            const messageType = $(e.currentTarget).attr('data-message-type');
            if (id === undefined || !id) {
                console.error(`[${this}]: Cannot edit message component content: [id] is missing.`);
                return;
            }

            editEntityModal(id, messageType);
        });
    });
})();