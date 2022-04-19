using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Chat.SignalR
{
    public class SendRMSChatMessageInput
    {
        public int? TenantId { get; set; }

        public List<long> UserId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string Message { get; set; }

        public string GroupName { get; set; }
    }
}
