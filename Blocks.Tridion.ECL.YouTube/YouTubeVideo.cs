using System;
using System.Collections.Generic;
using System.Linq;
using Google.YouTube;
using Tridion.ExternalContentLibrary.V2;

namespace Blocks.Tridion.ECL.YouTube
{
    public class YouTubeVideo : YouTubeListItem, IContentLibraryMultimediaItem
    {
        public YouTubeVideo(int publicationId, Video video)
            : base(publicationId, video) {}

        public IContentLibraryItem Save(bool readback)
        {
            return readback ? this : null;
        }

        public bool CanGetViewItemUrl
        {
            get { return true; }
        }

        public bool CanUpdateMetadataXml
        {
            get { return false; }
        }

        public bool CanUpdateTitle
        {
            get { return false; }
        }

        public DateTime? Created
        {
            get { return Video.AtomEntry.Published; }
        }

        public string CreatedBy
        {
            get { return Video.Uploader; }
        }

        public string MetadataXml
        {
            get
            {
                return "<Metadata xmlns=\"http://gdata.youtube.com/schemas/2007\"><Description>{0}</Description><Width>{1}</Width><Height>{2}</Height><VideoID>{3}</VideoID><MimeType>{4}</MimeType></Metadata>"
                       .FormatWith(Video.Description, Width, Height, Filename, MimeType);
            }
            set { throw new NotSupportedException(); }
        }

        public ISchemaDefinition MetadataXmlSchema
        {
            get 
            {
                var schema = YouTubeProvider.HostServices.CreateSchemaDefinition("Metadata", "http://gdata.youtube.com/schemas/2007");
                schema.Fields.Add(YouTubeProvider.HostServices.CreateMultiLineTextFieldDefinition("Description", "Description", 0, 1, 7));
                schema.Fields.Add(YouTubeProvider.HostServices.CreateNumberFieldDefinition("Width", "Width", 0, 1));
                schema.Fields.Add(YouTubeProvider.HostServices.CreateNumberFieldDefinition("Height", "Height", 0, 1));
                schema.Fields.Add(YouTubeProvider.HostServices.CreateSingleLineTextFieldDefinition("VideoID", "Video ID", 0, 1));
                schema.Fields.Add(YouTubeProvider.HostServices.CreateSingleLineTextFieldDefinition("MimeType", "MIME Type", 0, 1));

                return schema;
            }
        }

        public string ModifiedBy
        {
            get { return Video.Uploader; }
        }

        public IEclUri ParentId
        {
            get { return YouTubeProvider.HostServices.CreateEclUri(Id.PublicationId, Id.MountPointId); }
        }

        public IContentResult GetContent(IList<ITemplateAttribute> attributes)
        {
            return null;
        }

        public string GetDirectLinkToPublished(IList<ITemplateAttribute> attributes)
        {
            return "http://www.youtube.com/embed/{0}".FormatWith(Video.VideoId);
        }

        public string GetTemplateFragment(IList<ITemplateAttribute> attributes)
        {
            var supportedAttributeNames = new[] {"width", "height"};
            var supportedAttributes = attributes.SupportedAttributes(supportedAttributeNames);

            return "<iframe src=\"{0}\" {1}></iframe>".FormatWith(GetDirectLinkToPublished(attributes), supportedAttributes);
        }

        public string Filename
        {
            get { return Video.VideoId; }
        }

        public int? Height
        {
            get { return 600; }
        }

        public string MimeType
        {
            get 
            { 
                var content = Video.Contents.FirstOrDefault();
                return content != null ? content.Type : "application/x-shockwave-flash";
            }
        }

        public int? Width
        {
            get { return 600; }
        }
    }
}