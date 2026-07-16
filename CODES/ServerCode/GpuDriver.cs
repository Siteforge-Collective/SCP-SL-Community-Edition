public static class GpuDriver
{
	private const string Library = "GpuDriver.dll";

	private const global::System.Runtime.InteropServices.CallingConvention CallingConv = global::System.Runtime.InteropServices.CallingConvention.StdCall;

	private static string _driverVersion;

	private static readonly object _dataLock = new object();

	public static string DriverVersion
	{
		get
		{
			if (!global::System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(global::System.Runtime.InteropServices.OSPlatform.Windows))
			{
				return global::UnityEngine.SystemInfo.graphicsDeviceVersion;
			}
			lock (_dataLock)
			{
				if (!string.IsNullOrWhiteSpace(_driverVersion))
				{
					return _driverVersion;
				}
				try
				{
					string gpuName = global::UnityEngine.SystemInfo.graphicsDeviceName;
					global::System.Threading.Thread thread = new global::System.Threading.Thread((global::System.Threading.ThreadStart)delegate
					{
						lock (_dataLock)
						{
							global::System.IntPtr driverVersion = GetDriverVersion(gpuName);
							if (driverVersion == global::System.IntPtr.Zero)
							{
								global::UnityEngine.Debug.LogWarning("GPU Driver version for " + gpuName + " not found!");
								driverVersion = GetDriverVersion(null);
							}
							_driverVersion = global::System.Runtime.InteropServices.Marshal.PtrToStringUni(driverVersion) ?? "Loading failed";
							Free(driverVersion);
							global::UnityEngine.Debug.Log("GPU Driver version: " + _driverVersion);
							MainThreadDispatcher.Dispatch(delegate
							{
								GpuDriver.DriverLoaded?.Invoke(_driverVersion);
							});
						}
					});
					thread.IsBackground = true;
					thread.SetApartmentState(global::System.Threading.ApartmentState.MTA);
					thread.Start();
					return "Loading...";
				}
				catch (global::System.Exception message)
				{
					global::UnityEngine.Debug.Log(message);
					return global::UnityEngine.SystemInfo.graphicsDeviceVersion;
				}
			}
		}
	}

	public static event global::System.Action<string> DriverLoaded;

	[global::System.Runtime.InteropServices.DllImport("GpuDriver.dll", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.StdCall, EntryPoint = "get_gpu_driver")]
	private static extern global::System.IntPtr GetDriverVersion([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)] string name);

	[global::System.Runtime.InteropServices.DllImport("GpuDriver.dll", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.StdCall, EntryPoint = "free_driver")]
	private static extern void Free(global::System.IntPtr version);
}
