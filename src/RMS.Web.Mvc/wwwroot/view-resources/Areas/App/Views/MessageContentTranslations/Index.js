(function () {
    $(function () {

        var _$messageContentTranslationsTable = $('#MessageContentTranslationsTable');
        var _messageContentTranslationsService = abp.services.app.messageContentTranslations;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.MessageContentTranslation';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageContentTranslations.Create'),
            edit: abp.auth.hasPermission('Pages.MessageContentTranslations.Edit'),
            'delete': abp.auth.hasPermission('Pages.MessageContentTranslations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageContentTranslations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageContentTranslations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageContentTranslationModal'
        });       

		 var _viewMessageContentTranslationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageContentTranslations/ViewmessageContentTranslationModal',
            modalClass: 'ViewMessageContentTranslationModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$messageContentTranslationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageContentTranslationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessageContentTranslationsTableFilter').val(),
					contentFilter: $('#ContentFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val(),
					messageComponentContentContentFilter: $('#MessageComponentContentContentFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewMessageContentTranslationModal.open({ id: data.record.messageContentTranslation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.messageContentTranslation.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.messageContentTranslation.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessageContentTranslation(data.record.messageContentTranslation);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "messageContentTranslation.content",
						 name: "content"   
					},
					{
						targets: 2,
						 data: "localeDescription" ,
						 name: "localeFk.description" 
					},
					{
						targets: 3,
						 data: "messageComponentContentContent" ,
						 name: "messageComponentContentFk.content" 
					}
            ]
        });

        function getMessageContentTranslations() {
            dataTable.ajax.reload();
        }

        function deleteMessageContentTranslation(messageContentTranslation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageContentTranslationsService.delete({
                            id: messageContentTranslation.id
                        }).done(function () {
                            getMessageContentTranslations(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewMessageContentTranslationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messageContentTranslationsService
                .getMessageContentTranslationsToExcel({
				filter : $('#MessageContentTranslationsTableFilter').val(),
					contentFilter: $('#ContentFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val(),
					messageComponentContentContentFilter: $('#MessageComponentContentContentFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageContentTranslationModalSaved', function () {
            getMessageContentTranslations();
        });

		$('#GetMessageContentTranslationsButton').click(function (e) {
            e.preventDefault();
            getMessageContentTranslations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessageContentTranslations();
		  }
		});
    });
})();