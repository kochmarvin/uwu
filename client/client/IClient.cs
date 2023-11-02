namespace netlib.client;

internal interface IClient
{
    Task SendMessage(string message);
}