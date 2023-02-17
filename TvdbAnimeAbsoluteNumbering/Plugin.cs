using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using TvdbAnimeAbsoluteNumbering.Configuration;
using System;
using System.Collections.Generic;
using MediaBrowser.Model.Drawing;
using System.IO;

namespace TvdbAnimeAbsoluteNumbering
{
    public class Plugin
        : BasePlugin<PluginConfiguration>, IHasWebPages, IHasThumbImage
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogger logger) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public override string Name
        {
            get { return "Tvdb Anime Absolute Numbering Enhancer"; }
        }

        public static Plugin Instance { get; private set; }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "anime absolute numbering",
                    EmbeddedResourcePath = "TvdbAnimeAbsoluteNumbering.Configuration.configPage.html"
                }
            };
        }

        private Guid _id = new Guid("e7a3a125-6c66-4ea1-8396-863e9f339a72");

        public override Guid Id
        {
            get { return _id; }
        }

        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream(type.Namespace + ".thumb.png");
        }

        public ImageFormat ThumbImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }
    }
}