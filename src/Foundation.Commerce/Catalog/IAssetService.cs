using System.Collections.Generic;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace Foundation.Commerce.Catalog
{
    public interface IAssetService
    {
        IList<string> GetAssets<TContentMedia>(IAssetContainer assetContainer) where TContentMedia : IContentMedia;
        string GetDefaultAsset<TContentMedia>(IAssetContainer assetContainer) where TContentMedia : IContentMedia;
    }
}