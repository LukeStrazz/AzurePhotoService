using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace APSApp.Controllers;

public class ApiKeyServiceClientCredentials : ServiceClientCredentials
{
    private readonly string _subscriptionKey;

    public ApiKeyServiceClientCredentials(string subscriptionKey)
    {
        _subscriptionKey = subscriptionKey;
    }

    public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
        await base.ProcessHttpRequestAsync(request, cancellationToken);
    }
}
