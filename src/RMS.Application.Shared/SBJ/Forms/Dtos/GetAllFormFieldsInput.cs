using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
	public class GetAllFormFieldsInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string LabelFilter { get; set; }

		public string DefaultValueFilter { get; set; }

		public int? MaxMaxLengthFilter { get; set; }

		public int? MinMaxLengthFilter { get; set; }

		public int? RequiredFilter { get; set; }

		public int? ReadOnlyFilter { get; set; }

		public string InputMaskFilter { get; set; }

		public string RegularExpressionFilter { get; set; }

		public string ValidationApiCallFilter { get; set; }

		public string RegistrationFieldFilter { get; set; }

		public string PurchaseRegistrationFieldFilter { get; set; }

		public int? IsPurchaseRegistrationFilter { get; set; }

		public string FieldTypeDescriptionFilter { get; set; }
	}
}