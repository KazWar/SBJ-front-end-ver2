function openModalAddNewMessageComponentContent() {
    console.log('modal');
    $('#messageComponentContentModal').modal();
}

function deleteMessageComponentContent(messageComponentContentId) {
    if (!messageComponentContentId) return;

    var _messageComponentContentService = abp.services.app.messageComponentContents;

    _messageComponentContentService.delete({ id: messageComponentContentId })
        .done(function () {
            window.location.reload;
        });
}