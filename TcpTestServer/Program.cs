using System.Net;
using System.Net.Sockets;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class TcpTest
{
	public static async Task Main(string[] args)
	{
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
		Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		
		serverSocket.Bind(endPoint);
		serverSocket.Listen(1);
		
		Socket clientSocket = await serverSocket.AcceptAsync();
		
		byte[] receiveBuffer = new byte[1024];
		byte[] sendBuffer = new byte[1024];
		
		int length = await clientSocket.ReceiveAsync(receiveBuffer, SocketFlags.None);
		string rcv = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);

		if (rcv == "" || rcv.Length == 0)
			return;

		if (clientSocket.Connected == false)
			return;

		var response = rcv + " Did you send this ~?";
		Console.WriteLine(response);

		string json = JsonSerializer.Serialize(response);
		sendBuffer = System.Text.Encoding.UTF8.GetBytes(json);
		clientSocket.Send(sendBuffer, 0, json.Length, SocketFlags.None);
		
		clientSocket.Close();
	}
}