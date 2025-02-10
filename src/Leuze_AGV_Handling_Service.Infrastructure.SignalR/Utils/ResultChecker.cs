using System.Text.Json;
using Ardalis.Result;
using Microsoft.AspNetCore.SignalR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Utils;

public static class ResultChecker<T>
{
    public static void Check(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            // Prepare the JSON structure
            var errorPayload = new
            {
                Status = result.Status.ToString(),
                Errors = result.Errors.Concat(
                result.ValidationErrors.Select(ve => ve.ErrorMessage)
                )
            };

            // Serialize to JSON
            var errorJson = JsonSerializer.Serialize(errorPayload);

            // Throw HubException with JSON payload
            throw new HubException(errorJson);
        }
    }
}