using System;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.QueryRepository.Service.Interfaces
{
    public interface IPublicationService
    {
        Publication GetPublication(Guid publicationId);
    }
}