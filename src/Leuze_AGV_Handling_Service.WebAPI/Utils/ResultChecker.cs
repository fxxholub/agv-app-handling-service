using System.Text.Json;
using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.WebAPI.Utils;

public static class ResultChecker<T>
{
    public static string ErrorMessageJson(Result<T> result)
    {
        string errorJson = "";
        if (!result.IsSuccess)
        {
            // Prepare the JSON structure
            var errorPayload = new
            {
                Status = result.Status.ToString(),
                Errors = result.Errors
            };

            // Serialize to JSON
            errorJson = JsonSerializer.Serialize(errorPayload);
        }

        return errorJson;
    }
}