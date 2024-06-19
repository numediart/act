using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _Scripts._Version_1._0.Managers.Network.WebSocket
{
    public sealed class WebsocketManager
    {
        public ClientWebSocket _clientWebSocket;
        private static WebsocketManager _instance;
        private static readonly EventManager _eventManager = EventManager.Instance;

        public static WebsocketManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WebsocketManager();
                }

                return _instance;
            }
        }

        private WebsocketManager()
        {
            _clientWebSocket = new ClientWebSocket();
            ClientWebSocketOptions options = _clientWebSocket.Options;
        }

        public async Task Connect(string uri)
        {
            await _clientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            _ = Receive();
        }

        private SemaphoreSlim sendSemaphore = new SemaphoreSlim(1);

        public async Task Send(string message)
        {
            await sendSemaphore.WaitAsync();
            try
            {
                var buffer = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(message));
                await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            finally
            {
                sendSemaphore.Release();
            }
        }

        public async Task Receive()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                var bufferSize = 4096; // Set an appropriate buffer size
                var buffer = new ArraySegment<byte>(new byte[bufferSize]);

                while (_clientWebSocket.State == WebSocketState.Open)
                {
                    var result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await Close();
                        return;
                    }

                    // Append the received bytes to the StringBuilder
                    stringBuilder.Append(Encoding.UTF8.GetString(buffer.Array, 0, result.Count));

                    // Check if the message is complete
                    if (result.EndOfMessage)
                    {
                        var rawMessage = stringBuilder.ToString();
                        var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MsgFormat>(rawMessage);

                        // Emit the event on the UI thread
                    
                            _eventManager.Emit(message.EventName, message.Data);

                        // Reset the StringBuilder for the next message
                        stringBuilder.Clear();
                    }
                }
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                Console.WriteLine("Connection closed unexpectedly.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Close()
        {
            _clientWebSocket.Abort();
            WebsocketManager._instance = null;
        }
    }
}