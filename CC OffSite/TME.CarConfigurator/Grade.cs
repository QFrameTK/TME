﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Core;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.Interfaces.Packs;

namespace TME.CarConfigurator
{
    public class Grade : BaseObject<Repository.Objects.Grade>, IGrade
    {
        private readonly Repository.Objects.Publication _repositoryPublication;
        private readonly Repository.Objects.Context _repositoryContext;
        private readonly IAssetFactory _assetFactory;
        private readonly IGradeEquipmentFactory _gradeEquipmentFactory;
        private readonly IPackFactory _packFactory;
        private IEnumerable<IAsset> _fetchedAssets;
        private IEnumerable<IVisibleInModeAndView> _fetchedVisibleInModeAndViews;
        private IReadOnlyList<IGradeEquipmentItem> _equipmentItems;
        private IGradeEquipment _gradeEquipment;
        private IEnumerable<IGradePack> _packs;

        Price _price;
        readonly IGrade _basedUponGrade;

        public Grade(Repository.Objects.Grade repositoryGrade, Repository.Objects.Publication repositoryPublication, Repository.Objects.Context repositoryContext, IGrade basedUponGrade, IAssetFactory assetFactory, IGradeEquipmentFactory gradeEquipmentFactory, IPackFactory packFactory)
            : base(repositoryGrade)
        {
            if (repositoryPublication == null) throw new ArgumentNullException("repositoryPublication");
            if (repositoryContext == null) throw new ArgumentNullException("repositoryContext");
            if (assetFactory == null) throw new ArgumentNullException("assetFactory");
            if (gradeEquipmentFactory == null) throw new ArgumentNullException("gradeEquipmentFactory");
            if (packFactory == null) throw new ArgumentNullException("packFactory");

            _repositoryPublication = repositoryPublication;
            _repositoryContext = repositoryContext;
            _basedUponGrade = basedUponGrade;
            _assetFactory = assetFactory;
            _gradeEquipmentFactory = gradeEquipmentFactory;
            _packFactory = packFactory;
        }

        public bool Special { get { return RepositoryObject.Special; } }

        public IGrade BasedUpon { get { return _basedUponGrade; } }

        public IPrice StartingPrice { get { return _price = _price ?? new Price(RepositoryObject.StartingPrice); } }

        public virtual IEnumerable<IVisibleInModeAndView> VisibleIn
        {
            get
            {
                return _fetchedVisibleInModeAndViews = _fetchedVisibleInModeAndViews ?? RepositoryObject.VisibleIn.Select(visibleInModeAndView => new VisibleInModeAndView(RepositoryObject.ID, visibleInModeAndView, _repositoryPublication, _repositoryContext, _assetFactory)).ToList();
            }
        }

        public virtual IEnumerable<IAsset> Assets { get { return _fetchedAssets = _fetchedAssets ?? _assetFactory.GetAssets(_repositoryPublication, ID, _repositoryContext); } }

        public IGradeEquipment GradeEquipment
        {
            get { return _gradeEquipment = _gradeEquipment ?? _gradeEquipmentFactory.GetGradeEquipment(_repositoryPublication, _repositoryContext, ID); }
        }

        public IEnumerable<IGradeEquipmentItem> Equipment
        {
            get { return _equipmentItems = _equipmentItems ?? GradeEquipment.Accessories.Cast<IGradeEquipmentItem>().Concat(GradeEquipment.Options).ToList(); }
        }

        public IEnumerable<IGradePack> Packs
        {
            get { return _packs = _packs ?? _packFactory.GetGradePacks(_repositoryPublication, _repositoryContext, RepositoryObject.ID); }
        }
    }
}
