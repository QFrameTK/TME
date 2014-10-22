﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.Publisher.Common.Result;

namespace TME.CarConfigurator.Publisher.Interfaces
{
    public interface ITransmissionPublisher
    {
        Task<IEnumerable<Result>> PublishGenerationTransmissions(IContext context);
    }
}
