﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.S3.Publisher.Interfaces;
using TME.CarConfigurator.S3.Shared.Result;

namespace TME.CarConfigurator.S3.Publisher
{
    public class SteeringPublisher : ISteeringPublisher
    {
        readonly ISteeringService _steeringService;
        readonly ITimeFramePublishHelper _timeFramePublishHelper;

        public SteeringPublisher(ISteeringService steeringService, ITimeFramePublishHelper timeFramePublishHelper)
        {
            if (steeringService == null) throw new ArgumentNullException("steeringService");
            if (timeFramePublishHelper == null) throw new ArgumentNullException("timeFramePublishHelper");

            _steeringService = steeringService;
            _timeFramePublishHelper = timeFramePublishHelper;
        }

        public async Task<IEnumerable<Result>> PublishGenerationSteerings(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            return await _timeFramePublishHelper.PublishTimeFrameObjects(context, timeFrame => timeFrame.Steerings, _steeringService.PutTimeFrameGenerationSteerings);
        }
    }
}
