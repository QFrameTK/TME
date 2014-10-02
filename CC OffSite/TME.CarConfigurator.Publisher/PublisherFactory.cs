﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TME.CarConfigurator.Publisher
{

    public interface IPublisherFactory
    {
        IPublisher Get(String target);
    }

    public class PublisherFactory : IPublisherFactory
    {
        public IPublisher Get(String target)
        {
            throw new NotImplementedException();
        }
    }
}