internal class UnityWebRequestDispatcher : global::UnityEngine.MonoBehaviour
{
	public abstract class Request
	{
		public readonly string Url;

		public bool Successful;

		public string Text;

		public global::System.Net.HttpStatusCode Code;

		public volatile bool Done;

		protected Request(string url)
		{
			Url = url;
		}
	}

	public class GetRequest : UnityWebRequestDispatcher.Request
	{
		public GetRequest(string url)
			: base(url)
		{
		}
	}

	public class PostRequest : UnityWebRequestDispatcher.Request
	{
		public readonly string Data;

		public PostRequest(string url, string data)
			: base(url)
		{
			Data = data;
		}
	}

	private static readonly global::System.Collections.Concurrent.ConcurrentQueue<UnityWebRequestDispatcher.GetRequest> GetQueue = new global::System.Collections.Concurrent.ConcurrentQueue<UnityWebRequestDispatcher.GetRequest>();

	private static readonly global::System.Collections.Concurrent.ConcurrentQueue<UnityWebRequestDispatcher.PostRequest> PostQueue = new global::System.Collections.Concurrent.ConcurrentQueue<UnityWebRequestDispatcher.PostRequest>();

	public static UnityWebRequestDispatcher.GetRequest Get(string url)
	{
		UnityWebRequestDispatcher.GetRequest getRequest = new UnityWebRequestDispatcher.GetRequest(url);
		GetQueue.Enqueue(getRequest);
		return getRequest;
	}

	public static UnityWebRequestDispatcher.PostRequest Post(string url, string data)
	{
		UnityWebRequestDispatcher.PostRequest postRequest = new UnityWebRequestDispatcher.PostRequest(url, data);
		PostQueue.Enqueue(postRequest);
		return postRequest;
	}

	private void Update()
	{
		UnityWebRequestDispatcher.GetRequest result;
		while (GetQueue.TryDequeue(out result))
		{
			global::MEC.Timing.RunCoroutine(ProcessGet(result));
		}
		UnityWebRequestDispatcher.PostRequest result2;
		while (PostQueue.TryDequeue(out result2))
		{
			global::MEC.Timing.RunCoroutine(ProcessPost(result2));
		}
	}

	private static global::System.Collections.Generic.IEnumerator<float> ProcessGet(UnityWebRequestDispatcher.GetRequest request)
	{
		using (global::UnityEngine.Networking.UnityWebRequest uwr = global::UnityEngine.Networking.UnityWebRequest.Get(request.Url))
		{
			yield return global::MEC.Timing.WaitUntilDone(uwr.SendWebRequest());
			request.Successful = !uwr.isNetworkError && !uwr.isHttpError;
			request.Code = (global::System.Net.HttpStatusCode)uwr.responseCode;
			request.Text = (string.IsNullOrEmpty(uwr.error) ? uwr.downloadHandler.text : uwr.error);
			request.Done = true;
		}
	}

	private static global::System.Collections.Generic.IEnumerator<float> ProcessPost(UnityWebRequestDispatcher.PostRequest request)
	{
		using (global::UnityEngine.Networking.UnityWebRequest uwr = global::UnityEngine.Networking.UnityWebRequest.Post(request.Url, HttpQuery.ToUnityForm(request.Data)))
		{
			yield return global::MEC.Timing.WaitUntilDone(uwr.SendWebRequest());
			request.Successful = !uwr.isNetworkError && !uwr.isHttpError;
			request.Code = (global::System.Net.HttpStatusCode)uwr.responseCode;
			request.Text = (string.IsNullOrEmpty(uwr.error) ? uwr.downloadHandler.text : uwr.error);
			request.Done = true;
		}
	}
}
