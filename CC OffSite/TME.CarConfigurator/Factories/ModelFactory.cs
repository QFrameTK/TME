using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Factories;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Enums;

namespace TME.CarConfigurator.Factories
{
    public class ModelFactory : IModelFactory
    {
        private readonly IModelService _modelService;
        private readonly IPublicationFactory _publicationFactory;
        private readonly IBodyTypeFactory _bodyTypeFactory;
        private readonly IEngineFactory _engineFactory;
        private readonly ITransmissionFactory _transmissionFactory;
        private readonly IWheelDriveFactory _wheelDriveFactory;
        private readonly ISteeringFactory _steeringFactory;
        private readonly ICarFactory _carFactory;

        public ModelFactory(
            IModelService modelService,
            IPublicationFactory publicationFactory,
            IBodyTypeFactory bodyTypeFactory,
            IEngineFactory engineFactory,
            ITransmissionFactory transmissionFactory,
            IWheelDriveFactory wheelDriveFactory,
            ISteeringFactory steeringFactory,
            ICarFactory carFactory)
        {
            if (modelService == null) throw new ArgumentNullException("modelService");
            if (publicationFactory == null) throw new ArgumentNullException("publicationFactory");
            if (bodyTypeFactory == null) throw new ArgumentNullException("bodyTypeFactory");
            if (engineFactory == null) throw new ArgumentNullException("engineFactory");
            if (transmissionFactory == null) throw new ArgumentNullException("transmissionFactory");
            if (wheelDriveFactory == null) throw new ArgumentNullException("wheelDriveFactory");
            if (steeringFactory == null) throw new ArgumentNullException("steeringFactory");
            if (carFactory == null) throw new ArgumentNullException("carFactory");

            _modelService = modelService;
            _publicationFactory = publicationFactory;
            _bodyTypeFactory = bodyTypeFactory;
            _engineFactory = engineFactory;
            _transmissionFactory = transmissionFactory;
            _wheelDriveFactory = wheelDriveFactory;
            _steeringFactory = steeringFactory;
            _carFactory = carFactory;
        }

        public IEnumerable<IModel> GetModels(Context context)
        {
            var repositoryModels = _modelService.GetModels(context).Where(HasActivePublicationsThatAreCurrentlyAvailable);

            var convertedModels = repositoryModels.Select(repositoryModel => CreateModel(repositoryModel, context));

            return convertedModels;
        }

        private static bool HasActivePublicationsThatAreCurrentlyAvailable(Repository.Objects.Model model)
        {
            return model.Publications.Any(p => p.State == PublicationState.Activated && p.LineOffFrom <= DateTime.Now && DateTime.Now <= p.LineOffTo);
        }

        private IModel CreateModel(Repository.Objects.Model repositoryModel, Context context)
        {
            return new Model(repositoryModel, context, _publicationFactory, _bodyTypeFactory, _engineFactory, _transmissionFactory, _wheelDriveFactory, _steeringFactory, _carFactory);
        }
    }
}