﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Publisher.Common.Enums;

namespace TME.CarConfigurator.Publisher.Interfaces
{
    public interface IPublisherFacade
    {
        IPublisher GetPublisher(String environment, PublicationDataSubset dataSubset);
    }
}
