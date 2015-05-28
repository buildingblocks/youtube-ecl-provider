using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    public class YouTubeMountPoint : IContentLibraryContext
    {
        public bool CanGetUploadMultimediaItemsUrl(int publicationId)
        {
            return false;
        }

        public bool CanSearch(int publicationId)
        {
            return false;
        }

        public IList<IContentLibraryListItem> FindItem(IEclUri eclUri)
        {
            return null;
        }
        public bool canPaginate = true;
        public IFolderContent GetFolderContent(IEclUri parentFolderUri, int pageIndex, EclItemTypes itemTypes)
        {
            int totalVideos = 0;
            var items = new List<IContentLibraryListItem>();

            if (parentFolderUri.ItemType == EclItemTypes.MountPoint && itemTypes.HasFlag(EclItemTypes.Folder))
            {
                items.AddRange(YouTubeProvider.Client.Users.Select(user => new YouTubeListItem(parentFolderUri.PublicationId, user)));
            }

            if (parentFolderUri.ItemType == EclItemTypes.Folder && parentFolderUri.SubType == "usr" && itemTypes.HasFlag(EclItemTypes.File))
            {
                canPaginate = true;
                items.AddRange(YouTubeProvider.Client.GetUploadsForUser(parentFolderUri.ItemId, pageIndex, out totalVideos).Select(v => new YouTubeListItem(parentFolderUri.PublicationId, v)));
            }

            if (pageIndex == 0 && items.Count < 50)
            {
                canPaginate = false;
            }

            if (canPaginate)
            {
                bool isLast = items.Count < 50 || ((pageIndex + 1) * 50 >= totalVideos);
                return YouTubeProvider.HostServices.CreateFolderContent(parentFolderUri, pageIndex, isLast, items, CanGetUploadMultimediaItemsUrl(parentFolderUri.PublicationId), CanSearch(parentFolderUri.PublicationId));
            }
            else
            {
                return YouTubeProvider.HostServices.CreateFolderContent(parentFolderUri, items, CanGetUploadMultimediaItemsUrl(parentFolderUri.PublicationId), CanSearch(parentFolderUri.PublicationId));
            }
        }

        public IContentLibraryItem GetItem(IEclUri eclUri)
        {
            if (eclUri.ItemType == EclItemTypes.File && eclUri.SubType == "vid")
            {
                return new YouTubeVideo(eclUri.PublicationId, YouTubeProvider.Client.GetVideo(eclUri.ItemId));
            }

            throw new NotSupportedException();
        }

        public IList<IContentLibraryItem> GetItems(IList<IEclUri> eclUris)
        {
            var result = new List<IContentLibraryItem>();

            var ids = eclUris.Where(u => u.ItemType == EclItemTypes.File && u.SubType == "vid")
                             .Select(uri => uri.ItemId).Distinct();

            foreach (var id in ids)
            {
                var uris = eclUris.Where(u => u.ItemType == EclItemTypes.File && u.SubType == "vid" && u.ItemId == id);
                result.AddRange(uris.Select(GetItem));
            }

            return result;
        }

        public byte[] GetThumbnailImage(IEclUri eclUri, int maxWidth, int maxHeight)
        {
            if (eclUri.ItemType == EclItemTypes.File && eclUri.SubType == "vid")
            {
                var video = YouTubeProvider.Client.GetVideo(eclUri.ItemId);

                using (var webClient = new WebClient())
                {
                    webClient.Proxy = YouTubeProvider.Client._proxy;
                    var data = webClient.DownloadData(video.Thumbnail);
                    using (var ms = new MemoryStream(data, false))
                    {
                        return YouTubeProvider.HostServices.CreateThumbnailImage(maxWidth, maxHeight, ms, 600, 600, null);
                    }
                }
            }

            return null;
        }

        public string GetUploadMultimediaItemsUrl(IEclUri parentFolderUri)
        {
            throw new NotSupportedException();
        }

        public string GetViewItemUrl(IEclUri eclUri)
        {
            //if (eclUri.ItemType == EclItemTypes.File && eclUri.SubType == "vid")
            //{
            //    return YouTubeProvider.Client.GetVideo(eclUri.ItemId).WatchPage.ToString();
            //}

            throw new NotSupportedException();
        }

        public IFolderContent Search(IEclUri contextUri, string searchTerm, int pageIndex, int numberOfItems)
        {
            throw new NotSupportedException();
        }

        public void StubComponentCreated(IEclUri eclUri, string tcmUri) { }

        public void StubComponentDeleted(IEclUri eclUri, string tcmUri) { }

        public string IconIdentifier
        {
            get { return "youtube"; }
        }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotSupportedException();
        }

        public void Dispose() { }
    }
}