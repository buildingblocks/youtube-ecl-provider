using System;
using System.Collections.Generic;
using System.Linq;
using Google.GData.YouTube;
using Google.YouTube;

namespace Blocks.Tridion.ECL.YouTube.API
{
    /// <summary>
    /// Client for accessing the YouTube API.
    /// </summary>
    public class YouTubeClient
    {
        private readonly YouTubeRequestSettings _settings;
        private readonly string _userName;

        public string UserToDisplay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTubeClient"/> class.
        /// </summary>
        /// <param name="appName">The YouTube Application Name</param>
        /// <param name="developerKey">The YouTube Developer Key</param>
        /// <param name="userName">The YouTube Username</param>
        /// <param name="password">The YouTube Password.</param>
        public YouTubeClient(string appName, string developerKey, string userName, string password)
        {
            _settings = new YouTubeRequestSettings(appName, developerKey, userName, password);
            _settings.AutoPaging = true;

            _userName = userName;
        }

        /// <summary>
        /// Retrieves a list of videos uploaded by the configured user.
        /// </summary>
        public IList<Video> GetUploadsForUser()
        {
            try
            {
                var userName = _userName;
                if (!string.IsNullOrEmpty(UserToDisplay))
                    userName = UserToDisplay;

                var request = new YouTubeRequest(_settings);
                var uri = new Uri("http://gdata.youtube.com/feeds/api/users/{0}/uploads".FormatWith(userName));

                var feed = request.Get<Video>(uri);

                return feed.Entries.ToList();
            }
            catch
            {
                return new List<Video>();
            }
        }

        /// <summary>
        /// Retrieves the details of a specific video by its ID.
        /// </summary>
        /// <param name="id">The YouTube video ID.</param>
        public Video GetVideo(string id)
        {
            try
            {
                var request = new YouTubeRequest(_settings);
                var uri = new Uri("http://gdata.youtube.com/feeds/api/videos/{0}".FormatWith(id));

                var video = request.Retrieve<Video>(uri);

                return video;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the current most popular videos on YouTube.
        /// </summary>
        public IList<Video> GetMostPopular()
        {
            try
            {
                var request = new YouTubeRequest(_settings);
                var feed = request.GetStandardFeed(YouTubeQuery.MostPopular);

                return feed.Entries.ToList();
            }
            catch
            {
                return new List<Video>();
            }
        } 

        /// <summary>
        /// Performs a search of YouTube videos.
        /// </summary>
        /// <param name="terms">The query to search for.</param>
        public IList<Video> Search(string terms)
        {
            try
            {
                var request = new YouTubeRequest(_settings);
                var query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri)
                {
                    OrderBy = "viewCount",
                    Query = terms,
                    SafeSearch = YouTubeQuery.SafeSearchValues.None
                };

                var feed = request.Get<Video>(query);

                return feed.Entries.ToList();
            }
            catch
            {
                return new List<Video>();
            }
        } 
    }
}