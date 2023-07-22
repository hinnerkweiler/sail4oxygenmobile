using System;
using CommunityToolkit.Mvvm.ComponentModel;
using static Android.Graphics.ImageDecoder;

namespace sail4oxygen.Models
{
	/// <summary>
	/// Represents a News Item with a Headline, url, imageurl, imagesource and date
	/// </summary>
	public partial class NewsItem : ObservableObject
	{
		[ObservableProperty]
		private string headline;

		[ObservableProperty]
		private string theAbstract;

		[ObservableProperty]
		private Uri url;

		[ObservableProperty]
		private Uri featuredImageUrl;

		[ObservableProperty]
		private DateTime date;

		public string RelativeDateTime
		{
			get
			{
				if (Date != null)
				{
					switch (Date)
					{
						case DateTime date when (DateTime.Now - date).TotalSeconds < 60:
							return Resources.Languages.lang.justnow;

						case DateTime date when (DateTime.Now - date).TotalMinutes < 60:
							return string.Format(Resources.Languages.lang.minutesago,(int)(DateTime.Now - date).TotalMinutes);
							
						case DateTime date when (DateTime.Now - date).TotalHours < 24:
							return string.Format(Resources.Languages.lang.hourssago,(int)(DateTime.Now - date).TotalHours);
							
						case DateTime date when (DateTime.Now - date).TotalDays < 7:
							return string.Format(Resources.Languages.lang.daysago,(int)(DateTime.Now - date).TotalDays);
							
						default:
							return Date.ToString("d");
					}
				}
				return Resources.Languages.lang.NoDate;
			} 					
			

		}
				

		public ImageSource FeaturedImageSource
		{
			get
			{
				if (FeaturedImageUrl != null)
				{
					try
					{
#if DEBUG
						Console.WriteLine($"Reading URL: {FeaturedImageUrl}");
#endif
						return ImageSource.FromUri(FeaturedImageUrl);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Could not Read URL: {FeaturedImageUrl} ==> {ex.Message}");
					}
				}
#if DEBUG
						Console.WriteLine($"Returning Default Image");
#endif
				return ImageSource.FromFile("noimage.png");
            }
		}

        public NewsItem()
		{
		}

		public NewsItem(string title, string description, string source, string datestring)
        {
			this.Headline = title;
			this.TheAbstract = description;
			this.Url = uriFromString(source);
			this.Date = dateTimeFromString(datestring);
        }

        public NewsItem(string title, string description, string source, string image, string datestring)
        {
			this.Headline = title;
			this.TheAbstract = description;
			this.Url = uriFromString(source);
			this.FeaturedImageUrl = uriFromString(image);
			this.Date = dateTimeFromString(datestring);
        }

		private static DateTime dateTimeFromString(string dateTimeString)
		{
			if (!String.IsNullOrEmpty(dateTimeString))
			{
				try
				{
					return DateTime.Parse(dateTimeString);
                }
                catch (Exception ex)
				{
                    Console.WriteLine($"Could convert Sting to DateTime: {dateTimeString} ==> {ex.Message}");
                }
			}
			return DateTime.Now;
		}

		private static Uri uriFromString(string source)
		{
			if (!String.IsNullOrEmpty(source) && source.StartsWith("http"))
			{
				try
				{
					return new Uri(source);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Could vonvert Sting to Url: {source} ==> {ex.Message}");
				}
			}
            return new Uri("https://sail4oxygen.org");
        }
    }
}

