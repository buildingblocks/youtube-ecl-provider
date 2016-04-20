using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using log4net.Repository.Hierarchy;

namespace Blocks.Tridion.ECL.YouTube.API
{
    /// <summary>
    /// Client for accessing the YouTube API.
    /// </summary>
    public class YouTubeClient
    {
        private readonly YouTubeService _youTubeService;
        private readonly string _userName;

        public WebProxy _proxy { get; set; }
        public string UserToDisplay { get; set; }
        public List<string> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="apiKey"></param>
        /// <param name="userName"></param>
        public YouTubeClient(string appName, string apiKey, string userName)
        {
            _youTubeService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApplicationName = appName,
                    ApiKey = apiKey
                });
            _userName = userName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTubeClient"/> class with proxy .
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="apiKey"></param>
        /// <param name="userName"></param>
        /// <param name="proxy"></param>
        public YouTubeClient(string appName, string apiKey, string userName, WebProxy proxy)
        {
            _youTubeService = new YouTubeService(
                 new BaseClientService.Initializer()
                 {
                     ApplicationName = appName,
                     ApiKey = apiKey
                 });
            _userName = userName;
            _proxy = proxy;

        }


        /// <summary>
        /// Retrieves a list of videos uploaded by the configured user.
        /// </summary>
        public IList<Video> GetUploadsForUser()
        {
            try
            {
                List<Video> videos = new List<Video>();
                var channelsListRequest = _youTubeService.Channels.List("contentDetails");
                channelsListRequest.ForUsername = _userName;
                var channelsListResponse = channelsListRequest.Execute();
                foreach (var channel in channelsListResponse.Items)
                {
                    var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                    var playlistItemsListRequest = _youTubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadsListId;
                    playlistItemsListRequest.MaxResults = 50;
                    //playlistItemsListRequest.PageToken = "";
                    var playlistItemsListResponse = playlistItemsListRequest.Execute();
                    videos.AddRange(playlistItemsListResponse.Items.Select(x => new Video(x)));
                }
                return videos;
            }
            catch (Exception ex)
            {
                return new List<Video>();
            }
        }

        /// <summary>
        /// GetUploadsForUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalVideos"></param>
        /// <returns></returns>
        public IList<Video> GetUploadsForUser(string user, int pageIndex, out int totalVideos)
        {
            totalVideos = 0;
            try
            {
                List<Video> videos = new List<Video>();
                var channelsListRequest = _youTubeService.Channels.List("contentDetails");
                channelsListRequest.ForUsername = user;
                var channelsListResponse = channelsListRequest.Execute();
                string pageToken = " ";
                foreach (var channel in channelsListResponse.Items)
                {
                    //TODO: This loop is to find the pageToken, If you find a better option DO IT
                    for (int page = 0; page <= pageIndex; page++)
                    {
                        var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                        var playlistItemsListRequest = _youTubeService.PlaylistItems.List("snippet");
                        playlistItemsListRequest.PlaylistId = uploadsListId;
                        playlistItemsListRequest.MaxResults = 50;
                        playlistItemsListRequest.PageToken = pageToken;
                        var playlistItemsListResponse = playlistItemsListRequest.Execute();
                        pageToken = playlistItemsListResponse.NextPageToken;
                        
                        if (playlistItemsListResponse.PageInfo.TotalResults != null)
                            totalVideos = playlistItemsListResponse.PageInfo.TotalResults.Value;
                        if (page == pageIndex)
                        {
                            videos.AddRange(playlistItemsListResponse.Items.Select(x => new Video(x)));
                        }
                    }



                }
                return videos;
            }
            catch (Exception ex)
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
                var search = _youTubeService.Search.List("id,snippet");
                search.Q = id;
                var result = search.Execute();
                var video = result.Items.FirstOrDefault();
                if (video == null) return null;
                return (new Video(video));
            }
            catch
            {
                return null;
            }
        }

        ///// <summary>
        ///// Retrieves the current most popular videos on YouTube.
        ///// </summary>
        //public IList<Video> GetMostPopular()
        //{
        //    try
        //    {
        //        var request = new YouTubeRequest(_youTubeService);
        //        if (_proxy != null)
        //        {
        //            request.Proxy = _proxy;
        //        }
        //        var feed = request.GetStandardFeed(YouTubeQuery.MostPopular);

        //        return feed.Entries.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<Video>();
        //    }
        //}

        ///// <summary>
        ///// Performs a search of YouTube videos.
        ///// </summary>
        ///// <param name="terms">The query to search for.</param>
        public IList<Video> Search(string terms)
        {
            try
            {
                List<Video> videos = new List<Video>();
                var search = _youTubeService.Search.List("snippet");
                search.Q = terms;
                var result = search.Execute();
                videos.AddRange(result.Items.Select(x => new Video(x)));
                return videos;

            }
            catch (Exception ex)
            {
                return new List<Video>();
            }
        }
    }
}