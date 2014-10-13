﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared.Result;

namespace TME.CarConfigurator.CommandServices
{
    public interface IPublicationService
    {
        Task<Result> PutPublication(String brand, String country, Publication publication);
    }
}
