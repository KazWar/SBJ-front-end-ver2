namespace RMS.SBJ.Company
{
    public class CompanyConsts
    {

		public const int MinNameLength = 1;
		public const int MaxNameLength = 1000;
						
						
		public const string EmailAddressRegex = @"[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*";
						
		public const string BicCashBackRegex = @"[A-Z0-9]{1,}";
						
		public const string IbanCashBackRegex = @"[A-Z0-9]{1,}";
						
    }
}