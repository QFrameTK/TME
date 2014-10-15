﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Repository.Objects.Assets;

namespace TME.CarConfigurator.Publisher.Interfaces
{
    public interface IAssetTypeMapper
    {
        AssetType MapGenerationAssetType(Administration.Assets.AssetType assetType);
    }
}