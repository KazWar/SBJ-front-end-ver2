(function ($) {
    app.modals.CreateOrEditProjectManagerModal = function () {

        var _projectManagersService = abp.services.app.projectManagers;

        var _modalManager;
        var _$projectManagerInformationForm = null;

		        var _ProjectManageraddressLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProjectManagers/AddressLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProjectManagers/_ProjectManagerAddressLookupTableModal.js',
            modalClass: 'AddressLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$projectManagerInformationForm = _modalManager.getModal().find('form[name=ProjectManagerInformationsForm]');
            _$projectManagerInformationForm.validate();
        };

		          $('#OpenAddressLookupTableButton').click(function () {

            var projectManager = _$projectManagerInformationForm.serializeFormToObject();

            _ProjectManageraddressLookupTableModal.open({ id: projectManager.addressId, displayName: projectManager.addressPostalCode }, function (data) {
                _$projectManagerInformationForm.find('input[name=addressPostalCode]').val(data.displayName); 
                _$projectManagerInformationForm.find('input[name=addressId]').val(data.id); 
            });
        });
		
		$('#ClearAddressPostalCodeButton').click(function () {
                _$projectManagerInformationForm.find('input[name=addressPostalCode]').val(''); 
                _$projectManagerInformationForm.find('input[name=addressId]').val(''); 
        });
		


        this.save = function () {
            if (!_$projectManagerInformationForm.valid()) {
                return;
            }
            if ($('#ProjectManager_AddressId').prop('required') && $('#ProjectManager_AddressId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Address')));
                return;
            }

            var projectManager = _$projectManagerInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _projectManagersService.createOrEdit(
				projectManager
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProjectManagerModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);