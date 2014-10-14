﻿using TME.CarConfigurator.Interfaces.Factories;

namespace TME.CarConfigurator.DI.Interfaces
{
    public interface IModelFactoryFacade
    {
        IModelFactoryFacade WithServiceFacade(IServiceFacade serviceFacade);
        IModelFactoryFacade WithPublicationFactory(IPublicationFactory publicationFactory);
        IModelFactoryFacade WithBodyTypeFactory(IBodyTypeFactory bodyTypeFactory);
        IModelFactoryFacade WithEngineFactory(IEngineFactory engineFactory);

        IModelFactory Create();
    }
}