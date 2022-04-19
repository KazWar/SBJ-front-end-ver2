using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Messaging.Dtos
{
    public class MessagesDto : EntityDto<int>
    {
        public string Source { get; set; }
        public string Reference { get; set; }
        public string InitiatorReference { get; set; }
        public string MessageInfo { get; set; }
        public int MessageCollectionId { get; set; }
        public int CurrentStepId { get; set; }
        public int TemplateId { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public string Subject { get; set; }
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Body { get; set; }
        public string HeaderImage { get; set; }
        public string HeaderText { get; set; }
        public string GenericText1 { get; set; }
        public string GenericText2 { get; set; }
        public string GenericText3 { get; set; }
        public string GenericText4 { get; set; }
        public string GenericText5 { get; set; }

        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Attachement { get; set; }
        public decimal Priority { get; set; }
        public bool AwaitingSend { get; set; }
        public bool SendError { get; set; }
        public bool Finished { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PreviewFile { get; set; }
    }
}
