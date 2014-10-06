﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using FakeItEasy;
using TME.CarConfigurator.Administration.Translations;
using TME.CarConfigurator.Publisher;
using TME.CarConfigurator.Publisher.Enums;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.Publisher.S3;
using TME.CarConfigurator.Repository.Objects;
using TME.Carconfigurator.Tests.Base;
using TME.CarConfigurator.Tests.Shared;

namespace TME.Carconfigurator.Tests.GivenAS3Publisher
{
    public abstract class ActivatePublicationTestBase : TestBase
    {
        protected IS3Service Service;
        protected IPublisher Publisher;
        protected const string Brand = "Toyota";
        protected const string Country = "BE";
        protected const string Language1 = "nl";
        protected const string Language2 = "fr";
        protected Context Context;
        protected readonly Guid GenerationID = Guid.NewGuid();
        protected readonly Guid ModelID = Guid.NewGuid();
        protected const string ModelNameForLanguage1 = "ModelNameForLanguage1";
        protected const string ModelNameForLanguage2 = "ModelNameForLanguage2";
        protected const string InternalCodeForLanguage1 = "InternalCode1";
        protected const string LocalCodeForLanguage1 = "LocalCode1";
        protected const string DescriptionForLanguage1 = "Description";
        protected const string FootNoteForLanguage1 = "FootNote";
        protected const string TooltipForLanguage1 = "ToolTip";
        protected const int SortIndexForLanguage1 = 4;
        protected List<Label> LabelsForLanguage1 = new List<Label>()
        {
            new Label(){Code = "New Code 1",Value = "new value 1"},
            new Label(){Code = "New Code 2",Value = "new value 2"},
            new Label(){Code = "New Code 3",Value = "new value 3"}
        }; 

        protected override void Arrange()
        {
            Service = A.Fake<IS3Service>(x => x.Strict());
            var serialiser = A.Fake<IS3Serialiser>();
            Context = new Context(Brand, Country, GenerationID, PublicationDataSubset.Live);

            var contextDataForLanguage1 = new ContextData();
            contextDataForLanguage1.Models.Add(new Model
            {
                Name = ModelNameForLanguage1, 
                ID = ModelID,
                InternalCode = InternalCodeForLanguage1,
                LocalCode = LocalCodeForLanguage1,
                Description = DescriptionForLanguage1,
                FootNote = FootNoteForLanguage1,
                ToolTip = TooltipForLanguage1,
                SortIndex = SortIndexForLanguage1,
                Labels = LabelsForLanguage1
            });
            contextDataForLanguage1.Generations.Add(new Generation());

            var contextDataForLanguage2 = new ContextData();
            contextDataForLanguage2.Models.Add(new Model { Name = ModelNameForLanguage2, ID = ModelID});
            contextDataForLanguage2.Generations.Add(new Generation());

            Context.ContextData.Add(Language1, contextDataForLanguage1);
            Context.ContextData.Add(Language2, contextDataForLanguage2);

            var timeFrames = new List<TimeFrame> { new TimeFrame(DateTime.MinValue, DateTime.MaxValue, new List<Car>()) };

            Context.TimeFrames.Add(Language1, timeFrames);
            Context.TimeFrames.Add(Language2, timeFrames);

            A.CallTo(() => Service.PutModelsOverviewPerLanguage(null, null, null)).WithAnyArguments();
            A.CallTo(() => Service.PutObject(null, null)).WithAnyArguments();

            Publisher = new S3Publisher(Service, serialiser);
        }

        protected override void Act()
        {
            Publisher.Publish(Context);
        }

        protected Model GetModel(string modelName, string internalCode, string localCode, string oldDescriptionForLanguage1,string footNote,string tooltip,int sortIndex,List<Label> labels)
        {
            return new Model
            {
                Name = modelName,
                ID = ModelID,
                InternalCode = internalCode,
                LocalCode = localCode,
                Labels = labels,
                Publications =
                {
                    new PublicationInfo(new Publication{ID = Guid.NewGuid(),Generation = new Generation()})
                }
            };
        }
    }
}