﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.Publisher.Common.Result;
using TME.CarConfigurator.Publisher.Interfaces;

namespace TME.CarConfigurator.S3.Publisher
{
    public class BodyTypePublisher : IBodyTypePublisher
    {
        readonly IBodyTypeService _bodyTypeService;

        public BodyTypePublisher(IBodyTypeService bodyTypeService)
        {
            if (bodyTypeService == null) throw new ArgumentNullException("bodyTypeService");

            _bodyTypeService = bodyTypeService;
        }

        public async Task<IEnumerable<Result>> PublishGenerationBodyTypes(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var tasks = new List<Task<IEnumerable<Result>>>();

            foreach (var entry in context.ContextData)
            {
                var language = entry.Key;
                var data = entry.Value;
                var timeFrames = context.TimeFrames[language];

                tasks.Add(PutTimeFramesGenerationBodyTypes(context.Brand, context.Country, timeFrames, data));
            }

            var result = await Task.WhenAll(tasks);
            return result.SelectMany(xs => xs.ToList());
        }

        async Task<IEnumerable<Result>> PutTimeFramesGenerationBodyTypes(String brand, String country, IEnumerable<TimeFrame> timeFrames, ContextData data)
        {
            var publication = data.Publication;

            var bodyTypes = timeFrames.ToDictionary(
                                timeFrame => data.Publication.TimeFrames.Single(publicationTimeFrame => publicationTimeFrame.ID == timeFrame.ID),
                                timeFrame => data.BodyTypes.Where(bodyType =>
                                                                            timeFrame.Cars.Any(car => car.BodyType.ID == bodyType.ID))
                                                                     .OrderBy(bodyType => bodyType.SortIndex)
                                                                     .ThenBy(bodyType => bodyType.Name)
                                                                     .ToList());

            var tasks = bodyTypes.Select(entry => _bodyTypeService.PutTimeFrameGenerationBodyTypes(brand, country, publication.ID, entry.Key.ID, entry.Value)).ToList();

            return await Task.WhenAll(tasks);
        }
    }
}
