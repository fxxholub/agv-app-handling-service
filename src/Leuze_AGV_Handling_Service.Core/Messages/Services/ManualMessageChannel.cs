using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;

namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

public class ManualMessageChannel(IManualMessageSender senderTarget, IManualMessageReceiver receiverTarget) : MessageChannelBase, IManualMessageChannel
{
    public async Task ReceiveMap(MapDTO map)
    {
        if (await IsEnabled())
        {
            await receiverTarget.ReceiveMap(map);
        }
    }

    public async Task SendJoy(JoyDTO joy)
    {
        if (await IsEnabled())
        {
            await senderTarget.SendJoy(joy);
        }
    }
}