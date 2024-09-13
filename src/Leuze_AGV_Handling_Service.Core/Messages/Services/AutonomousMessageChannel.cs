using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;

namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

/// <summary>
/// Message channel for the autonomous messages. Wraps the receiver and sender message methods with channel lock.
/// Disabled messages won't make it thru the channel.
/// </summary>
/// <param name="senderTarget">Send messages channel end.</param>
/// <param name="receiverTarget">Receive messages channel end.</param>
public class AutonomousMessageChannel(IAutonomousMessageSender senderTarget, IAutonomousMessageReceiver receiverTarget) : MessageChannelBase, IAutonomousMessageChannel
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