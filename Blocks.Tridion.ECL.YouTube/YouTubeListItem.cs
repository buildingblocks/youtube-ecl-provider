using Blocks.Tridion.ECL.YouTube.API;
using System;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    public class YouTubeListItem : IContentLibraryListItem
    {
        protected readonly Video Video;
        private String User;
        private readonly IEclUri _id;
        bool IsVideo = false;

        private string _displayTypeId;

        public YouTubeListItem(int publicationId, Video video)
        {
            Video = video;
            _id = YouTubeProvider.HostServices.CreateEclUri(publicationId, YouTubeProvider.MountPointId, Video.VideoId, "vid", EclItemTypes.File);
            IsVideo = true;
            _displayTypeId = "vid";
        }

        public YouTubeListItem(int publicationId, String user)
        {
            User = user;
            _id = YouTubeProvider.HostServices.CreateEclUri(publicationId, YouTubeProvider.MountPointId, user, "usr", EclItemTypes.Folder);
            _displayTypeId = "usr";
            IsVideo = false;
        }

        public bool CanGetUploadMultimediaItemsUrl
        {
            get { return false; }
        }

        public bool CanSearch
        {
            get { return false; }
        }

        public string DisplayTypeId
        {
            get { return _displayTypeId; }
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

            get
            {
                if (IsVideo)
                    return Video.Thumbnail != null;
                else
                    return false;
            }
        }

        public DateTime? Modified
        {
            get
            {
                if (IsVideo)
                    return Video.Published;
                return null;

            }
        }

        public string ThumbnailETag
        {
            get
            {
                if (IsVideo)
                    return Video.ThumbnailETag;
                else
                    return "";

            }
        }

        public string Title
        {
            get
            {
                if (IsVideo)
                    return Video.Title;
                else
                    return User;
            }
            set { throw new NotSupportedException(); }
        }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotSupportedException();
        }
    }
}