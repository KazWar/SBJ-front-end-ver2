using System;
using System.Collections.Generic;
using RMS.SBJ.HandlingBatch.Sepa;

namespace RMS.SBJ.HandlingBatch.Models
{

    public class Payment
    {
        public int Id { get; set; } //End to End Identification; Use RegistrationId
        public decimal Amount { get; set; } //Always in EUR

        public readonly string Currency = "EUR";
        public string BIC { get; set; } //FinInstnId CdtrAgt
        public string IBAN { get; set; }  //CdtrAcct Id

        public string Name { get; set; } //Cdtr Nm
        public string EmailAddress { get; set; } 

        public readonly AddressType2Code AddressType = AddressType2Code.ADDR;
        public string AddressLine { get; set; }  // Streetname + housenumber
        public string PostcalCode { get; set; }
        public string TownName { get; set; }
        public string Country { get; set; }  // ISO 2 CHAR CODE
        public readonly string PurposeCode  = "OTHR";
        public string UnstructuredInfo { get; set; } // PaymentInfo: Company + Reg.Id ; For example "Philips Health - Reg ID: 3023456"


    }
    public class SepaPaymentBatch
    {
        // GroupHeader use and as PmtInfI
        public string MessageId { get; set; }

        //InitgPty use as Nm; only field, also used as Debtor Nm
        public string InitiatorName { get; set; }

        public readonly PaymentMethod3Code PaymentMethod = PaymentMethod3Code.TRF;

        //Payment Type Info
        public readonly Priority2Code InstructionPriority = Priority2Code.NORM;  //Normal
        public readonly ChargeBearerType1Code ServiceLevel = ChargeBearerType1Code.SLEV; //Usage here recommended instead of use ChargeBearer
        public readonly string ServiceLevelCode = "SEPA"; 

        public DateTime RequestedExecutionDate { get; set; }

        public string IBAN { get; set; }  //DbtrAcct Id
        public string BIC { get; set; } //FinInstnId DbtrAcct

        public List<Payment> Payments { get; set; }
    }
}
