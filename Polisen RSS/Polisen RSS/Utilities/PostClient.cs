using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PolisenRSS.Utilities
{
	public class PostClient
	{
		public void PostDataAsyc(string url, string data)
		{
			Url = url;
			Data = data;

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
		}

		private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
		{
			var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
			var postStream = webRequest.EndGetRequestStream(asynchronousResult);
			byte[] byteArray = Encoding.UTF8.GetBytes(Data);
			postStream.Write(byteArray, 0, byteArray.Length);
			postStream.Close();
			webRequest.BeginGetResponse(new AsyncCallback(GetResponseCallback), webRequest);
		}

		private void GetResponseCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
				var webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
				var streamReader = new StreamReader(webResponse.GetResponseStream());
				Response = streamReader.ReadToEnd();
				streamReader.Close();
				webResponse.Close();
				OnResponseReceived(new ResponseReceivedEventArgs { Response = Response });
			}
			catch (WebException ex)
			{
				OnErrorOccurred(new PostClientErrorEventArgs { Message = "Error when getting response from remote server.", Exception = ex });
			}
		}


		#region ErrorOccurred event

		private event EventHandler<PostClientErrorEventArgs> _postClientError;

		public event EventHandler<PostClientErrorEventArgs> ErrorOccurred
		{
			add { _postClientError += value; }
			remove { _postClientError -= value; }
		}

		private void OnErrorOccurred(PostClientErrorEventArgs e)
		{
			EventHandler<PostClientErrorEventArgs> handler = _postClientError;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region ResponseReceived event

		private event EventHandler<ResponseReceivedEventArgs> _response;

		public event EventHandler<ResponseReceivedEventArgs> ResponseReceived
		{
			add { _response += value; }
			remove { _response -= value; }
		}

		private void OnResponseReceived(ResponseReceivedEventArgs e)
		{
			EventHandler<ResponseReceivedEventArgs> handler = _response;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion
				
		public string Url { get; private set; }
		public string Data { get; private set; }
		public string Response { get; private set; }
	}
}
