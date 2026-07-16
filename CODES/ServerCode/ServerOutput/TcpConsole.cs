namespace ServerOutput
{
	public class TcpConsole : global::ServerOutput.IServerOutput, global::System.IDisposable
	{
		private bool _disposing;

		private readonly ushort _port;

		private readonly global::System.Net.Sockets.TcpClient _client;

		private global::System.Net.Sockets.NetworkStream _stream;

		private readonly global::System.Threading.Thread _receiveThread;

		private readonly global::System.Threading.Thread _queueThread;

		private readonly global::System.Collections.Concurrent.ConcurrentQueue<global::ServerOutput.IOutputEntry> _prompterQueue = new global::System.Collections.Concurrent.ConcurrentQueue<global::ServerOutput.IOutputEntry>();

		public TcpConsole(ushort port)
		{
			_client = new global::System.Net.Sockets.TcpClient();
			_receiveThread = new global::System.Threading.Thread(Receive)
			{
				Priority = global::System.Threading.ThreadPriority.Lowest,
				IsBackground = true,
				Name = "Dedicated server console input"
			};
			_queueThread = new global::System.Threading.Thread(Send)
			{
				Priority = global::System.Threading.ThreadPriority.Lowest,
				IsBackground = true,
				Name = "Dedicated server console output"
			};
			_port = port;
		}

		public void Start()
		{
			_client.Connect(new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Loopback, _port));
			_stream = _client.GetStream();
			_queueThread.Start();
			_receiveThread.Start();
		}

		public void Dispose()
		{
			_disposing = true;
			try
			{
				if (_receiveThread.IsAlive)
				{
					_receiveThread.Abort();
				}
			}
			catch
			{
			}
			try
			{
				if (_queueThread.IsAlive)
				{
					_queueThread.Abort();
				}
			}
			catch
			{
			}
			_stream?.Dispose();
			_client.Dispose();
		}

		private void Receive()
		{
			byte[] array = new byte[4];
			while (!_disposing)
			{
				try
				{
					_stream.Read(array, 0, 4);
					int num = global::System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>((global::System.Span<byte>)array)[0];
					byte[] array2 = global::System.Buffers.ArrayPool<byte>.Shared.Rent(num);
					_stream.Read(array2, 0, num);
					string item = Utf8.GetString(array2, 0, num);
					global::System.Buffers.ArrayPool<byte>.Shared.Return(array2);
					ServerConsole.PrompterQueue.Enqueue(item);
				}
				catch (global::System.Exception ex)
				{
					AddLog("[TcpClient] Receive exception: " + ex.Message);
					AddLog("[TcpClient] " + ex.StackTrace);
					global::UnityEngine.Debug.LogException(ex);
				}
			}
		}

		public void AddLog(string text, global::System.ConsoleColor color)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (ServerStatic.IsDedicated)
				{
					_prompterQueue.Enqueue(new global::ServerOutput.TextOutputEntry(text, color));
				}
				else
				{
					global::GameCore.Console.AddLog(text, global::UnityEngine.Color.grey);
				}
			}
		}

		public void AddLog(string text)
		{
			AddLog(text, global::System.ConsoleColor.Gray);
		}

		public void AddOutput(global::ServerOutput.IOutputEntry entry)
		{
			if (ServerStatic.IsDedicated)
			{
				_prompterQueue.Enqueue(entry);
			}
		}

		private void Send()
		{
			while (!_disposing)
			{
				if (_prompterQueue.Count == 0)
				{
					global::System.Threading.Thread.Sleep(25);
					continue;
				}
				try
				{
					if (_prompterQueue.TryDequeue(out var result))
					{
						byte[] buffer = global::System.Buffers.ArrayPool<byte>.Shared.Rent(result.GetBytesLength());
						result.GetBytes(ref buffer, out var length);
						_stream.Write(buffer, 0, length);
						global::System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
					}
				}
				catch (global::System.Exception ex)
				{
					AddLog("[TcpClient] Send exception: " + ex.Message);
					AddLog("[TcpClient] " + ex.StackTrace);
					global::UnityEngine.Debug.LogException(ex);
				}
			}
		}
	}
}
