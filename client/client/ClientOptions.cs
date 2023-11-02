namespace netlib.client;

public record ClientOptions
{
    public int Port { get; init; }
    public string? Hostname { get; init; }
}