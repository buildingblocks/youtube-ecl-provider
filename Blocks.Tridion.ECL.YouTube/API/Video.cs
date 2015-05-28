using System;
using Google.Apis.YouTube.v3.Data;

namespace Blocks.Tridion.ECL.YouTube.API
{
    public class Video
    {
        public Video(PlaylistItem result)
        {
            VideoId = result.Snippet.ResourceId.VideoId;
            Title = result.Snippet.Title;
            Uploader = "";
            Description = result.Snippet.Description;
            //Type=result.Snippet.
            Thumbnail = result.Snippet.Thumbnails.Default.Url;
            ThumbnailETag = result.Snippet.Thumbnails.ETag;
        }

        public Video(SearchResult result)
        {
            VideoId = result.Id.VideoId;
            Title = result.Snippet.Title;
            Uploader = "";
            Description = result.Snippet.Description;
            //Type=result
            Thumbnail = result.Snippet.Thumbnails.Default.Url;
            ThumbnailETag = result.Snippet.Thumbnails.ETag;
        }

        public DateTime? Published { get; set; }
        public string Title { get; set; }
        public string Uploader { get; set; }
        public string Description { get; set; }
        public string VideoId { get; set; }
        public string Type { get; set; }

        public string Thumbnail { get; set; }
        public string ThumbnailETag { get; set; }
    }
}
