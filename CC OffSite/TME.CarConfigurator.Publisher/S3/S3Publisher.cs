﻿using System;
using System.Linq;
using TME.CarConfigurator.Publisher.Enums.Result;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Enums;

namespace TME.CarConfigurator.Publisher.S3
{
    public class S3Publisher : IPublisher
    {
        IS3Service _service;
        IS3Serialiser _serialiser;

        String _publicationPathTemplate = "{0}/generation/{1}";

        public S3Publisher(IS3Service service, IS3Serialiser serialiser)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (serialiser == null) throw new ArgumentNullException("serialiser");

            _service = service;
            _serialiser = serialiser;
        }

        public void Publish(IContext context)
        {
            var languages = context.ContextData.Keys;

            foreach (var language in languages)
            { 
                PublishLanguage(language, context);
            }

            foreach (var language in languages)
            {
                var s3Models = _service.GetModelsOverview(context.Brand, context.Country, language);
                var contextModel = context.ContextData[language].Models.Single();
                var s3Model = s3Models.SingleOrDefault(m => m.ID == contextModel.ID);
               
                if (s3Model == null)
                {
                    s3Models.Add(contextModel);
                }
                else
                {
                    s3Model.Publications.Single(e => e.State == PublicationState.Activated).State = PublicationState.ToBeDeleted;
                    s3Model.Publications.Add(contextModel.Publications.Single());
                }
                
                _service.PutModelsOverview(context.Brand, context.Country, language, s3Models);
            }
        }

        void PublishLanguage(String language, IContext context)
        {
            PublishPublication(language, context);

            // publish rest
        }

        void PublishPublication(String language, IContext context)
        {
            var data = context.ContextData[language];
            var timeFrames = context.TimeFrames[language];
            var publication = new Publication
            {
                ID = Guid.NewGuid(),
                Generation = data.Generations.Single(),
                LineOffFrom = timeFrames.First().From,
                LineOffTo = timeFrames.Last().Until,
                TimeFrames = timeFrames.Select(timeFrame => new PublicationTimeFrame
                {
                    ID = timeFrame.ID,
                    LineOffFrom = timeFrame.From,
                    LineOffTo = timeFrame.Until
                })
                                       .ToList(),
                PublishedOn = DateTime.Now
            };

            _service.PutObject(String.Format(_publicationPathTemplate, language, publication.ID),
                               _serialiser.Serialise(publication));

            data.Models.Single().Publications.Add(new PublicationInfo(publication));
        }
    }
}
