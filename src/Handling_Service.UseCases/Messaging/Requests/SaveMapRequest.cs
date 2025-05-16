using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Requests;

public record SaveMapRequest(SaveMapServiceRequest Data) : IRequest<Result<bool>>;