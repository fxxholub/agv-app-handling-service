using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Services;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.Infrastructure.Messages;

public class ManualMessageChannel(IManualMessageSender senderTarget, IManualMessageReceiver receiverTarget) : MessageChannelBase, IManualMessageChannel
{
    public async Task ReceiveMap(MapDTO map)
    {
        if (IsEnabled())
        {
            await receiverTarget.ReceiveMap(map);
        }
    }

    public async Task SendJoy(JoyDTO joy)
    {
        if (IsEnabled())
        {
            await senderTarget.SendJoy(joy);
        }
    }
}