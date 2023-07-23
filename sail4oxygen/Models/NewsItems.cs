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
			UpdateNews();
		}

		public ObservableCollection<NewsItem> FirstArticles
		{
			get
			{
				//get first 3 Items from List
				var articles = new ObservableCollection<NewsItem>();
				
				switch (List.Count)
				{
					case 0:
						articles.Add(new NewsItem("Auf See?", "Die neuesten Nachrichten zum Projekt sailing4oxygen konnten nicht geladen werden. Vermutlich hast Du gerade kein Netz.", "https://sail4oxygen.org", DateTime.Now.ToString("d)")));
						break;
					case 1:
						articles.Add(List[0]);
						break;
					case 2:
						articles.Add(List[0]);
						articles.Add(List[1]);
						break;
					default:
						for (int i = 0; i < 3; i++)
						{
							articles.Add(List[i]);
						}
						break;
				}
				foreach (var item in articles)
				{
					_=item.UpdateImage();
				}
				return articles;
			}
		}

		async public void UpdateNews()
		{
			List = await RssHelper.GetNewsItems();	
		}


    }
}

