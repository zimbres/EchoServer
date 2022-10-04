using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", (HttpRequest request, HttpContext context) =>
{
    return Handler(request, context);
});


app.MapPost("/", (HttpRequest request, HttpContext context) =>
{
    return Handler(request, context);
});

app.Run();

dynamic Handler(HttpRequest request, HttpContext context)
{
    var sr = new StreamReader(request.Body);
    string body = sr.ReadToEndAsync().Result;
    var response = new
    {
        Hostname = Environment.GetEnvironmentVariable("HOSTNAME"),
        PodInformation = new
        {
            Name = Environment.GetEnvironmentVariable("POD_NAME"),
            Namespace = Environment.GetEnvironmentVariable("POD_NAMESPACE"),
            Ip = Environment.GetEnvironmentVariable("POD_IP"),
            Node = Environment.GetEnvironmentVariable("NODE_NAME")
        },
        Request = new
        {
            ClientAddress = context.Connection.RemoteIpAddress?.ToString(),
            Method = request.Method,
            RealPath = request.Path.Value,
            Query = request.Query,
            RequestVersion = request.Protocol,
            RequestScheme = request.Scheme,
            RequestUri = request.GetDisplayUrl()
        },
        Headers = request.Headers,
        Body = body
    };
    return response;
}
