var _messageComponentContentService = abp.services.app.messageComponentContents;

function openModalAddNewMessageComponentContent() {
    // reset the modal
    $('.user-interaction__tile').removeClass('d-none');
    $('.step__choose-mt').addClass('d-none');

    $('#message-component-contents__save-button')
        .prop('disabled', true)
        .addClass('disabled');

    $('#messageComponentContentModal').modal();
}

function saveEditorContentsAndPostToController(campaignTypeEventRegistrationStatusId) {
    $('#message-component-contents__save-button').prop('disabled', true);

    let messageComponentIds = [];
    let messageComponentDictionary = [];

    messageComponents.forEach(component => messageComponentIds.push(component.messageComponent.id));

    for (let i = 0; i < messageComponentIds.length; i++) {
        messageComponentDictionary.push({
            key: messageComponentIds[i],
            value: editorContents[i]
        });
    }

    // map the array to dictionary format
    let convertedMessageComponentDictionary = Object.assign({}, ...messageComponentDictionary.map(x => ({ [x.key]: x.value })));

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        url: 'MessageComponentContents/UpdateMessageComponentContents/',
        data: JSON.stringify({
            CampaignTypeEventRegistrationStatusId: campaignTypeEventRegistrationStatusId,
            MessageComponentDictionary: convertedMessageComponentDictionary
        }),
        complete: function (data) {
            console.log('Successfully added message component content.', data);
            window.location.reload();
        }
    });
}

function deleteMessageComponentContent(messageComponentContentId) {
    if (!messageComponentContentId) return;

    _messageComponentContentService.delete({ id: messageComponentContentId })
        .finally(window.location.reload());
}