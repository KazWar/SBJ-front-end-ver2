using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace RMS.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var uniqueCodeByCampaigns = pages.CreateChildPermission(AppPermissions.Pages_UniqueCodeByCampaigns, L("UniqueCodeByCampaigns"));
            uniqueCodeByCampaigns.CreateChildPermission(AppPermissions.Pages_UniqueCodeByCampaigns_Create, L("CreateNewUniqueCodeByCampaign"));
            uniqueCodeByCampaigns.CreateChildPermission(AppPermissions.Pages_UniqueCodeByCampaigns_Edit, L("EditUniqueCodeByCampaign"));
            uniqueCodeByCampaigns.CreateChildPermission(AppPermissions.Pages_UniqueCodeByCampaigns_Delete, L("DeleteUniqueCodeByCampaign"));

            var makitaBaseModelSerials = pages.CreateChildPermission(AppPermissions.Pages_MakitaBaseModelSerials, L("MakitaBaseModelSerials"));
            makitaBaseModelSerials.CreateChildPermission(AppPermissions.Pages_MakitaBaseModelSerials_Create, L("CreateNewMakitaBaseModelSerial"));
            makitaBaseModelSerials.CreateChildPermission(AppPermissions.Pages_MakitaBaseModelSerials_Edit, L("EditMakitaBaseModelSerial"));
            makitaBaseModelSerials.CreateChildPermission(AppPermissions.Pages_MakitaBaseModelSerials_Delete, L("DeleteMakitaBaseModelSerial"));

            var productGifts = pages.CreateChildPermission(AppPermissions.Pages_ProductGifts, L("ProductGifts"));
            productGifts.CreateChildPermission(AppPermissions.Pages_ProductGifts_Create, L("CreateNewProductGift"));
            productGifts.CreateChildPermission(AppPermissions.Pages_ProductGifts_Edit, L("EditProductGift"));
            productGifts.CreateChildPermission(AppPermissions.Pages_ProductGifts_Delete, L("DeleteProductGift"));

            var campaignCountries = pages.CreateChildPermission(AppPermissions.Pages_CampaignCountries, L("CampaignCountries"));
            campaignCountries.CreateChildPermission(AppPermissions.Pages_CampaignCountries_Create, L("CreateNewCampaignCountry"));
            campaignCountries.CreateChildPermission(AppPermissions.Pages_CampaignCountries_Edit, L("EditCampaignCountry"));
            campaignCountries.CreateChildPermission(AppPermissions.Pages_CampaignCountries_Delete, L("DeleteCampaignCountry"));

            var uniqueCodes = pages.CreateChildPermission(AppPermissions.Pages_UniqueCodes, L("UniqueCodes"));
            uniqueCodes.CreateChildPermission(AppPermissions.Pages_UniqueCodes_Create, L("CreateNewUniqueCode"));
            uniqueCodes.CreateChildPermission(AppPermissions.Pages_UniqueCodes_Edit, L("EditUniqueCode"));
            uniqueCodes.CreateChildPermission(AppPermissions.Pages_UniqueCodes_Delete, L("DeleteUniqueCode"));

            var CampaignTranslations = pages.CreateChildPermission(AppPermissions.Pages_CampaignTranslations, L("CampaignTranslations"));
            CampaignTranslations.CreateChildPermission(AppPermissions.Pages_CampaignTranslations_Create, L("CreateNewHandlingBatchLineHistory"));
            CampaignTranslations.CreateChildPermission(AppPermissions.Pages_CampaignTranslations_Edit, L("EditHandlingBatchLineHistory"));
            CampaignTranslations.CreateChildPermission(AppPermissions.Pages_CampaignTranslations_Delete, L("DeleteHandlingBatchLineHistory"));

            var handlingBatchLineHistories = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineHistories, L("HandlingBatchLineHistories"));
            handlingBatchLineHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineHistories_Create, L("CreateNewHandlingBatchLineHistory"));
            handlingBatchLineHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineHistories_Edit, L("EditHandlingBatchLineHistory"));
            handlingBatchLineHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineHistories_Delete, L("DeleteHandlingBatchLineHistory"));

            var handlingBatchHistories = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatchHistories, L("HandlingBatchHistories"));
            handlingBatchHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchHistories_Create, L("CreateNewHandlingBatchHistory"));
            handlingBatchHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchHistories_Edit, L("EditHandlingBatchHistory"));
            handlingBatchHistories.CreateChildPermission(AppPermissions.Pages_HandlingBatchHistories_Delete, L("DeleteHandlingBatchHistory"));

            var handlingBatchLineStatuses = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineStatuses, L("HandlingBatchLineStatuses"));
            handlingBatchLineStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineStatuses_Create, L("CreateNewHandlingBatchLineStatus"));
            handlingBatchLineStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineStatuses_Edit, L("EditHandlingBatchLineStatus"));
            handlingBatchLineStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchLineStatuses_Delete, L("DeleteHandlingBatchLineStatus"));

            var handlingBatchStatuses = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatchStatuses, L("HandlingBatchStatuses"));
            handlingBatchStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchStatuses_Create, L("CreateNewHandlingBatchStatus"));
            handlingBatchStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchStatuses_Edit, L("EditHandlingBatchStatus"));
            handlingBatchStatuses.CreateChildPermission(AppPermissions.Pages_HandlingBatchStatuses_Delete, L("DeleteHandlingBatchStatus"));

            var handlingBatchLines = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatchLines, L("HandlingBatchLines"));
            handlingBatchLines.CreateChildPermission(AppPermissions.Pages_HandlingBatchLines_Create, L("CreateNewHandlingBatchLine"));
            handlingBatchLines.CreateChildPermission(AppPermissions.Pages_HandlingBatchLines_Edit, L("EditHandlingBatchLine"));
            handlingBatchLines.CreateChildPermission(AppPermissions.Pages_HandlingBatchLines_Delete, L("DeleteHandlingBatchLine"));

            var handlingBatches = pages.CreateChildPermission(AppPermissions.Pages_HandlingBatches, L("HandlingBatches"));
            handlingBatches.CreateChildPermission(AppPermissions.Pages_HandlingBatches_Create, L("CreateNewHandlingBatch"));
            handlingBatches.CreateChildPermission(AppPermissions.Pages_HandlingBatches_Edit, L("EditHandlingBatch"));
            handlingBatches.CreateChildPermission(AppPermissions.Pages_HandlingBatches_Delete, L("DeleteHandlingBatch"));
            handlingBatches.CreateChildPermission(AppPermissions.Pages_HandlingBatches_ViewMenu, L("ViewMenuHandlingBatch"));

            var messageHistories = pages.CreateChildPermission(AppPermissions.Pages_MessageHistories, L("MessageHistories"));
            messageHistories.CreateChildPermission(AppPermissions.Pages_MessageHistories_Create, L("CreateNewMessageHistory"));
            messageHistories.CreateChildPermission(AppPermissions.Pages_MessageHistories_Edit, L("EditMessageHistory"));
            messageHistories.CreateChildPermission(AppPermissions.Pages_MessageHistories_Delete, L("DeleteMessageHistory"));

            var registrationHistories =
                pages.CreateChildPermission(AppPermissions.Pages_RegistrationHistories, L("RegistrationHistories"));
            registrationHistories.CreateChildPermission(AppPermissions.Pages_RegistrationHistories_Create,
                L("CreateNewRegistrationHistory"));
            registrationHistories.CreateChildPermission(AppPermissions.Pages_RegistrationHistories_Edit,
                L("EditRegistrationHistory"));
            registrationHistories.CreateChildPermission(AppPermissions.Pages_RegistrationHistories_Delete,
                L("DeleteRegistrationHistory"));
            registrationHistories.CreateChildPermission(AppPermissions.Pages_RegistrationHistories_ViewMenu,
                L("ViewMenuRegistrationHistory"));

            var makitaSerialNumbers =
                pages.CreateChildPermission(AppPermissions.Pages_MakitaSerialNumbers, L("MakitaSerialNumbers"));
            makitaSerialNumbers.CreateChildPermission(AppPermissions.Pages_MakitaSerialNumbers_Create,
                L("CreateNewMakitaSerialNumber"));
            makitaSerialNumbers.CreateChildPermission(AppPermissions.Pages_MakitaSerialNumbers_Edit,
                L("EditMakitaSerialNumber"));
            makitaSerialNumbers.CreateChildPermission(AppPermissions.Pages_MakitaSerialNumbers_Delete,
                L("DeleteMakitaSerialNumber"));
            makitaSerialNumbers.CreateChildPermission(AppPermissions.Pages_MakitaSerialNumbers_ViewMenu,
                L("ViewMenuMakitaSerialNumber"));

            var makitaCampaigns =
                pages.CreateChildPermission(AppPermissions.Pages_MakitaCampaigns, L("MakitaCampaigns"));
            makitaCampaigns.CreateChildPermission(AppPermissions.Pages_MakitaCampaigns_Create,
                L("CreateMakitaCampaigns"));
            makitaCampaigns.CreateChildPermission(AppPermissions.Pages_MakitaCampaigns_Edit,
                L("EditMakitaCampaigns"));
            makitaCampaigns.CreateChildPermission(AppPermissions.Pages_MakitaCampaigns_Delete,
                L("DeleteMakitaCampaigns"));
            makitaCampaigns.CreateChildPermission(AppPermissions.Pages_MakitaCampaigns_ViewMenu,
                L("ViewMenuMakitaCampaigns"));

            var rejectionReasons = pages.CreateChildPermission(AppPermissions.Pages_RejectionReasons, L("RejectionReasons"));
            rejectionReasons.CreateChildPermission(AppPermissions.Pages_RejectionReasons_Create, L("CreateNewRejectionReason"));
            rejectionReasons.CreateChildPermission(AppPermissions.Pages_RejectionReasons_Edit, L("EditRejectionReason"));
            rejectionReasons.CreateChildPermission(AppPermissions.Pages_RejectionReasons_Delete, L("DeleteRejectionReason"));
            rejectionReasons.CreateChildPermission(AppPermissions.Pages_RejectionReasons_ViewMenu, L("ViewMenuRejectionReason"));

            var registrationJsonDatas = pages.CreateChildPermission(AppPermissions.Pages_RegistrationJsonDatas, L("RegistrationJsonDatas"));
            registrationJsonDatas.CreateChildPermission(AppPermissions.Pages_RegistrationJsonDatas_Create, L("CreateNewRegistrationJsonData"));
            registrationJsonDatas.CreateChildPermission(AppPermissions.Pages_RegistrationJsonDatas_Edit, L("EditRegistrationJsonData"));
            registrationJsonDatas.CreateChildPermission(AppPermissions.Pages_RegistrationJsonDatas_Delete, L("DeleteRegistrationJsonData"));
            registrationJsonDatas.CreateChildPermission(AppPermissions.Pages_RegistrationJsonDatas_ViewMenu, L("ViewMenuRegistrationJsonData"));

            var purchaseRegistrationFormFields = pages.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFormFields, L("PurchaseRegistrationFormFields"));
            purchaseRegistrationFormFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFormFields_Create, L("CreateNewPurchaseRegistrationFormField"));
            purchaseRegistrationFormFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFormFields_Edit, L("EditPurchaseRegistrationFormField"));
            purchaseRegistrationFormFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFormFields_Delete, L("DeletePurchaseRegistrationFormField"));
            purchaseRegistrationFormFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFormFields_ViewMenu, L("ViewMenuPurchaseRegistrationFormField"));

            var campaignMessages = pages.CreateChildPermission(AppPermissions.Pages_CampaignMessages, L("CampaignMessages"));
            campaignMessages.CreateChildPermission(AppPermissions.Pages_CampaignMessages_Create, L("CreateNewCampaignMessage"));
            campaignMessages.CreateChildPermission(AppPermissions.Pages_CampaignMessages_Edit, L("EditCampaignMessage"));
            campaignMessages.CreateChildPermission(AppPermissions.Pages_CampaignMessages_Delete, L("DeleteCampaignMessage"));
            campaignMessages.CreateChildPermission(AppPermissions.Pages_CampaignMessages_ViewMenu, L("ViewMenuCampaignMessage"));

            var purchaseRegistrationFieldDatas = pages.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFieldDatas, L("PurchaseRegistrationFieldDatas"));
            purchaseRegistrationFieldDatas.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Create, L("CreateNewPurchaseRegistrationFieldData"));
            purchaseRegistrationFieldDatas.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Edit, L("EditPurchaseRegistrationFieldData"));
            purchaseRegistrationFieldDatas.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Delete, L("DeletePurchaseRegistrationFieldData"));
            purchaseRegistrationFieldDatas.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFieldDatas_ViewMenu, L("ViewMenuPurchaseRegistrationFieldData"));

            var purchaseRegistrationFields = pages.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFields, L("PurchaseRegistrationFields"));
            purchaseRegistrationFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFields_Create, L("CreateNewPurchaseRegistrationField"));
            purchaseRegistrationFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFields_Edit, L("EditPurchaseRegistrationField"));
            purchaseRegistrationFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFields_Delete, L("DeletePurchaseRegistrationField"));
            purchaseRegistrationFields.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrationFields_ViewMenu, L("ViewMenuPurchaseRegistrationField"));

            var registrationFormFieldDatas = pages.CreateChildPermission(AppPermissions.Pages_RegistrationFormFieldDatas, L("RegistrationFormFieldDatas"));
            registrationFormFieldDatas.CreateChildPermission(AppPermissions.Pages_RegistrationFormFieldDatas_Create, L("CreateNewRegistrationFormFieldData"));
            registrationFormFieldDatas.CreateChildPermission(AppPermissions.Pages_RegistrationFormFieldDatas_Edit, L("EditRegistrationFormFieldData"));
            registrationFormFieldDatas.CreateChildPermission(AppPermissions.Pages_RegistrationFormFieldDatas_Delete, L("DeleteRegistrationFormFieldData"));
            registrationFormFieldDatas.CreateChildPermission(AppPermissions.Pages_RegistrationFormFieldDatas_ViewMenu, L("ViewMenuRegistrationFormFieldData"));

            var registrationFormFields = pages.CreateChildPermission(AppPermissions.Pages_RegistrationFormFields, L("RegistrationFormFields"));
            registrationFormFields.CreateChildPermission(AppPermissions.Pages_RegistrationFormFields_Create, L("CreateNewRegistrationFormField"));
            registrationFormFields.CreateChildPermission(AppPermissions.Pages_RegistrationFormFields_Edit, L("EditRegistrationFormField"));
            registrationFormFields.CreateChildPermission(AppPermissions.Pages_RegistrationFormFields_Delete, L("DeleteRegistrationFormField"));
            registrationFormFields.CreateChildPermission(AppPermissions.Pages_RegistrationFormFields_ViewMenu, L("ViewMenuRegistrationFormField"));

            var purchaseRegistrations = pages.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrations, L("PurchaseRegistrations"));
            purchaseRegistrations.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrations_Create, L("CreateNewPurchaseRegistration"));
            purchaseRegistrations.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrations_Edit, L("EditPurchaseRegistration"));
            purchaseRegistrations.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrations_Delete, L("DeletePurchaseRegistration"));
            purchaseRegistrations.CreateChildPermission(AppPermissions.Pages_PurchaseRegistrations_ViewMenu, L("ViewMenuPurchaseRegistration"));

            var activationCodeRegistrations = pages.CreateChildPermission(AppPermissions.Pages_ActivationCodeRegistrations, L("ActivationCodeRegistrations"));
            activationCodeRegistrations.CreateChildPermission(AppPermissions.Pages_ActivationCodeRegistrations_Create, L("CreateNewActivationCodeRegistration"));
            activationCodeRegistrations.CreateChildPermission(AppPermissions.Pages_ActivationCodeRegistrations_Edit, L("EditActivationCodeRegistration"));
            activationCodeRegistrations.CreateChildPermission(AppPermissions.Pages_ActivationCodeRegistrations_Delete, L("DeleteActivationCodeRegistration"));
            activationCodeRegistrations.CreateChildPermission(AppPermissions.Pages_ActivationCodeRegistrations_ViewMenu, L("ViewMenuActivationCodeRegistration"));

            var registrations = pages.CreateChildPermission(AppPermissions.Pages_Registrations, L("Registrations"));
            registrations.CreateChildPermission(AppPermissions.Pages_Registrations_Create, L("CreateNewRegistration"));
            registrations.CreateChildPermission(AppPermissions.Pages_Registrations_Edit, L("EditRegistration"));
            registrations.CreateChildPermission(AppPermissions.Pages_Registrations_EditAll, L("EditAllRegistration"));
            registrations.CreateChildPermission(AppPermissions.Pages_Registrations_Delete, L("DeleteRegistration"));
            registrations.CreateChildPermission(AppPermissions.Pages_Registrations_ViewMenu, L("ViewMenuRegistration"));

            var activationCodes = pages.CreateChildPermission(AppPermissions.Pages_ActivationCodes, L("ActivationCodes"));
            activationCodes.CreateChildPermission(AppPermissions.Pages_ActivationCodes_Create, L("CreateNewActivationCode"));
            activationCodes.CreateChildPermission(AppPermissions.Pages_ActivationCodes_Edit, L("EditActivationCode"));
            activationCodes.CreateChildPermission(AppPermissions.Pages_ActivationCodes_Delete, L("DeleteActivationCode"));
            activationCodes.CreateChildPermission(AppPermissions.Pages_ActivationCodes_ViewMenu, L("ViewMenuActivationCode"));

            var campaignRetailerLocations = pages.CreateChildPermission(AppPermissions.Pages_CampaignRetailerLocations, L("CampaignRetailerLocations"));
            campaignRetailerLocations.CreateChildPermission(AppPermissions.Pages_CampaignRetailerLocations_Create, L("CreateNewCampaignRetailerLocation"));
            campaignRetailerLocations.CreateChildPermission(AppPermissions.Pages_CampaignRetailerLocations_Edit, L("EditCampaignRetailerLocation"));
            campaignRetailerLocations.CreateChildPermission(AppPermissions.Pages_CampaignRetailerLocations_Delete, L("DeleteCampaignRetailerLocation"));
            campaignRetailerLocations.CreateChildPermission(AppPermissions.Pages_CampaignRetailerLocations_ViewMenu, L("ViewMenuCampaignRetailerLocation"));

            var handlingLineLocales = pages.CreateChildPermission(AppPermissions.Pages_HandlingLineLocales, L("HandlingLineLocales"));
            handlingLineLocales.CreateChildPermission(AppPermissions.Pages_HandlingLineLocales_Create, L("CreateNewHandlingLineLocale"));
            handlingLineLocales.CreateChildPermission(AppPermissions.Pages_HandlingLineLocales_Edit, L("EditHandlingLineLocale"));
            handlingLineLocales.CreateChildPermission(AppPermissions.Pages_HandlingLineLocales_Delete, L("DeleteHandlingLineLocale"));
            handlingLineLocales.CreateChildPermission(AppPermissions.Pages_HandlingLineLocales_ViewMenu, L("ViewMenuHandlingLineLocale"));

            var handlingLineLogics = pages.CreateChildPermission(AppPermissions.Pages_HandlingLineLogics, L("HandlingLineLogics"));
            handlingLineLogics.CreateChildPermission(AppPermissions.Pages_HandlingLineLogics_Create, L("CreateNewHandlingLineLogic"));
            handlingLineLogics.CreateChildPermission(AppPermissions.Pages_HandlingLineLogics_Edit, L("EditHandlingLineLogic"));
            handlingLineLogics.CreateChildPermission(AppPermissions.Pages_HandlingLineLogics_Delete, L("DeleteHandlingLineLogic"));
            handlingLineLogics.CreateChildPermission(AppPermissions.Pages_HandlingLineLogics_ViewMenu, L("ViewMenuHandlingLineLogic"));

            var handlingLineRetailers = pages.CreateChildPermission(AppPermissions.Pages_HandlingLineRetailers, L("HandlingLineRetailers"));
            handlingLineRetailers.CreateChildPermission(AppPermissions.Pages_HandlingLineRetailers_Create, L("CreateNewHandlingLineRetailer"));
            handlingLineRetailers.CreateChildPermission(AppPermissions.Pages_HandlingLineRetailers_Edit, L("EditHandlingLineRetailer"));
            handlingLineRetailers.CreateChildPermission(AppPermissions.Pages_HandlingLineRetailers_Delete, L("DeleteHandlingLineRetailer"));
            handlingLineRetailers.CreateChildPermission(AppPermissions.Pages_HandlingLineRetailers_ViewMenu, L("ViewMenuHandlingLineRetailer"));

            var handlingLineProducts = pages.CreateChildPermission(AppPermissions.Pages_HandlingLineProducts, L("HandlingLineProducts"));
            handlingLineProducts.CreateChildPermission(AppPermissions.Pages_HandlingLineProducts_Create, L("CreateNewHandlingLineProduct"));
            handlingLineProducts.CreateChildPermission(AppPermissions.Pages_HandlingLineProducts_Edit, L("EditHandlingLineProduct"));
            handlingLineProducts.CreateChildPermission(AppPermissions.Pages_HandlingLineProducts_Delete, L("DeleteHandlingLineProduct"));
            handlingLineProducts.CreateChildPermission(AppPermissions.Pages_HandlingLineProducts_ViewMenu, L("ViewMenuHandlingLineProduct"));

            var retailerLocations = pages.CreateChildPermission(AppPermissions.Pages_RetailerLocations, L("RetailerLocations"));
            retailerLocations.CreateChildPermission(AppPermissions.Pages_RetailerLocations_Create, L("CreateNewRetailerLocation"));
            retailerLocations.CreateChildPermission(AppPermissions.Pages_RetailerLocations_Edit, L("EditRetailerLocation"));
            retailerLocations.CreateChildPermission(AppPermissions.Pages_RetailerLocations_Delete, L("DeleteRetailerLocation"));
            retailerLocations.CreateChildPermission(AppPermissions.Pages_RetailerLocations_ViewMenu, L("ViewMenuRetailerLocation"));

            var handlingLines = pages.CreateChildPermission(AppPermissions.Pages_HandlingLines, L("HandlingLines"));
            handlingLines.CreateChildPermission(AppPermissions.Pages_HandlingLines_Create, L("CreateNewHandlingLine"));
            handlingLines.CreateChildPermission(AppPermissions.Pages_HandlingLines_Edit, L("EditHandlingLine"));
            handlingLines.CreateChildPermission(AppPermissions.Pages_HandlingLines_Delete, L("DeleteHandlingLine"));
            handlingLines.CreateChildPermission(AppPermissions.Pages_HandlingLines_ViewMenu, L("ViewMenuHandlingLine"));

            var productHandlings = pages.CreateChildPermission(AppPermissions.Pages_ProductHandlings, L("ProductHandlings"));
            productHandlings.CreateChildPermission(AppPermissions.Pages_ProductHandlings_Create, L("CreateNewProductHandling"));
            productHandlings.CreateChildPermission(AppPermissions.Pages_ProductHandlings_Edit, L("EditProductHandling"));
            productHandlings.CreateChildPermission(AppPermissions.Pages_ProductHandlings_Delete, L("DeleteProductHandling"));
            productHandlings.CreateChildPermission(AppPermissions.Pages_ProductHandlings_ViewMenu, L("ViewMenuProductHandling"));

            var campaignForms = pages.CreateChildPermission(AppPermissions.Pages_CampaignForms, L("CampaignForms"));
            campaignForms.CreateChildPermission(AppPermissions.Pages_CampaignForms_Create, L("CreateNewCampaignForm"));
            campaignForms.CreateChildPermission(AppPermissions.Pages_CampaignForms_Edit, L("EditCampaignForm"));
            campaignForms.CreateChildPermission(AppPermissions.Pages_CampaignForms_Delete, L("DeleteCampaignForm"));
            campaignForms.CreateChildPermission(AppPermissions.Pages_CampaignForms_ViewMenu, L("ViewMenuCampaignForm"));

            var campaignCampaignTypes = pages.CreateChildPermission(AppPermissions.Pages_CampaignCampaignTypes, L("CampaignCampaignTypes"));
            campaignCampaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignCampaignTypes_Create, L("CreateNewCampaignCampaignType"));
            campaignCampaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignCampaignTypes_Edit, L("EditCampaignCampaignType"));
            campaignCampaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignCampaignTypes_Delete, L("DeleteCampaignCampaignType"));
            campaignCampaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignCampaignTypes_ViewMenu, L("ViewMenuCampaignCampaignType"));

            var campaigns = pages.CreateChildPermission(AppPermissions.Pages_Campaigns, L("Campaigns"));
            campaigns.CreateChildPermission(AppPermissions.Pages_Campaigns_Create, L("CreateNewCampaign"));
            campaigns.CreateChildPermission(AppPermissions.Pages_Campaigns_Edit, L("EditCampaign"));
            campaigns.CreateChildPermission(AppPermissions.Pages_Campaigns_Delete, L("DeleteCampaign"));
            campaigns.CreateChildPermission(AppPermissions.Pages_Campaigns_ViewMenu, L("ViewMenuCampaign"));

            var formFieldTranslations = pages.CreateChildPermission(AppPermissions.Pages_FormFieldTranslations, L("FormFieldTranslations"));
            formFieldTranslations.CreateChildPermission(AppPermissions.Pages_FormFieldTranslations_Create, L("CreateNewFormFieldTranslation"));
            formFieldTranslations.CreateChildPermission(AppPermissions.Pages_FormFieldTranslations_Edit, L("EditFormFieldTranslation"));
            formFieldTranslations.CreateChildPermission(AppPermissions.Pages_FormFieldTranslations_Delete, L("DeleteFormFieldTranslation"));
            formFieldTranslations.CreateChildPermission(AppPermissions.Pages_FormFieldTranslations_ViewMenu, L("ViewMenuFormFieldTranslation"));

            var formFieldValueLists = pages.CreateChildPermission(AppPermissions.Pages_FormFieldValueLists, L("FormFieldValueLists"));
            formFieldValueLists.CreateChildPermission(AppPermissions.Pages_FormFieldValueLists_Create, L("CreateNewFormFieldValueList"));
            formFieldValueLists.CreateChildPermission(AppPermissions.Pages_FormFieldValueLists_Edit, L("EditFormFieldValueList"));
            formFieldValueLists.CreateChildPermission(AppPermissions.Pages_FormFieldValueLists_Delete, L("DeleteFormFieldValueList"));
            formFieldValueLists.CreateChildPermission(AppPermissions.Pages_FormFieldValueLists_ViewMenu, L("ViewMenuFormFieldValueList"));

            var listValueTranslations = pages.CreateChildPermission(AppPermissions.Pages_ListValueTranslations, L("ListValueTranslations"));
            listValueTranslations.CreateChildPermission(AppPermissions.Pages_ListValueTranslations_Create, L("CreateNewListValueTranslation"));
            listValueTranslations.CreateChildPermission(AppPermissions.Pages_ListValueTranslations_Edit, L("EditListValueTranslation"));
            listValueTranslations.CreateChildPermission(AppPermissions.Pages_ListValueTranslations_Delete, L("DeleteListValueTranslation"));
            listValueTranslations.CreateChildPermission(AppPermissions.Pages_ListValueTranslations_ViewMenu, L("ViewMenuListValueTranslation"));

            var listValues = pages.CreateChildPermission(AppPermissions.Pages_ListValues, L("ListValues"));
            listValues.CreateChildPermission(AppPermissions.Pages_ListValues_Create, L("CreateNewListValue"));
            listValues.CreateChildPermission(AppPermissions.Pages_ListValues_Edit, L("EditListValue"));
            listValues.CreateChildPermission(AppPermissions.Pages_ListValues_Delete, L("DeleteListValue"));
            listValues.CreateChildPermission(AppPermissions.Pages_ListValues_ViewMenu, L("ViewMenuListValue"));

            var valueLists = pages.CreateChildPermission(AppPermissions.Pages_ValueLists, L("ValueLists"));
            valueLists.CreateChildPermission(AppPermissions.Pages_ValueLists_Create, L("CreateNewValueList"));
            valueLists.CreateChildPermission(AppPermissions.Pages_ValueLists_Edit, L("EditValueList"));
            valueLists.CreateChildPermission(AppPermissions.Pages_ValueLists_Delete, L("DeleteValueList"));
            valueLists.CreateChildPermission(AppPermissions.Pages_ValueLists_ViewMenu, L("ViewMenuValueList"));

            var formBlockFields = pages.CreateChildPermission(AppPermissions.Pages_FormBlockFields, L("FormBlockFields"));
            formBlockFields.CreateChildPermission(AppPermissions.Pages_FormBlockFields_Create, L("CreateNewFormBlockField"));
            formBlockFields.CreateChildPermission(AppPermissions.Pages_FormBlockFields_Edit, L("EditFormBlockField"));
            formBlockFields.CreateChildPermission(AppPermissions.Pages_FormBlockFields_Delete, L("DeleteFormBlockField"));
            formBlockFields.CreateChildPermission(AppPermissions.Pages_FormBlockFields_ViewMenu, L("ViewMenuFormBlockField"));

            var formFields = pages.CreateChildPermission(AppPermissions.Pages_FormFields, L("FormFields"));
            formFields.CreateChildPermission(AppPermissions.Pages_FormFields_Create, L("CreateNewFormField"));
            formFields.CreateChildPermission(AppPermissions.Pages_FormFields_Edit, L("EditFormField"));
            formFields.CreateChildPermission(AppPermissions.Pages_FormFields_Delete, L("DeleteFormField"));
            formFields.CreateChildPermission(AppPermissions.Pages_FormFields_ViewMenu, L("ViewMenuFormField"));

            var fieldTypes = pages.CreateChildPermission(AppPermissions.Pages_FieldTypes, L("FieldTypes"));
            fieldTypes.CreateChildPermission(AppPermissions.Pages_FieldTypes_Create, L("CreateNewFieldType"));
            fieldTypes.CreateChildPermission(AppPermissions.Pages_FieldTypes_Edit, L("EditFieldType"));
            fieldTypes.CreateChildPermission(AppPermissions.Pages_FieldTypes_Delete, L("DeleteFieldType"));
            fieldTypes.CreateChildPermission(AppPermissions.Pages_FieldTypes_ViewMenu, L("ViewMenuFieldType"));

            var formLocales = pages.CreateChildPermission(AppPermissions.Pages_FormLocales, L("FormLocales"));
            formLocales.CreateChildPermission(AppPermissions.Pages_FormLocales_Create, L("CreateNewFormLocale"));
            formLocales.CreateChildPermission(AppPermissions.Pages_FormLocales_Edit, L("EditFormLocale"));
            formLocales.CreateChildPermission(AppPermissions.Pages_FormLocales_Delete, L("DeleteFormLocale"));
            formLocales.CreateChildPermission(AppPermissions.Pages_FormLocales_ViewMenu, L("ViewMenuFormLocale"));

            var formBlocks = pages.CreateChildPermission(AppPermissions.Pages_FormBlocks, L("FormBlocks"));
            formBlocks.CreateChildPermission(AppPermissions.Pages_FormBlocks_Create, L("CreateNewFormBlock"));
            formBlocks.CreateChildPermission(AppPermissions.Pages_FormBlocks_Edit, L("EditFormBlock"));
            formBlocks.CreateChildPermission(AppPermissions.Pages_FormBlocks_Delete, L("DeleteFormBlock"));
            formBlocks.CreateChildPermission(AppPermissions.Pages_FormBlocks_ViewMenu, L("ViewMenuFormBlock"));

            var forms = pages.CreateChildPermission(AppPermissions.Pages_Forms, L("Forms"));
            forms.CreateChildPermission(AppPermissions.Pages_Forms_Create, L("CreateNewForm"));
            forms.CreateChildPermission(AppPermissions.Pages_Forms_Edit, L("EditForm"));
            forms.CreateChildPermission(AppPermissions.Pages_Forms_Delete, L("DeleteForm"));
            forms.CreateChildPermission(AppPermissions.Pages_Forms_ViewMenu, L("ViewMenuForm"));

            //PromoPlanner
            pages.CreateChildPermission(AppPermissions.Pages_PromoPlanner, L("PromoPlanner"));
            pages.CreateChildPermission(AppPermissions.Pages_PromoPlanner_ViewMenu, L("ViewMenuPromoPlanner"));

            pages.CreateChildPermission(AppPermissions.Pages_PromoDashboard, L("PromoDashboard"));
            pages.CreateChildPermission(AppPermissions.Pages_PromoDashboard_ViewMenu, L("ViewMenuPromoDashboard"));

            pages.CreateChildPermission(AppPermissions.Pages_PromoCalendar, L("PromoCalendar"));
            pages.CreateChildPermission(AppPermissions.Pages_PromoCalendar_ViewMenu, L("ViewMenuPromoCalendar"));

            var promos = pages.CreateChildPermission(AppPermissions.Pages_Promos, L("Promos"));
            promos.CreateChildPermission(AppPermissions.Pages_Promos_Create, L("CreateNewPromo"));
            promos.CreateChildPermission(AppPermissions.Pages_Promos_Edit, L("EditPromo"));
            promos.CreateChildPermission(AppPermissions.Pages_Promos_Delete, L("DeletePromo"));
            promos.CreateChildPermission(AppPermissions.Pages_Promos_ViewMenu, L("ViewMenuPromo"));

            //PromoPlanner other tables (currently not used in Promoplanner menus)
            var promoStepFields = pages.CreateChildPermission(AppPermissions.Pages_PromoStepFields, L("PromoStepFields"));
            promoStepFields.CreateChildPermission(AppPermissions.Pages_PromoStepFields_Create, L("CreateNewPromoStepField"));
            promoStepFields.CreateChildPermission(AppPermissions.Pages_PromoStepFields_Edit, L("EditPromoStepField"));
            promoStepFields.CreateChildPermission(AppPermissions.Pages_PromoStepFields_Delete, L("DeletePromoStepField"));
            promoStepFields.CreateChildPermission(AppPermissions.Pages_PromoStepFields_ViewMenu, L("ViewMenuPromoStepField"));

            var promoSteps = pages.CreateChildPermission(AppPermissions.Pages_PromoSteps, L("PromoSteps"));
            promoSteps.CreateChildPermission(AppPermissions.Pages_PromoSteps_Create, L("CreateNewPromoStep"));
            promoSteps.CreateChildPermission(AppPermissions.Pages_PromoSteps_Edit, L("EditPromoStep"));
            promoSteps.CreateChildPermission(AppPermissions.Pages_PromoSteps_Delete, L("DeletePromoStep"));
            promoSteps.CreateChildPermission(AppPermissions.Pages_PromoSteps_ViewMenu, L("ViewMenuPromoStep"));

            //Promoplanner only used for edits within Promos (Appservices)
            var promoProducts = pages.CreateChildPermission(AppPermissions.Pages_PromoProducts, L("PromoProducts"));
            promoProducts.CreateChildPermission(AppPermissions.Pages_PromoProducts_Create, L("CreateNewPromoProduct"));
            promoProducts.CreateChildPermission(AppPermissions.Pages_PromoProducts_Edit, L("EditPromoProduct"));
            promoProducts.CreateChildPermission(AppPermissions.Pages_PromoProducts_Delete, L("DeletePromoProduct"));
            promoProducts.CreateChildPermission(AppPermissions.Pages_PromoProducts_ViewMenu, L("ViewMenuPromoProduct"));

            var promoRetailers = pages.CreateChildPermission(AppPermissions.Pages_PromoRetailers, L("PromoRetailers"));
            promoRetailers.CreateChildPermission(AppPermissions.Pages_PromoRetailers_Create, L("CreateNewPromoRetailer"));
            promoRetailers.CreateChildPermission(AppPermissions.Pages_PromoRetailers_Edit, L("EditPromoRetailer"));
            promoRetailers.CreateChildPermission(AppPermissions.Pages_PromoRetailers_Delete, L("DeletePromoRetailer"));
            promoRetailers.CreateChildPermission(AppPermissions.Pages_PromoRetailers_ViewMenu, L("ViewMenuPromoRetailer"));

            var promoCountries = pages.CreateChildPermission(AppPermissions.Pages_PromoCountries, L("PromoCountries"));
            promoCountries.CreateChildPermission(AppPermissions.Pages_PromoCountries_Create, L("CreateNewPromoCountry"));
            promoCountries.CreateChildPermission(AppPermissions.Pages_PromoCountries_Edit, L("EditPromoCountry"));
            promoCountries.CreateChildPermission(AppPermissions.Pages_PromoCountries_Delete, L("DeletePromoCountry"));
            promoCountries.CreateChildPermission(AppPermissions.Pages_PromoCountries_ViewMenu, L("ViewMenuPromoCountry"));

            //PromoConfig
            pages.CreateChildPermission(AppPermissions.Pages_PromoConfig, L("PromoConfig"));
            pages.CreateChildPermission(AppPermissions.Pages_PromoConfig_ViewMenu, L("ViewMenuPromoConfig"));

            var promoScopes = pages.CreateChildPermission(AppPermissions.Pages_PromoScopes, L("PromoScopes"));
            promoScopes.CreateChildPermission(AppPermissions.Pages_PromoScopes_Create, L("CreateNewPromoScope"));
            promoScopes.CreateChildPermission(AppPermissions.Pages_PromoScopes_Edit, L("EditPromoScope"));
            promoScopes.CreateChildPermission(AppPermissions.Pages_PromoScopes_Delete, L("DeletePromoScope"));
            promoScopes.CreateChildPermission(AppPermissions.Pages_PromoScopes_ViewMenu, L("ViewMenuPromoScope"));

            var productCategoryYearPOs = pages.CreateChildPermission(AppPermissions.Pages_ProductCategoryYearPos, L("ProductCategoryYearPOs"));
            productCategoryYearPOs.CreateChildPermission(AppPermissions.Pages_ProductCategoryYearPos_Create, L("CreateNewProductCategoryYearPO"));
            productCategoryYearPOs.CreateChildPermission(AppPermissions.Pages_ProductCategoryYearPos_Edit, L("EditProductCategoryYearPO"));
            productCategoryYearPOs.CreateChildPermission(AppPermissions.Pages_ProductCategoryYearPos_Delete, L("DeleteProductCategoryYearPO"));
            productCategoryYearPOs.CreateChildPermission(AppPermissions.Pages_ProductCategoryYearPOs_ViewMenu, L("ViewMenuProductCategoryYearPO"));

            var products = pages.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_ViewMenu, L("ViewMenuProduct"));

            var productCategories = pages.CreateChildPermission(AppPermissions.Pages_ProductCategories, L("ProductCategories"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Create, L("CreateNewProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Edit, L("EditProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Delete, L("DeleteProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_ViewMenu, L("ViewMenuProductCategory"));

            var retailers = pages.CreateChildPermission(AppPermissions.Pages_Retailers, L("Retailers"));
            retailers.CreateChildPermission(AppPermissions.Pages_Retailers_Create, L("CreateNewRetailer"));
            retailers.CreateChildPermission(AppPermissions.Pages_Retailers_Edit, L("EditRetailer"));
            retailers.CreateChildPermission(AppPermissions.Pages_Retailers_Delete, L("DeleteRetailer"));
            retailers.CreateChildPermission(AppPermissions.Pages_Retailers_ViewMenu, L("ViewMenuRetailer"));

            var companies = pages.CreateChildPermission(AppPermissions.Pages_Companies, L("Companies"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Create, L("CreateNewCompany"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Edit, L("EditCompany"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Delete, L("DeleteCompany"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_ViewMenu, L("ViewMenuCompany"));

            var projectManagers = pages.CreateChildPermission(AppPermissions.Pages_ProjectManagers, L("ProjectManagers"));
            projectManagers.CreateChildPermission(AppPermissions.Pages_ProjectManagers_Create, L("CreateNewProjectManager"));
            projectManagers.CreateChildPermission(AppPermissions.Pages_ProjectManagers_Edit, L("EditProjectManager"));
            projectManagers.CreateChildPermission(AppPermissions.Pages_ProjectManagers_Delete, L("DeleteProjectManager"));
            projectManagers.CreateChildPermission(AppPermissions.Pages_ProjectManagers_ViewMenu, L("ViewMenuProjectManager"));

            var addresses = pages.CreateChildPermission(AppPermissions.Pages_Addresses, L("Addresses"));
            addresses.CreateChildPermission(AppPermissions.Pages_Addresses_Create, L("CreateNewAddress"));
            addresses.CreateChildPermission(AppPermissions.Pages_Addresses_Edit, L("EditAddress"));
            addresses.CreateChildPermission(AppPermissions.Pages_Addresses_Delete, L("DeleteAddress"));
            addresses.CreateChildPermission(AppPermissions.Pages_Addresses_ViewMenu, L("ViewMenuAddress"));

            var messageVariables = pages.CreateChildPermission(AppPermissions.Pages_MessageVariables, L("MessageVariables"));
            messageVariables.CreateChildPermission(AppPermissions.Pages_MessageVariables_Create, L("CreateNewMessageVariable"));
            messageVariables.CreateChildPermission(AppPermissions.Pages_MessageVariables_Edit, L("EditMessageVariable"));
            messageVariables.CreateChildPermission(AppPermissions.Pages_MessageVariables_Delete, L("DeleteMessageVariable"));
            messageVariables.CreateChildPermission(AppPermissions.Pages_MessageVariables_ViewMenu, L("ViewMenuMessageVariable"));

            var messageContentTranslations = pages.CreateChildPermission(AppPermissions.Pages_MessageContentTranslations, L("MessageContentTranslations"));
            messageContentTranslations.CreateChildPermission(AppPermissions.Pages_MessageContentTranslations_Create, L("CreateNewMessageContentTranslation"));
            messageContentTranslations.CreateChildPermission(AppPermissions.Pages_MessageContentTranslations_Edit, L("EditMessageContentTranslation"));
            messageContentTranslations.CreateChildPermission(AppPermissions.Pages_MessageContentTranslations_Delete, L("DeleteMessageContentTranslation"));
            messageContentTranslations.CreateChildPermission(AppPermissions.Pages_MessageContentTranslations_ViewMenu, L("ViewMenuMessageContentTranslation"));

            var messageComponentContents = pages.CreateChildPermission(AppPermissions.Pages_MessageComponentContents, L("MessageComponentContents"));
            messageComponentContents.CreateChildPermission(AppPermissions.Pages_MessageComponentContents_Create, L("CreateNewMessageComponentContent"));
            messageComponentContents.CreateChildPermission(AppPermissions.Pages_MessageComponentContents_Edit, L("EditMessageComponentContent"));
            messageComponentContents.CreateChildPermission(AppPermissions.Pages_MessageComponentContents_Delete, L("DeleteMessageComponentContent"));
            messageComponentContents.CreateChildPermission(AppPermissions.Pages_MessageComponentContents_ViewMenu, L("ViewMenuMessageComponentContent"));

            var messageComponents = pages.CreateChildPermission(AppPermissions.Pages_MessageComponents, L("MessageComponents"));
            messageComponents.CreateChildPermission(AppPermissions.Pages_MessageComponents_Create, L("CreateNewMessageComponent"));
            messageComponents.CreateChildPermission(AppPermissions.Pages_MessageComponents_Edit, L("EditMessageComponent"));
            messageComponents.CreateChildPermission(AppPermissions.Pages_MessageComponents_Delete, L("DeleteMessageComponent"));
            messageComponents.CreateChildPermission(AppPermissions.Pages_MessageComponents_ViewMenu, L("ViewMenuMessageComponent"));

            var messageTypes = pages.CreateChildPermission(AppPermissions.Pages_MessageTypes, L("MessageTypes"));
            messageTypes.CreateChildPermission(AppPermissions.Pages_MessageTypes_Create, L("CreateNewMessageType"));
            messageTypes.CreateChildPermission(AppPermissions.Pages_MessageTypes_Edit, L("EditMessageType"));
            messageTypes.CreateChildPermission(AppPermissions.Pages_MessageTypes_Delete, L("DeleteMessageType"));
            messageTypes.CreateChildPermission(AppPermissions.Pages_MessageTypes_ViewMenu, L("ViewMenuMessageType"));

            var messages = pages.CreateChildPermission(AppPermissions.Pages_Messages, L("Messages"));
            messages.CreateChildPermission(AppPermissions.Pages_Messages_Create, L("CreateNewMessage"));
            messages.CreateChildPermission(AppPermissions.Pages_Messages_Edit, L("EditMessage"));
            messages.CreateChildPermission(AppPermissions.Pages_Messages_Delete, L("DeleteMessage"));
            messages.CreateChildPermission(AppPermissions.Pages_Messages_ViewMenu, L("ViewMenuMessage"));

            var systemLevels = pages.CreateChildPermission(AppPermissions.Pages_SystemLevels, L("SystemLevels"));
            systemLevels.CreateChildPermission(AppPermissions.Pages_SystemLevels_Create, L("CreateNewSystemLevel"));
            systemLevels.CreateChildPermission(AppPermissions.Pages_SystemLevels_Edit, L("EditSystemLevel"));
            systemLevels.CreateChildPermission(AppPermissions.Pages_SystemLevels_Delete, L("DeleteSystemLevel"));
            systemLevels.CreateChildPermission(AppPermissions.Pages_SystemLevels_ViewMenu, L("ViewMenuSystemLevel"));

            var campaignTypeEventRegistrationStatuses = pages.CreateChildPermission(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses, L("CampaignTypeEventRegistrationStatuses"));
            campaignTypeEventRegistrationStatuses.CreateChildPermission(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Create, L("CreateNewCampaignTypeEventRegistrationStatus"));
            campaignTypeEventRegistrationStatuses.CreateChildPermission(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit, L("EditCampaignTypeEventRegistrationStatus"));
            campaignTypeEventRegistrationStatuses.CreateChildPermission(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Delete, L("DeleteCampaignTypeEventRegistrationStatus"));
            campaignTypeEventRegistrationStatuses.CreateChildPermission(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_ViewMenu, L("ViewMenuCampaignTypeEventRegistrationStatus"));

            var campaignTypeEvents = pages.CreateChildPermission(AppPermissions.Pages_CampaignTypeEvents, L("CampaignTypeEvents"));
            campaignTypeEvents.CreateChildPermission(AppPermissions.Pages_CampaignTypeEvents_Create, L("CreateNewCampaignTypeEvent"));
            campaignTypeEvents.CreateChildPermission(AppPermissions.Pages_CampaignTypeEvents_Edit, L("EditCampaignTypeEvent"));
            campaignTypeEvents.CreateChildPermission(AppPermissions.Pages_CampaignTypeEvents_Delete, L("DeleteCampaignTypeEvent"));
            campaignTypeEvents.CreateChildPermission(AppPermissions.Pages_CampaignTypeEvents_ViewMenu, L("ViewMenuCampaignTypeEvent"));

            var campaignCategoryTranslations = pages.CreateChildPermission(AppPermissions.Pages_CampaignCategoryTranslations, L("CampaignCategoryTranslations"));
            campaignCategoryTranslations.CreateChildPermission(AppPermissions.Pages_CampaignCategoryTranslations_Create, L("CreateNewCampaignCategoryTranslation"));
            campaignCategoryTranslations.CreateChildPermission(AppPermissions.Pages_CampaignCategoryTranslations_Edit, L("EditCampaignCategoryTranslation"));
            campaignCategoryTranslations.CreateChildPermission(AppPermissions.Pages_CampaignCategoryTranslations_Delete, L("DeleteCampaignCategoryTranslation"));
            campaignCategoryTranslations.CreateChildPermission(AppPermissions.Pages_CampaignCategoryTranslations_ViewMenu, L("ViewMenuCampaignCategoryTranslation"));

            var campaignCategories = pages.CreateChildPermission(AppPermissions.Pages_CampaignCategories, L("CampaignCategories"));
            campaignCategories.CreateChildPermission(AppPermissions.Pages_CampaignCategories_Create, L("CreateNewCampaignCategory"));
            campaignCategories.CreateChildPermission(AppPermissions.Pages_CampaignCategories_Edit, L("EditCampaignCategory"));
            campaignCategories.CreateChildPermission(AppPermissions.Pages_CampaignCategories_Delete, L("DeleteCampaignCategory"));
            campaignCategories.CreateChildPermission(AppPermissions.Pages_CampaignCategories_ViewMenu, L("ViewMenuCampaignCategory"));

            var messageComponentTypes = pages.CreateChildPermission(AppPermissions.Pages_MessageComponentTypes, L("MessageComponentTypes"));
            messageComponentTypes.CreateChildPermission(AppPermissions.Pages_MessageComponentTypes_Create, L("CreateNewMessageComponentType"));
            messageComponentTypes.CreateChildPermission(AppPermissions.Pages_MessageComponentTypes_Edit, L("EditMessageComponentType"));
            messageComponentTypes.CreateChildPermission(AppPermissions.Pages_MessageComponentTypes_Delete, L("DeleteMessageComponentType"));
            messageComponentTypes.CreateChildPermission(AppPermissions.Pages_MessageComponentTypes_ViewMenu, L("ViewMenuMessageComponentType"));

            var registrationStatuses = pages.CreateChildPermission(AppPermissions.Pages_RegistrationStatuses, L("RegistrationStatuses"));
            registrationStatuses.CreateChildPermission(AppPermissions.Pages_RegistrationStatuses_Create, L("CreateNewRegistrationStatus"));
            registrationStatuses.CreateChildPermission(AppPermissions.Pages_RegistrationStatuses_Edit, L("EditRegistrationStatus"));
            registrationStatuses.CreateChildPermission(AppPermissions.Pages_RegistrationStatuses_Delete, L("DeleteRegistrationStatus"));
            registrationStatuses.CreateChildPermission(AppPermissions.Pages_RegistrationStatuses_ViewMenu, L("ViewMenuRegistrationStatus"));

            var processEvents = pages.CreateChildPermission(AppPermissions.Pages_ProcessEvents, L("ProcessEvents"));
            processEvents.CreateChildPermission(AppPermissions.Pages_ProcessEvents_Create, L("CreateNewProcessEvent"));
            processEvents.CreateChildPermission(AppPermissions.Pages_ProcessEvents_Edit, L("EditProcessEvent"));
            processEvents.CreateChildPermission(AppPermissions.Pages_ProcessEvents_Delete, L("DeleteProcessEvent"));
            processEvents.CreateChildPermission(AppPermissions.Pages_ProcessEvents_ViewMenu, L("ViewMenuProcessEvent"));

            var campaignTypes = pages.CreateChildPermission(AppPermissions.Pages_CampaignTypes, L("CampaignTypes"));
            campaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignTypes_Create, L("CreateNewCampaignType"));
            campaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignTypes_Edit, L("EditCampaignType"));
            campaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignTypes_Delete, L("DeleteCampaignType"));
            campaignTypes.CreateChildPermission(AppPermissions.Pages_CampaignTypes_ViewMenu, L("ViewMenuCampaignType"));

            var locales = pages.CreateChildPermission(AppPermissions.Pages_Locales, L("Locales"));
            locales.CreateChildPermission(AppPermissions.Pages_Locales_Create, L("CreateNewLocale"));
            locales.CreateChildPermission(AppPermissions.Pages_Locales_Edit, L("EditLocale"));
            locales.CreateChildPermission(AppPermissions.Pages_Locales_Delete, L("DeleteLocale"));
            locales.CreateChildPermission(AppPermissions.Pages_Locales_ViewMenu, L("ViewMenuLocale"));

            var countries = pages.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_ViewMenu, L("ViewMenuCountry"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));
            administration.CreateChildPermission(AppPermissions.Pages_Administration_ViewMenu, L("ViewMenuAdministration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ViewMenu, L("ViewMenuUsers"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //Reports
            var reports = pages.CreateChildPermission(AppPermissions.Pages_Reports, L("Reports"));
            reports.CreateChildPermission(AppPermissions.Pages_EmployeePerformanceReport, L("EmployeePerformanceReport"));
            reports.CreateChildPermission(AppPermissions.Pages_GeneralReport, L("GeneralReport"));
            reports.CreateChildPermission(AppPermissions.Pages_CostReport, L("CostReport"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RMSConsts.LocalizationSourceName);
        }
    }
}