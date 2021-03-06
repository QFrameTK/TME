﻿using System;
using System.Collections.Generic;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Interfaces.Factories
{
    public interface IEngineFactory
    {
        IReadOnlyList<IEngine> GetEngines(Publication publication, Context context);
        IEngine GetCarEngine(Engine repoEngine, Guid carId, Publication publication, Context context);
    }
}
