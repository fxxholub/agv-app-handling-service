namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IManualMessageChannel : 
        IMessageChannel,
        IManualMessageSender,
        IManualMessageReceiver
{
    
}