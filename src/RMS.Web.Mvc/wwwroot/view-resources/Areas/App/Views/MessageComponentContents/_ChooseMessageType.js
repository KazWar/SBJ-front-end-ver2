(function () {
    $(function () {
        var _messageVariableService = abp.services.app.messageVariables;
        var messageComponents = null;
        var messageComponentIds = [];
        var editorContentCollection = [];
        var getAllMessageVariables = [];
        var messageVariableDescriptionList = [];

        async function populateMessageVariable() {
            return await _messageVariableService.getAllMessageVariables();
        };

        (async () => {
            getAllMessageVariables = await populateMessageVariable();
        })();

        function createIterationEditor() {
            let div = document.createElement('div');
            let editor = document.createElement('div');

            div.classList.add('row__column');
            editor.classList.add('editor__wysiwyg');

            div.appendChild(editor);

            return div;
        }

        $('.tile__message-type').click(function (e) {
            const messageTypeId = $(e.currentTarget).data('id');
            const messageTypeName = $(e.currentTarget).data('message-type-name');
            const messageId = $(e.currentTarget).data('message-id');
            if (messageTypeId === undefined || !messageTypeId) {
                console.log('There is no associated MessageTypeID.');
                return;
            }

            $('.wrapper__message-type').addClass('d-none');
            $('.step__choose-mt .heading__main').addClass('d-none');
            $('.step__choose-mt .paragraph__main').addClass('d-none');

            $.ajax({
                type: 'GET',
                async: true,
                url: 'MessageComponentContents/GetMessageComponentsByMessageTypeId/',
                data: {
                    messageTypeId: messageTypeId,
                    messageTypeName: messageTypeName,
                    messageId: messageId
                },
                mimeType: 'json',
                complete: function (data) {
                    if (data === undefined || !data) {
                        console.error('No message components have been returned.');
                        return;
                    }

                    messageComponents = data.responseJSON.result.items;

                    $('.step__choose-mt').removeClass('d-none');

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

                    messageComponents.forEach((item, index) => {
                        let editorContainer, iterationRef, editorRef = null;
                        editorContainer = document.getElementById('editor__container');

                        iterationRef = createIterationEditor();
                        iterationRef.id = `editor-box__box-${index}`;

                        const containerWrapper = document.createElement('div');
                        containerWrapper.classList.add('editor__container__wrapper');

                        messageComponentIds.push(item.messageComponent.id);
                        containerWrapper.setAttribute('data-message-component-id', item.messageComponent.id);
                        containerWrapper.setAttribute('data-message-component-type-name', item.messageComponentTypeName);

                        containerWrapper.appendChild(iterationRef);
                        editorContainer.appendChild(containerWrapper);

                        let heading = document.createElement('h5');
                        heading.classList.add('editor__heading');
                        heading.innerText = `Add ${item.messageComponentTypeName} contents for ${item.messageTypeName}`;

                        containerWrapper.prepend(heading);
                        
                        editorRef = new Quill(`#editor-box__box-${index}`, {
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
                                        [{'header': [1,2,3, false]}],
                                        ['bold', 'italic', 'underline', 'link'],
                                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                                        [{ 'AddMessageVariable': messageVariableDescriptionList } ],
                                        ['clean'],
                                        ['table'],
                                        ['image']
                                    ],
                                    handlers: {
                                        'AddMessageVariable': function (value) {
                                            const range = this.quill.getSelection();
                                            this.quill.insertText(range, '#{' + value +'}' );
                                        },
                                    },
                                }
                            },
                            theme: 'snow'
                        });

                        editorContentCollection[index] = '';
                        editorRef.on('text-change', function (delta, oldDelta, source) {
                            editorContentCollection[index] = editorRef.root.innerHTML.trim();
                        });
                    });

                    $('#message-component-contents__save-button').prop('disabled', false);
                    $('#message-component-contents__save-button').removeClass('disabled');
                }
            });
        });

        
    });
})();