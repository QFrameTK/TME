﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Assets;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator
{
    public class CarPart : ICarPart
    {
        protected readonly Repository.Objects.CarPart RepositoryCarPart;
        protected IAssetFactory AssetFactory;
        protected Publication RepositoryPublication;
        private readonly Guid _carID;
        protected Context RepositoryContext;
        protected IReadOnlyList<VisibleInModeAndView> FetchedVisibleIn;

        public CarPart(Repository.Objects.CarPart repositoryCarPart, Publication repositoryPublication, Guid carID, Context repositoryContext, IAssetFactory assetFactory)
        {
            if (repositoryCarPart == null) throw new ArgumentNullException("repositoryCarPart");
            if (repositoryPublication == null) throw new ArgumentNullException("repositoryPublication");
            if (repositoryContext == null) throw new ArgumentNullException("repositoryContext");
            if (assetFactory == null) throw new ArgumentNullException("assetFactory");

            RepositoryCarPart = repositoryCarPart;
            AssetFactory = assetFactory;
            RepositoryContext = repositoryContext;
            RepositoryPublication = repositoryPublication;
            _carID = carID;
        }

        public string Code { get { return RepositoryCarPart.Code; } }
        public string Name { get { return RepositoryCarPart.Name; } }
        public Guid ID { get { return RepositoryCarPart.ID; } }

        public virtual IReadOnlyList<IVisibleInModeAndView> VisibleIn
        {
            get
            {
                return
                    FetchedVisibleIn =
                    FetchedVisibleIn ??
                    RepositoryCarPart.VisibleIn.Select(
                    visibleIn =>
                    new CarPartVisibleInModeAndView(_carID, ID, visibleIn, RepositoryPublication,
                    RepositoryContext, AssetFactory)).ToList();
            }
        }
    }
}