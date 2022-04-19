using Abp.Domain.Repositories;
using RMS.SBJ.Messaging.Dtos;
using RMS.SBJ.Messaging.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RMS.SBJ.Messaging
{
    public class MessagingAppService : RMSAppServiceBase, IMessagingAppService
    {
        private readonly IRepository<Messages> _messagesRepository;

        public MessagingAppService(IRepository<Messages> messagesRepository)
        {
            _messagesRepository = messagesRepository;
        }

        public int CreateOrEditAndGetId(MessagesDto input)
        {
            var message = ObjectMapper.Map<Messages>(input);
            if (input.Id == 0)
            {
                var messagingId =  _messagesRepository.InsertAndGetId(message);
                return messagingId;
            }
            return default;
        }

        public List<MessageStatus> getMessageStatusList(List<long> messageIds)
        {
            var messageStatusList = new List<MessageStatus>();
            var messages = _messagesRepository.GetAll().Where(m => messageIds.Contains(m.Id));

            foreach (var messageId in messageIds)
            {
                var message = messages.Where(m => m.Id == messageId).FirstOrDefault();
                var messageStatus = MessageStatusHelper.Unknown;

                if (message != null)
                {
                    if (message.Finished && message.CurrentStepId == 5)
                    {
                        messageStatus = MessageStatusHelper.Sent;
                    }
                    else if (message.SendError)
                    {
                        messageStatus = MessageStatusHelper.Failed;
                    }
                    else
                    {
                        messageStatus = MessageStatusHelper.Awaiting;
                    }
                }

                messageStatusList.Add(new MessageStatus { MessageId = messageId, StatusId = messageStatus });
            }

            return messageStatusList;
        }
    }
}
