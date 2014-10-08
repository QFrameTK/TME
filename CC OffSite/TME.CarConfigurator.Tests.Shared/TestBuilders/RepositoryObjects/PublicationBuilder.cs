using System;
using System.Collections.Generic;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Tests.Shared.TestBuilders.RepositoryObjects
{
    public class PublicationBuilder
    {
        private readonly Publication _publication;

        private PublicationBuilder(Publication publication)
        {
            _publication = publication;
        }

        public static PublicationBuilder Initialize()
        {
            var publication = new Publication
            {
                TimeFrames = new List<PublicationTimeFrame>()
            };

            return new PublicationBuilder(publication);
        }

        public PublicationBuilder WithGeneration(Generation generation)
        {
            _publication.Generation = generation;

            return this;
        }

        public PublicationBuilder WithID(Guid publicationId)
        {
            _publication.ID = publicationId;

            return this;
        }

        public PublicationBuilder WithTimeFrames(params PublicationTimeFrame[] timeFrames)
        {
            _publication.TimeFrames.AddRange(timeFrames);

            return this;
        }

        public Publication Build()
        {
            return _publication;
        }
    }
}