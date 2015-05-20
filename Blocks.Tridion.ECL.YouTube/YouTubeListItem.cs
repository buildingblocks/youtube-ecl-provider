using System;
using Google.YouTube;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    public class YouTubeListItem : IContentLibraryListItem
    {
        protected readonly Video Video;
        private readonly IEclUri _id;

        public YouTubeListItem(int publicationId, Video video)
        {
            Video = video;
            _id = YouTubeProvider.HostServices.CreateEclUri(publicationId, YouTubeProvider.MountPointId, Video.VideoId, DisplayTypeId, EclItemTypes.File);
        }

        public bool CanGetUploadMultimediaItemsUrl
        {
            get { return true; }
        }

        public bool CanSearch
        {
            get { return false; }
        }

        public string DisplayTypeId
        {
            get { return "vid"; }
        }

        public string IconIdentifier
        {
            get { return null; }
        }

        public IEclUri Id
        {
            get { return _id; }
        }

        public bool IsThumbnailAvailable
        {
            get { return Video.Thumbnails.Count > 0; }
        }

        public DateTime? Modified
        {
            get { return Video.AtomEntry.Published; }
        }

        public string ThumbnailETag
        {
            get { return Video.ETag; }
        }

        public string Title
        {
            get { return Video.Title; }
            set { throw new NotSupportedException(); }
        }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotSupportedException();
        }
    }
}