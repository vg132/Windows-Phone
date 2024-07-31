using Microsoft.Phone.Notification;
using PolisenRSS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PolisenRSS.Services
{
	public class PolisenRSSService
	{
		private const string DataFormat = "serviceId=2&&deviceId={0}&action={1}&regions={2}&feeds={3}";
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

			var allRegions = App.ViewModel.Regions.First(item => item.Id == 0);
			var regions = string.Join("|", App.ViewModel.Regions.Where(item => allRegions.IsActive || item.IsActive).Select(item => item.Id));
			var feeds = string.Join("|", App.ViewModel.Feeds.Where(item => item.IsActive).Select(item => item.Code));

			client.PostDataAsyc(PolisenRSSService.Url, string.Format(PolisenRSSService.DataFormat, deviceUrl ?? Settings.ChannelUri, "register", regions, feeds));
		}

		public static void Unregister(string deviceUrl = null)
		{
			var client = new PostClient();
			client.ResponseReceived += (sender, e) =>
			{
				Settings.IsRegisterd = false;
			};
			client.PostDataAsyc(PolisenRSSService.Url, string.Format(PolisenRSSService.DataFormat, deviceUrl ?? Settings.ChannelUri, "unregister", string.Empty, string.Empty));
		}

		public static void Update()
		{
			var client = new PostClient();

			var allRegions = App.ViewModel.Regions.First(item => item.Id == 0);
			var regions = string.Join("|", App.ViewModel.Regions.Where(item => allRegions.IsActive || item.IsActive).Select(item => item.Id));
			var feeds = string.Join("|", App.ViewModel.Feeds.Where(item => item.IsActive).Select(item => item.Code));

			client.PostDataAsyc(PolisenRSSService.Url, string.Format(PolisenRSSService.DataFormat, Settings.ChannelUri, "update", regions, feeds));
		}
	}
}
