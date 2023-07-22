using System;
namespace sail4oxygen.Models
{
	public static class RssHelper
	{
		private  static async Task<string> GetRssFeed(string url)
		{
			string rssFeedString = string.Empty;
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				var repsonse = await (new HttpClient()).SendAsync(request).Result.Content.ReadAsStringAsync();
				rssFeedString = repsonse;
			}
			catch (Exception ex)
			{
				rssFeedString = ex.Message;
			}
			return rssFeedString;
		}

		public static async Task<System.Collections.ObjectModel.ObservableCollection<NewsItem>> GetNewsItems(string rssFeedUrl)
		{
			var rssFeed = await GetRssFeed(rssFeedUrl);

			System.Collections.ObjectModel.ObservableCollection<NewsItem> list = new System.Collections.ObjectModel.ObservableCollection<NewsItem>();
			
			if (IsValidRssFeed(rssFeed))
			{				
				try
				{
					System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(rssFeed);
					var items = doc.Descendants("item");
					foreach (var item in items)
					{
						list.Add(new NewsItem(
							item.Element("title").Value,
							item.Element("description").Value,
							item.Element("link").Value,
							item.Element("pubDate").Value
						));
					}
				
				}
				catch (Exception ex)
				{
					list = null;
				}
			}
			return list;
		}


		//validate the url is a valid rss feed
		private static bool IsValidRssFeed(string rssFeed)
		{
			bool isValid = false;
			try
			{
				if (rssFeed.Contains("<rss version=\"2.0\""))
				{
					isValid = true;
				}
			}
			catch (Exception ex)
			{
				isValid = false;
			}
			return isValid;
		}

	}
}

