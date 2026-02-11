namespace NewsPortal.Backend.WebApi.Helpers;

public static class UriHelper
{
    public static Uri GetRequestBaseUri(this HttpRequest req)
    {
        var builder = new UriBuilder
        {
            Scheme = req.Scheme,
            Host = req.Host.Host,
            Path = req.Path.Value
        };
        if (req.Host.Port is not null) builder.Port = req.Host.Port.Value;

        return builder.Uri;
    }
}