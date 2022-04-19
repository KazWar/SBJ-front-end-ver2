using Abp.Application.Services;
using RMS.SBJ.Messaging.Dtos;
using System.Collections.Generic;

namespace RMS.SBJ.Messaging
{
    public interface IMessagingAppService : IApplicationService
    {
        int CreateOrEditAndGetId(MessagesDto input);

        List<MessageStatus> getMessageStatusList(List<long> messageIds);
    }
}
