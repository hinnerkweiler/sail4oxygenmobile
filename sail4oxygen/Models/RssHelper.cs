using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace sail4oxygen.Models
{
	public static class RssHelper
	{
		static string rssUrl = "https://www.trans-ocean.org/DesktopModules/DNNArticle/DNNArticleRSS.aspx?portalid=0&moduleid=426&tabid=226&categoryid=170&cp=True&uid=-1&Language=de-DE";

        private  static async Task<string> GetRssFeed()
		{
            string rssFeedString = string.Empty;

            try
            {
                HttpClient client = new HttpClient();
				client.Timeout = new TimeSpan(0, 0, 5);
                var request = new HttpRequestMessage(HttpMethod.Get, rssUrl);
                rssFeedString = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
				client.Dispose();
			}
			catch (Exception ex)
			{
				rssFeedString = ex.Message;
			}
			return rssFeedString;
		}



		public static async Task<System.Collections.ObjectModel.ObservableCollection<NewsItem>> GetNewsItems()
		{
			var rssFeed = await GetRssFeed();

			System.Collections.ObjectModel.ObservableCollection<NewsItem> list = new System.Collections.ObjectModel.ObservableCollection<NewsItem>();

			if (IsValidRssFeed(rssFeed))
			{				
				try
				{
					System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(rssFeed);
					var items = doc.Descendants("item");
					foreach (var item in items)
                    {						
						var newsItem = new NewsItem(
							title: item.Element("title").Value,
							description: item.Element("description").Value,
							source: item.Element("link").Value,
							datestring: item.Element("pubDate").Value
						);
						list.Add(newsItem);
					}
				
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
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
#if DEBUG
				Console.WriteLine(ex.Message);
#endif
				isValid = false;
			}
			return isValid;
		}



		public async static Task<Uri> GetImagefromProxy(Uri url)
		{
			string webhookUrl = "https://autopilot.funkschiff.com/webhook/296358ad-873b-4329-8be0-4aef2f78f499";

			try
			{
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl);
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6,ru;q=0.4");

                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("url", url.AbsoluteUri));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;

                var response = await client.SendAsync(request);
				var responseString = await response.Content.ReadAsStringAsync();

				var json = JsonConvert.DeserializeObject<JObject>(responseString);
				var imageUrl = json["ogImage"].ToString();
				if (imageUrl != null)
				{
					return new Uri(imageUrl);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;
		}
	}
}


