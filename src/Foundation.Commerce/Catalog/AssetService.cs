using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Catalog
{
    public class AssetService : IAssetService
    {
        private readonly IContentLoader _contentLoader;
        private readonly AssetUrlResolver _assetUrlResolver;
        private readonly UrlResolver _urlResolver;

        public AssetService(IContentLoader contentLoader, AssetUrlResolver assetUrlResolver, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _assetUrlResolver = assetUrlResolver;
        }

        public string GetDefaultAsset<TContentMedia>(IAssetContainer assetContainer) 
            where TContentMedia : IContentMedia
        {
            var url = _assetUrlResolver.GetAssetUrl<TContentMedia>(assetContainer);
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return uri.PathAndQuery;
            }

            return url;
        }

        public IList<string> GetAssets<TContentMedia>(IAssetContainer assetContainer)
            where TContentMedia : IContentMedia
        {
            var assets = new List<string>();
            if (assetContainer.CommerceMediaCollection != null)
            {
                assets.AddRange(assetContainer.CommerceMediaCollection
                    .Where(x => ValidateCorrectType<TContentMedia>(x.AssetLink))
                    .Select(media => _urlResolver.GetUrl(media.AssetLink)));
            }

            if (!assets.Any())
            {
                assets.Add(string.Empty);
            }

            return assets;
        }

        private bool ValidateCorrectType<TContentMedia>(ContentReference contentLink)
            where TContentMedia : IContentMedia
        {
            if (typeof(TContentMedia) == typeof(IContentMedia))
            {
                return true;
            }

            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return false;
            }

            return _contentLoader.TryGet(contentLink, out TContentMedia content);
        }
    }
}