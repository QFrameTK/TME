﻿using System;
using Spring.Context.Support;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common.Enums;
using TME.CarConfigurator.Publisher.DI.Interfaces;
using TME.CarConfigurator.S3.CommandServices;
using TME.CarConfigurator.S3.Shared.Interfaces;

namespace TME.CarConfigurator.Publisher.DI.FileSystem
{
    public class ServiceFactory : IServiceFactory
    {
        readonly ISerialiser _serialiser;
        readonly IKeyManager _keyManager;

        public ServiceFactory(ISerialiser serialiser, IKeyManager keyManager)
        {
            _serialiser = serialiser;
            _keyManager = keyManager;
        }

        private static IService GetService(String environment, PublicationDataSubset dataSubset)
        {
            return (IService)ContextRegistry.GetContext().GetObject(String.Format("{0}{1}FSService", environment, dataSubset));
        }

        public QueryServices.IModelService GetGetModelService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new CarConfigurator.S3.QueryServices.ModelService(_serialiser, service, _keyManager);
        }

        public IModelService GetPutModelService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new ModelService(service, _serialiser, _keyManager);
        }

        public IPublicationService GetPublicationService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new PublicationService(service, _serialiser, _keyManager);
        }

        public IBodyTypeService GetBodyTypeService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new BodyTypeService(service, _serialiser, _keyManager);
        }

        public IEngineService GetEngineService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new EngineService(service, _serialiser, _keyManager);
        }

        public ITransmissionService GetTransmissionService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new TransmissionService(service, _serialiser, _keyManager);
        }

        public IWheelDriveService GetWheelDriveService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new WheelDriveService(service, _serialiser, _keyManager);
        }

        public ISteeringService GetSteeringService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new SteeringService(service, _serialiser, _keyManager);
        }

        public IGradeService GetGradeService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new GradeService(service, _serialiser, _keyManager);
        }

        public ICarService GetCarService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new CarService(service, _serialiser, _keyManager);
        }

        public IAssetService GetAssetService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new AssetsService(service, _serialiser, _keyManager);
        }

        public ISubModelService GetSubModelService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new SubModelService(service, _serialiser, _keyManager);
        }

        public IEquipmentService GetEquipmentService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new EquipmentService(service, _serialiser, _keyManager);
        }

        public ISpecificationsService GetSpecificationsService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new SpecificationsService (service, _serialiser, _keyManager);
        }

        public IPackService GetPackService(string environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new PackService(service, _serialiser, _keyManager);
        }

        public IRuleService GetRuleService(string environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new RuleService(service, _serialiser, _keyManager);
        }

        public IColourService GetColourCombinationService(String environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new ColourService(service, _serialiser, _keyManager);
        }

        public ICarPartService GetCarPartService(string environment, PublicationDataSubset dataSubset)
        {
            var service = GetService(environment, dataSubset);

            return new CarPartService(service, _serialiser, _keyManager);
        }
    }
}
