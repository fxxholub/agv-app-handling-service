using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Services;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Contexts;

public class AutonomousNodeSendForwarder(
    IAutonomousMessageSender rosSender
    )
    : MessageForwarderBase, IAutonomousMessageSendForwarder
{
    public async Task SendJoy(JoyDTO joy)
    {
        if (IsEnabled())
        {
            await rosSender.SendJoy(joy);
        }
    }
}