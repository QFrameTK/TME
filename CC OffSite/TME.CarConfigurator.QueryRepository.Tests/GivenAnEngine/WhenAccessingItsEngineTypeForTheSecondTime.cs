﻿using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.GivenAnEngine
{
    public class WhenAccessingItsEngineTypeForTheSecondTime : TestBase
    {
        IEngine _engine;
        IEngineType _firstType;
        IEngineType _secondType;
        Repository.Objects.EngineType _repoType;

        protected override void Arrange()
        {
            _repoType = new EngineTypeBuilder()
                .WithCode("engine type code")
                .Build();

            var repoEngine = new EngineBuilder()
                .WithType(_repoType)
                .Build();

            var publicationTimeFrame = new PublicationTimeFrameBuilder()
                .WithDateRange(DateTime.MinValue, DateTime.MaxValue)
                .Build();

            var publication = new PublicationBuilder()
                .WithID(Guid.NewGuid())
                .AddTimeFrame(publicationTimeFrame)
                .Build();

            var context = new ContextBuilder().Build();

            var engineService = A.Fake<IEngineService>();
            A.CallTo(() => engineService.GetEngines(A<Guid>._, A<Guid>._, A<Context>._)).Returns(new List<Repository.Objects.Engine> { repoEngine });

            var engineFactory = new EngineFactoryBuilder()
                .WithEngineService(engineService)
                .Build();

            _engine = engineFactory.GetEngines(publication, context).Single();

            _firstType = _engine.Type;
        }

        protected override void Act()
        {
            _secondType = _engine.Type;
        }

        [Fact]
        public void ThenItShouldNotRecalculateTheType()
        {
            _secondType.Should().Be(_firstType);
        }

        [Fact]
        public void ThenItShouldHaveTheType()
        {
            _secondType.Code.Should().Be(_repoType.Code);
        }
    }
}
