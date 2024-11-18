using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;

namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

/// <summary>
/// Message channel for the manual messages. Wraps the receiver and sender message methods with channel lock.
/// Disabled messages won't make it thru the channel.
/// </summary>
/// <param name="senderTarget">Send messages channel end.</param>
/// <param name="receiverTarget">Receive messages channel end.</param>
public class ManualMessageChannel(IManualMessageSender senderTarget/*, IManualMessageReceiver receiverTarget*/) : IManualMessageChannel
{
    // public async Task ReceiveMap(MapDto map)
    // {
    //     if (await IsEnabled())
    //     {
    //         await receiverTarget.ReceiveMap(map);
    //     }
    // }

    public async Task SendJoy(JoyDto joy)
    {
        await senderTarget.SendJoy(joy);
    }
}