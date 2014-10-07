using System;
using TME.CarConfigurator.Extensions;
using TME.CarConfigurator.Factories.Interfaces;
using TME.CarConfigurator.QueryRepository.Interfaces;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Factories
{
    public class PublicationFactory : IPublicationFactory
    {
        private readonly IPublicationRepository _publicationRepository;

        public PublicationFactory(IPublicationRepository publicationRepository)
        {
            if (publicationRepository == null) throw new ArgumentNullException("publicationRepository");

            _publicationRepository = publicationRepository;
        }

        public Publication Get(Repository.Objects.Model repositoryModel)
        {
            var publicationInfo = repositoryModel.GetActivePublicationInfo();

            return _publicationRepository.Get(publicationInfo.ID);
        }
    }
}