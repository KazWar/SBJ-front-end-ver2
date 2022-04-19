(function () {
    $(function () {
        var _handlingLinesService = abp.services.app.handlingLines;

        var _$handlingLineInformationForm = $('form[name=HandlingLineInformationsForm]');
        _$handlingLineInformationForm.validate();

		        var _HandlingLinecampaignTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HandlingLines/CampaignTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HandlingLines/_HandlingLineCampaignTypeLookupTableModal.js',
            modalClass: 'CampaignTypeLookupTableModal'
        });        var _HandlingLineproductHandlingLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HandlingLines/ProductHandlingLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HandlingLines/_HandlingLineProductHandlingLookupTableModal.js',
            modalClass: 'ProductHandlingLookupTableModal'
        });
   
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
      
	            $('#OpenCampaignTypeLookupTableButton').click(function () {

            var handlingLine = _$handlingLineInformationForm.serializeFormToObject();

            _HandlingLinecampaignTypeLookupTableModal.open({ id: handlingLine.campaignTypeId, displayName: handlingLine.campaignTypeName }, function (data) {
                _$handlingLineInformationForm.find('input[name=campaignTypeName]').val(data.displayName); 
                _$handlingLineInformationForm.find('input[name=campaignTypeId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignTypeNameButton').click(function () {
                _$handlingLineInformationForm.find('input[name=campaignTypeName]').val(''); 
                _$handlingLineInformationForm.find('input[name=campaignTypeId]').val(''); 
        });
		
        $('#OpenProductHandlingLookupTableButton').click(function () {

            var handlingLine = _$handlingLineInformationForm.serializeFormToObject();

            _HandlingLineproductHandlingLookupTableModal.open({ id: handlingLine.productHandlingId, displayName: handlingLine.productHandlingDescription }, function (data) {
                _$handlingLineInformationForm.find('input[name=productHandlingDescription]').val(data.displayName); 
                _$handlingLineInformationForm.find('input[name=productHandlingId]').val(data.id); 
            });
        });
		
		$('#ClearProductHandlingDescriptionButton').click(function () {
                _$handlingLineInformationForm.find('input[name=productHandlingDescription]').val(''); 
                _$handlingLineInformationForm.find('input[name=productHandlingId]').val(''); 
        });
		


        function save(successCallback) {
            if (!_$handlingLineInformationForm.valid()) {
                return;
            }
            if ($('#HandlingLine_CampaignTypeId').prop('required') && $('#HandlingLine_CampaignTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignType')));
                return;
            }
            if ($('#HandlingLine_ProductHandlingId').prop('required') && $('#HandlingLine_ProductHandlingId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductHandling')));
                return;
            }

            var handlingLine = _$handlingLineInformationForm.serializeFormToObject();
			
			 abp.ui.setBusy();
			 _handlingLinesService.createOrEdit(
				handlingLine
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               abp.event.trigger('app.createOrEditHandlingLineModalSaved');
               
               if(typeof(successCallback)==='function'){
                    successCallback();
               }
			 }).always(function () {
			    abp.ui.clearBusy();
			});
        };
        
        function clearForm(){
            _$handlingLineInformationForm[0].reset();
        }
        
        $('#saveBtn').click(function(){
            save(function(){
                window.location="/App/HandlingLines";
            });
        });
        
        $('#saveAndNewBtn').click(function(){
            save(function(){
                if (!$('input[name=id]').val()) {//if it is create page
                   clearForm();
                }
            });
        });
    });
})();