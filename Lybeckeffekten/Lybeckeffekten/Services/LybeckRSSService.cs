using Microsoft.Phone.Notification;
using Lybeckeffekten.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Lybeckeffekten.Services
{
	public class LybeckRSSService
	{
		private const string DataFormat = "serviceId=4&&deviceId={0}&action={1}";
#if DEBUG
		private const string Url = "http://polisenrssbeta.appspot.com/registration";
#else
		private const string Url = "http://polisenrss.appspot.com/registration";
#endif

		public static void Register(string deviceUrl=null)
		{
			var client = new PostClient();
			client.ResponseReceived += (sender, e) =>
			{
				Settings.IsRegisterd = true;
			};
			client.PostDataAsyc(LybeckRSSService.Url, string.Format(LybeckRSSService.DataFormat, deviceUrl ?? Settings.ChannelUri, "register"));
		}

		public static void Unregister(string deviceUrl = null)
		{
			var client = new PostClient();
			client.ResponseReceived += (sender, e) =>
			{
				Settings.IsRegisterd = false;
			};
			client.PostDataAsyc(LybeckRSSService.Url, string.Format(LybeckRSSService.DataFormat, deviceUrl ?? Settings.ChannelUri, "unregister"));
		}

		public static void Update()
		{
			var client = new PostClient();
			client.PostDataAsyc(LybeckRSSService.Url, string.Format(LybeckRSSService.DataFormat, Settings.ChannelUri, "update"));
		}
	}
}
