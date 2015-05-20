using System;
using System.AddIn;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Blocks.Tridion.ECL.YouTube.API;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    [AddIn("YouTubeProvider", Version = "0.1.0.0")]
    public class YouTubeProvider : IContentLibrary
    {
        private static readonly string ThemeBasePath = Path.Combine(AddInFolder, "Themes");
        private static readonly XNamespace Namespace = "http://gdata.youtube.com/schemas/2007";

        internal static string MountPointId { get; private set; }
        internal static IHostServices HostServices { get; private set; }
        internal static YouTubeClient Client { get; private set; }

        public IContentLibraryContext CreateContext(IEclSession session)
        {
            return new YouTubeMountPoint();
        }

        public byte[] GetIconImage(string theme, string iconIdentifier, int iconSize)
        {
            return GetIconImage(iconIdentifier, iconSize);
        }

        public void Initialize(string mountPointId, string configurationXmlElement, IHostServices hostServices)
        {
            MountPointId = mountPointId;
            HostServices = hostServices;

            var config = XElement.Parse(configurationXmlElement);

            

            Client = new YouTubeClient(config.Element(Namespace + "AppName").Value,
                                       config.Element(Namespace + "DeveloperKey").Value,
                                       config.Element(Namespace + "Username").Value,
                                       config.Element(Namespace + "Password").Value)
            {
                UserToDisplay = config.Element(Namespace + "UserToDisplay").Value
            };
        }

        public IList<IDisplayType> DisplayTypes
        {
            get
            {
                return new List<IDisplayType>
                {
                    HostServices.CreateDisplayType("vid", "YouTube Video", EclItemTypes.File)
                };
            }
        }

        public void Dispose() {}

        internal static string AddInFolder
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        internal static byte[] GetIconImage(string icon, int size)
        {
            int actual;
            return HostServices.GetIcon(ThemeBasePath, "_Default", icon, size, out actual);
        }
    }
}