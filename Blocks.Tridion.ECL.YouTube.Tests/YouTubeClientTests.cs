using System.Configuration;
using Blocks.Tridion.ECL.YouTube.API;
using NUnit.Framework;

namespace Blocks.Tridion.ECL.YouTube.Tests
{
    [TestFixture]
    public class YouTubeClientTests
    {
        private YouTubeClient _client;

        [SetUp]
        public void Setup()
        {
            // NOTE: Go configure the settings before running this...

            var appSettings = ConfigurationManager.AppSettings;

            _client = new YouTubeClient(appSettings["AppName"],
                                        appSettings["DeveloperKey"],
                                        appSettings["Username"], 
                                        appSettings["Password"]);
        }

        [Test]
        public void Can_get_videos()
        {
            _client.UserToDisplay = "BadLipReading";

            var videos = _client.GetUploadsForUser();

            Assert.That(videos, Is.Not.Null);
            Assert.That(videos, Is.Not.Empty);
        }

        [Test]
        public void Can_get_popular_videos()
        {
            var videos = _client.GetMostPopular();

            Assert.That(videos, Is.Not.Null);
            Assert.That(videos, Is.Not.Empty);
        }

        [Test]
        public void Can_get_video_by_id()
        {
            var video = _client.GetVideo("D7jtpy0lfBU");

            Assert.That(video, Is.Not.Null);
        }

        [Test]
        public void Can_search_videos()
        {
            var videos = _client.Search("cats");

            Assert.That(videos, Is.Not.Null);
            Assert.That(videos, Is.Not.Empty);
        }

        [TearDown]
        public void Teardown()
        {
            _client = null;
        }
    }
}