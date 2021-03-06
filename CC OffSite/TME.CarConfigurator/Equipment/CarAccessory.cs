﻿using System;
using TME.CarConfigurator.Core;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Equipment
{
    public class CarAccessory : CarEquipmentItem<Repository.Objects.Equipment.CarAccessory>, ICarAccessory
    {
        private Price _price;

        public CarAccessory(Repository.Objects.Equipment.CarAccessory repositoryObject, Guid carID, Publication publication, Context context, IAssetFactory assetFactory, IRuleFactory ruleFactory)
            : base(repositoryObject, publication, carID, context, assetFactory, ruleFactory)
        {
        }

        public IPrice BasePrice { get { return _price = _price ?? new Price(RepositoryObject.BasePrice); } }

        public IMountingCosts MountingCostsOnNewVehicle { get { return new MountingCosts(RepositoryObject.MountingCostsOnNewVehicle); } }

        public IMountingCosts MountingCostsOnUsedVehicle { get { return new MountingCosts(RepositoryObject.MountingCostsOnUsedVehicle); } }

        public override IPrice Price
        {
            get { return new Price(new Repository.Objects.Core.Price
                {
                    ExcludingVat =  RepositoryObject.BasePrice.ExcludingVat + RepositoryObject.MountingCostsOnNewVehicle.Price.ExcludingVat,
                    IncludingVat = RepositoryObject.BasePrice.IncludingVat + RepositoryObject.MountingCostsOnNewVehicle.Price.IncludingVat
                });
            }
        }
    }
}