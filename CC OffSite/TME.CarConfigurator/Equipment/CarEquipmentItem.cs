﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Assets;
using TME.CarConfigurator.Core;
using TME.CarConfigurator.Extensions;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Enums;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.Interfaces.Rules;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Rules;
using ExteriorColourInfo = TME.CarConfigurator.Colours.ExteriorColourInfo;
using IExteriorColour = TME.CarConfigurator.Interfaces.Equipment.IExteriorColour;
using UpholsteryInfo = TME.CarConfigurator.Colours.UpholsteryInfo;

namespace TME.CarConfigurator.Equipment
{
    public abstract class CarEquipmentItem<T> : BaseObject<T>, ICarEquipmentItem
        where T  : Repository.Objects.Equipment.CarEquipmentItem
    {
        private readonly Guid _carID;
        private BestVisibleIn _bestVisibleIn;
        private CategoryInfo _categoryInfo;
        private List<Link> _links;
        private ExteriorColour _exteriorColour;
        private IReadOnlyList<IVisibleInModeAndView> _visibleIn;
        private readonly Publication _repositoryPublication;
        private readonly Context _repositoryContext;
        private readonly IAssetFactory _assetFactory;
        private IReadOnlyList<IAsset> _assets;
        private List<ExteriorColourInfo> _availableOnExteriorColors;
        private List<UpholsteryInfo> _availableOnUpholsteries;
        private IRuleSets _rules;
        readonly IRuleFactory _ruleFactory;

        protected CarEquipmentItem(T repositoryObject,Publication publication, Guid carID, Context context, IAssetFactory assetFactory, IRuleFactory ruleFactory) 
            : base(repositoryObject)
        {
            if (publication == null) throw new ArgumentNullException("publication");
            if (context == null) throw new ArgumentNullException("context");
            if (assetFactory == null) throw new ArgumentNullException("assetFactory");
            if (ruleFactory == null) throw new ArgumentNullException("ruleFactory");

            _carID = carID;

            _repositoryContext = context;
            _assetFactory = assetFactory;
            _ruleFactory = ruleFactory;
            _repositoryPublication = publication;
        }

        public int ShortID { get { return RepositoryObject.ShortID; } }

        public string InternalName { get { return RepositoryObject.InternalName; } }

        public string PartNumber { get { return RepositoryObject.PartNumber; } }

        public string Path { get { return RepositoryObject.Path; } }

        public bool KeyFeature { get { return RepositoryObject.KeyFeature; } }

        public bool GradeFeature { get { return RepositoryObject.GradeFeature; } }

        public bool OptionalGradeFeature { get { return RepositoryObject.OptionalGradeFeature; } }

        [Obsolete]
        public bool Brochure { get { return RepositoryObject.Visibility.HasFlag(Repository.Objects.Enums.Visibility.Brochure); } }

        public bool Standard { get { return RepositoryObject.Standard; } }

        public bool Optional { get { return RepositoryObject.Optional; } }

        public abstract IPrice Price { get; }

        public Visibility Visibility { get { return RepositoryObject.Visibility.ToVisibility(); } }

        public IBestVisibleIn BestVisibleIn
        {
            get { return _bestVisibleIn = _bestVisibleIn ?? new BestVisibleIn(RepositoryObject.BestVisibleIn); }
        }

        public ICategoryInfo Category { get { return _categoryInfo = _categoryInfo ?? new CategoryInfo(RepositoryObject.Category); } }

        public IExteriorColour ExteriorColour { get { return RepositoryObject.ExteriorColour == null ? null : _exteriorColour = _exteriorColour ?? new ExteriorColour(RepositoryObject.ExteriorColour); } }

        public IReadOnlyList<ILink> Links { get { return _links = _links ?? RepositoryObject.Links.Select(link => new Link(link)).ToList(); } }

        public IReadOnlyList<IVisibleInModeAndView> VisibleIn { get { return _visibleIn = _visibleIn ?? RepositoryObject.VisibleIn.Select(visibleIn => new CarEquipmentVisibleInModeAndView(_carID,RepositoryObject.ID,visibleIn,_repositoryPublication,_repositoryContext,_assetFactory)).ToList(); } }

        public IReadOnlyList<IAsset> Assets { get { return _assets = _assets ?? _assetFactory.GetCarEquipmentAssets(_repositoryPublication, _carID, RepositoryObject.ID, _repositoryContext); } }

        public IReadOnlyList<IExteriorColourInfo> AvailableForExteriorColours { get { return _availableOnExteriorColors = _availableOnExteriorColors ?? RepositoryObject.AvailableForExteriorColours.Select(colorInfo => new ExteriorColourInfo(colorInfo)).ToList(); } }
        public IReadOnlyList<IUpholsteryInfo> AvailableForUpholsteries { get { return _availableOnUpholsteries = _availableOnUpholsteries ?? RepositoryObject.AvailableForUpholsteries.Select(upholsteryInfo => new UpholsteryInfo(upholsteryInfo)).ToList(); } }

        public IRuleSets Rules
        {
            get
            {
                return
                    _rules =
                        _rules ??
                        _ruleFactory.GetCarEquipmentRuleSets(RepositoryObject.ID, _carID, _repositoryPublication,
                            _repositoryContext);
            }
        }
    }
}