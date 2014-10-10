﻿using System;
using System.Linq;
using FakeItEasy;
using TME.CarConfigurator.Publisher;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.Repository.Objects;
using TME.Carconfigurator.Tests.Builders;
using TME.CarConfigurator.Tests.Shared;
using System.Threading.Tasks;
using TME.CarConfigurator.S3.Shared.Result;
using System.Collections.Generic;
using TME.CarConfigurator.S3.Shared.Interfaces;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.CommandServices;

namespace TME.Carconfigurator.Tests.Base
{
    public abstract class PublicationTestBase : TestBase
    {
        protected IPublicationService PublicationService;
        protected IModelService PutModelService;
        protected CarConfigurator.QueryServices.IModelService GetModelService;
        protected IBodyTypeService BodyTypeService;
        protected IEngineService EngineService;
        protected Publisher Publisher;
        protected ISerialiser Serialiser;
        protected IContext Context;
        protected String Brand = "Toyota";
        protected String Country = "BE";
        protected String[] Languages = { "nl", "fr", "de", "en" };

        protected String GuidRegexPattern = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b";


        protected override void Arrange()
        {
            PublicationService = A.Fake<IPublicationService>(x => x.Strict());
            PutModelService = A.Fake<IModelService>(x => x.Strict());
            GetModelService = A.Fake<CarConfigurator.QueryServices.IModelService>(x => x.Strict());
            BodyTypeService = A.Fake<IBodyTypeService>(x => x.Strict());
            EngineService = A.Fake<IEngineService>(x => x.Strict());

            var successFullTask = Task.FromResult((Result)new Successfull());
            var successFullTasks = Task.FromResult((IEnumerable<Result>)new[] { new Successfull() });

            Serialiser = A.Fake<ISerialiser>();

            Publisher = new Publisher(PublicationService, PutModelService, GetModelService, BodyTypeService, EngineService);
            Context = ContextBuilder.GetDefaultContext(Languages);

            A.CallTo(() => Serialiser.Serialise((Publication)null)).WithAnyArguments().ReturnsLazily(args => args.Arguments.First().GetType().Name);
            A.CallTo(() => PutModelService.PutModelsByLanguage(null, null)).WithAnyArguments().Returns(successFullTask);
            A.CallTo(() => GetModelService.GetModelsByLanguage(Context.Brand, Context.Country)).Returns(new Languages());
            A.CallTo(() => PublicationService.PutPublications(null)).WithAnyArguments().Returns(successFullTasks);
            A.CallTo(() => BodyTypeService.PutGenerationBodyTypes(null)).WithAnyArguments().Returns(successFullTasks);
            A.CallTo(() => EngineService.PutGenerationEngines(null)).WithAnyArguments().Returns(successFullTasks);

        }

        protected override void Act()
        {
            var result = Publisher.Publish(Context).Result;
        }
    }
}
