﻿
using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Assets;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator
{
    public class SubModelGrade : Grade
    {
        private readonly Guid _subModelID;

        public SubModelGrade(Repository.Objects.Grade repositoryGrade, Publication repositoryPublication, Context repositoryContext,Guid subModelID, IAssetFactory assetFactory, IEquipmentFactory gradeEquipmentFactory, IPackFactory packFactory) 
            : base(repositoryGrade, repositoryPublication, repositoryContext, assetFactory, gradeEquipmentFactory, packFactory)
        {
            _subModelID = subModelID;
        }

        public override IReadOnlyList<IVisibleInModeAndView> VisibleIn
        {
            get
            {
                return
                    FetchedVisibleInModeAndViews =
                        FetchedVisibleInModeAndViews ??
                        RepositoryObject.VisibleIn.Select(
                            visibleIn =>
                                new SubModelVisibleInModeAndView(_subModelID, RepositoryObject.ID, visibleIn,
                                    RepositoryPublication, RepositoryContext, AssetFactory)).ToList();
            }
        }

        public override IReadOnlyList<IAsset> Assets
        {
            get { return FetchedAssets = FetchedAssets ?? AssetFactory.GetSubModelAssets(RepositoryPublication, _subModelID, ID, RepositoryContext); } }

        public override IGradeEquipment Equipment
        {
            get { return FetchedEquipment = FetchedEquipment ?? GradeEquipmentFactory.GetSubModelGradeEquipment(RepositoryPublication, _subModelID, RepositoryContext, ID); }
        }

        public override IReadOnlyList<Interfaces.Packs.IGradePack> Packs
        {
            get { return FetchedPacks = FetchedPacks ?? PackFactory.GetSubModelGradePacks(RepositoryPublication, RepositoryContext, _subModelID, RepositoryObject.ID); }
        }
    }
}