    (function ($) {
        app.modals.EditMessageComponentContentEntityModal = function () {
        var _modalManager;
        var messageComponentIds = [];
        var editorContentCollection = [];
        var editorContent = [];
        var getAllMessageVariables = [];
        var messageVariableDescriptionList = [];
        var localeId = $('#localeId').data('locale-id');
        
        var _messageVariableService = abp.services.app.messageVariables;

        async function populateMessageVariable() {
            return await _messageVariableService.getAllMessageVariables();
        };

        (async () => {
            getAllMessageVariables = await populateMessageVariable();
        })();

        this.init = function (modalManager) {
            _modalManager = modalManager;

            const id = $('#message-component-content').data('message-component-content-id');
            const campaignTypeEventRegistrationStatusId = $('#message-component-content').data('campaign-type-event-registration-status-id');
            const messageType = $('#message-component-content').data('message-type');

            function createIterationEditor() {
                let div = document.createElement('div');
                let editor = document.createElement('div');
                div.classList.add('row__column');
                editor.classList.add('editor__wysiwyg');
                div.appendChild(editor);
                return div;
            }

            $.ajax({
                type: 'GET',
                async: true,
                url: '/App/MessageComponentContents/EditMessageComponentContentTranslation/',
                data: {
                    messageType: messageType,
                    id: id,
                    campaignTypeEventRegistrationStatusId: campaignTypeEventRegistrationStatusId,
                    localeId: localeId
                },
                mimeType: 'json',
                complete: function (data) {
                    if (data === undefined || !data) {
                        console.error('No message components have been returned.');
                        return;
                    }
                    var messageContentTranslation = data.responseJSON.result.messageComponentContentTranslation;
                    var messageComponentContents = data.responseJSON.result.messageComponentContentsPerCampaignTypeEventRegistrationStatusId;

                    var messageVariables = getAllMessageVariables;
                    $.each(messageVariables, function (index, value) {
                        messageVariableDescriptionList.push(
                            value.messageVariable.description
                        );
                    });
                    
                    Quill.register({
                        'modules/tableUI': quillTableUI.default
                    }, true);
                    Quill.register('modules/blotFormatter', QuillBlotFormatter.default);

                    messageComponentContents.forEach((item, index) => {
                        let editorContainer, iterationRef, editorRef = null;
                        editorContainer = document.getElementById('editor__container');

                        iterationRef = createIterationEditor();
                        iterationRef.id = `editor-box__box-${index}`;

                        const containerWrapper = document.createElement('div');
                        containerWrapper.classList.add('editor__container__wrapper');

                        messageComponentIds.push(item.messageComponentContent.id);
                        containerWrapper.setAttribute('data-message-component-content-id', item.messageComponentContent.id);

                        containerWrapper.appendChild(iterationRef);
                        editorContainer.appendChild(containerWrapper);

                        let heading = document.createElement('h5');
                        heading.classList.add('editor__heading');
                        heading.innerText = `Edit ${item.messageComponentType} contents for ${item.messageType}`;

                        containerWrapper.prepend(heading);
                        
                        editorRef = new Quill(`#editor-box__box-${index}`, {
                            theme: 'snow',
                            modules: {
                                table: true,
                                tableUI: true,
                                blotFormatter: {
                                    overlay: {
                                        className: 'blot-formatter__overlay',
                                        style: {
                                            position: 'absolute',
                                            boxSizing: 'border-box',
                                            border: '1px dashed #444',
                                        },
                                    },
                                    align: {
                                        attribute: 'data-align',
                                        aligner: {
                                            applyStyle: true,
                                        },
                                        icons: {
                                            left: `<svg viewbox="0 0 18 18"><line class="ql-stroke" x1="3" x2="15" y1="9" y2="9"></line><line class="ql-stroke" x1="3" x2="13" y1="14" y2="14"></line><line class="ql-stroke" x1="3" x2="9" y1="4" y2="4"></line></svg>`,
                                            center: `<svg viewbox="0 0 18 18"><line class="ql-stroke" x1="15" x2="3" y1="9" y2="9"></line><line class="ql-stroke" x1="14" x2="4" y1="14" y2="14"></line><line class="ql-stroke" x1="12" x2="6" y1="4" y2="4"></line></svg>`,
                                            right: `<svg viewbox="0 0 18 18"><line class="ql-stroke" x1="15" x2="3" y1="9" y2="9"></line><line class="ql-stroke" x1="15" x2="5" y1="14" y2="14"></line><line class="ql-stroke" x1="15" x2="9" y1="4" y2="4"></line></svg>`,
                                        },
                                        toolbar: {
                                            allowDeselect: true,
                                            mainClassName: 'blot-formatter__toolbar',
                                            mainStyle: {
                                                position: 'absolute',
                                                top: '-12px',
                                                right: '0',
                                                left: '0',
                                                height: '0',
                                                minWidth: '100px',
                                                font: '12px/1.0 Arial, Helvetica, sans-serif',
                                                textAlign: 'center',
                                                color: '#333',
                                                boxSizing: 'border-box',
                                                cursor: 'default',
                                                zIndex: '1',
                                            },
                                            buttonClassName: 'blot-formatter__toolbar-button',
                                            addButtonSelectStyle: true,
                                            buttonStyle: {
                                                display: 'inline-block',
                                                width: '24px',
                                                height: '24px',
                                                background: 'white',
                                                border: '1px solid #999',
                                                verticalAlign: 'middle',
                                            },
                                            svgStyle: {
                                                display: 'inline-block',
                                                width: '24px',
                                                height: '24px',
                                                background: 'white',
                                                border: '1px solid #999',
                                                verticalAlign: 'middle',
                                            },
                                        },
                                    },
                                    resize: {
                                        handleClassName: 'blot-formatter__resize-handle',
                                        handleStyle: {
                                            position: 'absolute',
                                            height: '12px',
                                            width: '12px',
                                            backgroundColor: 'white',
                                            border: '1px solid #777',
                                            boxSizing: 'border-box',
                                            opacity: '0.80',
                                        },
                                    },
                                },
                                toolbar: {
                                    container: [
                                        [{ 'header': [1, 2, 3, false] }],
                                        ['bold', 'italic', 'underline', 'link'],
                                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                                        [{ 'AddMessageVariable': messageVariableDescriptionList }],
                                        ['clean'],
                                        ['table'],
                                        ['image']
                                    ],
                                    handlers: {
                                        'AddMessageVariable': function (value) {
                                            const range = this.quill.getSelection();
                                            this.quill.insertText(range, '#{' + value + '}');
                                        },
                                    }
                                },
                            }
                        });

                        messageContentTranslation.forEach((val, i) => {
                            if (val === null) {
                                editorRef.root.innerHTML = item.messageComponentContent.content;
                                containerWrapper.setAttribute('data-message-translation-id', 0);
                            }
                        });

                        messageContentTranslation.forEach((val, i) => {
                            if (val !== null && val.messageContentTranslation !== null && val.messageContentTranslation.messageComponentContentId == item.messageComponentContent.id) {
                                editorRef.root.innerHTML = val.messageContentTranslation.content;
                                containerWrapper.setAttribute('data-message-translation-id', val.messageContentTranslation.id);
                                return true;
                            }
                            else return false;
                        });
                        editorContentCollection[index] = item.messageComponentContent.content;

                        editorRef.on('text-change', function (delta, oldDelta, source) {
                            editorContentCollection[index] = editorRef.root.innerHTML.trim();
                        });
                    });

                    $('#message-component-contents__save-button').prop('disabled', false);
                    $('#message-component-contents__save-button').removeClass('disabled');
                }
            });
        }

        this.save = function () {
            _modalManager.setBusy(true);

            const container = $('#editor__container');
            if (container === undefined) {
                console.error('Cannot proceed with saving new message component contents: [container] is undefined.');
                return;
            }
            var children = container[0].children;
            const campaignTypeEventRegistrationStatusId = $('#message-component-content').data('campaign-type-event-registration-status-id');

            $.each(children, function (i, val) {
                const messageComponentContentId = $(this).data('messageComponentContentId');
                const messageContentTranslationId = $(this).data('messageTranslationId');
                const messageComponentId = $('#message-component-content').data('messageComponentId');
                var content = val.lastElementChild.children[0].innerHTML;
                editorContent.push({
                    messageContentTranslationId: messageContentTranslationId,
                    messageComponentContentId: messageComponentContentId,
                    messageComponentId: messageComponentId,
                    localeId : localeId,
                    content: content,
                    campaignTypeEventRegistrationStatusId: campaignTypeEventRegistrationStatusId
                });
            });

            $.each(editorContent, function (i, val) {
                $.ajax({
                    type: 'PUT',
                    contentType: 'application/json',
                    dataType: 'json',
                    async: false,
                    url: '/App/MessageComponentContents/EditMessageComponentContentTranslation/',
                    data: JSON.stringify({
                        Content: val.content,
                        MessageContentTranslationId: val.messageContentTranslationId,
                        MessageComponentContentId: val.messageComponentContentId,
                        messageComponentId: val.messageComponentId,
                        LocaleId: val.localeId,
                        CampaignTypeEventRegistrationStatusId: val.campaignTypeEventRegistrationStatusId
                    }),
                });
                if (i == editorContent.length - 1) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.editMessageComponentContentModalSaved')
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