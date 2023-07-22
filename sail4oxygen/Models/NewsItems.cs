using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace sail4oxygen.Models
{
	/// <summary>
	/// List of News from RSS Feed
	/// </summary>
	public partial class NewsItems : ObservableObject
	{
		private string rssfeed;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(FirstArticles))]
		ObservableCollection<NewsItem> list = new ObservableCollection<NewsItem>();

		public NewsItems()
		{
		}

        public NewsItems(string url)
        {
			rssfeed = url;
			UpdateNews(rssfeed);
        }

		public ObservableCollection<NewsItem> FirstArticles
		{
			get
			{
				//get first 3 Items from List
				var articles = new ObservableCollection<NewsItem>();
				
				switch (list.Count)
				{
					case 0:
						articles.Add(new NewsItem("No News", "The News Feed was not loaded", "https://sail4oxygen.org", DateTime.Now.ToString("d)")));
						break;
					case 1:
						articles.Add(list[0]);
						break;
					case 2:
						articles.Add(list[0]);
						articles.Add(list[1]);
						break;
					default:
						for (int i = 0; i < 3; i++)
						{
							articles.Add(list[i]);
						}
						break;
				}
				
				return articles;
			}
		}

		async public void UpdateNews(string rssFeedUrl)
		{
			list = await RssHelper.GetNewsItems(rssFeedUrl);	
		}


    }
}

