using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Factories
{
    public class AssetFactory : IAssetFactory
    {
        private readonly IAssetService _assetService;

        public AssetFactory(IAssetService assetService)
        {
            if (assetService == null) throw new ArgumentNullException("assetService");

            _assetService = assetService;
        }

        public IReadOnlyList<IAsset> GetAssets(Publication publication, Guid objectId, Context context)
        {
            var repoAssets = _assetService.GetAssets(publication.ID, objectId, context);

            return TransformIntoNonRepoAssets(repoAssets);
        }

        public IReadOnlyList<IAsset> GetAssets(Publication publication, Guid objectId, Context context, string view, string mode)
        {
            var repoAssets = _assetService.GetAssets(publication.ID, objectId, context, view, mode);

            return TransformIntoNonRepoAssets(repoAssets);
        }

        public IReadOnlyList<IAsset> GetCarAssets(Publication publication, Guid carId, Guid objectId, Context context)
        {
            var repoAssets = _assetService.GetCarAssets(publication.ID, carId, objectId, context);

            return TransformIntoNonRepoAssets(repoAssets);
        }

        public IReadOnlyList<IAsset> GetCarAssets(Publication publication, Guid carId, Guid objectId, Context context, string view, string mode)
        {
            var repoAssets = _assetService.GetCarAssets(publication.ID, carId, objectId, context, view, mode);

            return TransformIntoNonRepoAssets(repoAssets);
        }

        private static IReadOnlyList<Asset> TransformIntoNonRepoAssets(IEnumerable<Repository.Objects.Assets.Asset> repoAssets)
        {
            return repoAssets.Select(a => new Asset(a)).ToArray();
        }
    }
}