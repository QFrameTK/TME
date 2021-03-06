﻿using FakeItEasy;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.S3.Publisher;
using TME.CarConfigurator.S3.Publisher.Helpers;
using TME.CarConfigurator.S3.Publisher.Interfaces;

namespace TME.Carconfigurator.Tests.Builders
{
    public class SteeringPublisherBuilder
    {
        private ISteeringService _service = A.Fake<ISteeringService>();
        private ITimeFramePublishHelper _timeFramePublishHelper = new TimeFramePublishHelper();

        public SteeringPublisherBuilder WithService(ISteeringService service)
        {
            _service = service;

            return this;
        }

        public SteeringPublisher Build()
        {
            return new SteeringPublisher(_service, _timeFramePublishHelper);
        }
    }
}
