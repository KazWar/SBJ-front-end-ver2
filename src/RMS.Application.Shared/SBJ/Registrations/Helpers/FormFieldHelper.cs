using System;
using System.Globalization;
using System.Collections.Generic;
using RMS.SBJ.Registrations.Dtos.ProcessRegistration;

namespace RMS.SBJ.Registrations.Helpers
{
    public static class FormFieldHelper
    {
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string Gender = "Gender";
        public const string ZipCode = "ZipCode";
        public const string StreetName = "StreetName";
        public const string HouseNumber = "HouseNumber";
        public const string Residence = "Residence";
        public const string EmailAddress = "EmailAddress";
        public const string CountryId = "CountryId";
        public const string PhoneNumber = "PhoneNumber";
        public const string CompanyName = "CompanyName";

        public static FormField CloneUIFormField(FormField inputUIFormField)
        {
            var outputUIFormField = new FormField()
            {
                FieldId = inputUIFormField.FieldId,
                FieldTypeId = inputUIFormField.FieldTypeId,
                FieldType = inputUIFormField.FieldType,
                FieldLabel = inputUIFormField.FieldLabel,
                FieldValue = String.Empty,
                FieldListValues = new List<FormFieldListValue>(),
                FieldName = inputUIFormField.FieldName,
                FieldSource = inputUIFormField.FieldSource,
                RegistrationField = inputUIFormField.RegistrationField,
                PurchaseRegistrationField = inputUIFormField.PurchaseRegistrationField,
                FieldLineId = inputUIFormField.FieldLineId,
                IsRejected = inputUIFormField.IsRejected,
                IsReadOnly = inputUIFormField.IsReadOnly 
            };

            return outputUIFormField;
        }

        public static string FormatUIFormField(object fieldValue, string fieldType)
        {
            string fieldValueFormatted;

            switch (fieldType)
            {
                case FieldTypeHelper.InputNumber:
                    fieldValueFormatted = fieldValue.ToString().Replace(',','.');
                    break;
                case FieldTypeHelper.DatePicker:
                    fieldValueFormatted = ((DateTime)fieldValue).ToString("yyyy-MM-dd");
                    break;
                default:
                    fieldValueFormatted = fieldValue.ToString();
                    break;
            }

            return fieldValueFormatted;
        }

        public static object FormatDBFormField(string fieldValue, Type fieldType)
        {
            object fieldValueFormatted;

            //note: switch-case cannot be used here...
            if (fieldType == typeof(int))
            {
                fieldValueFormatted = Convert.ToInt32(fieldValue);
            }
            else if (fieldType == typeof(long))
            {
                fieldValueFormatted = Convert.ToInt64(fieldValue);
            }
            else if (fieldType == typeof(decimal))
            {
                fieldValueFormatted = decimal.Parse(fieldValue, CultureInfo.InvariantCulture);
            }
            else if (fieldType == typeof(DateTime))
            {
                fieldValueFormatted = Convert.ToDateTime(fieldValue);
            }
            else
            {
                fieldValueFormatted = fieldValue;
            }

            return fieldValueFormatted;
        }
    }
}
