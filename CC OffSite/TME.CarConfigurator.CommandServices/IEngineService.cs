﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TME.CarConfigurator.Publisher.Common.Result;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.CommandServices
{
    public interface IEngineService
    {
        Task<Result> PutTimeFrameGenerationEngines(String brand, String country, Guid publicationID, Guid timeFrameID, IEnumerable<Engine> engines);
    }
}
