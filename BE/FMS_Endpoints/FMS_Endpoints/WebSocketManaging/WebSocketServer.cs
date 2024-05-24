using Fleck;

public class WebSocketServer
{
    private static List<IWebSocketConnection> _allSockets = new List<IWebSocketConnection>();
    private static Fleck.WebSocketServer _server;

    public static void Start()
    {
        _server = new Fleck.WebSocketServer("ws://0.0.0.0:8181");
        _server.Start(socket =>
        {
            socket.OnOpen = () => _allSockets.Add(socket);
            socket.OnClose = () => _allSockets.Remove(socket);
            socket.OnMessage = message =>
            {
            };
        });

        Console.WriteLine("WebSocket server started at ws://0.0.0.0:8181");
    }

    public static void Broadcast(string message)
    {
        foreach (var socket in _allSockets)
        {
            socket.Send(message);
        }
    }
}
