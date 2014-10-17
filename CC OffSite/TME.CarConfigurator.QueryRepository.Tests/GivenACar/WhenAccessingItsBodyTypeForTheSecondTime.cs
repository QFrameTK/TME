﻿using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.GivenACar
{
    public class WhenAccessingItsBodyTypeForTheSecondTime : TestBase
    {
        ICar _car;
        IBodyType _firstBodyType;
        IBodyType _secondBodyType;
        Repository.Objects.BodyType _repoBodyType;

        protected override void Arrange()
        {
            _repoBodyType = new CarConfigurator.Tests.Shared.TestBuilders.BodyTypeBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            var repoCar = new CarBuilder()
                .WithBodyType(_repoBodyType)
                .Build();

            var publicationTimeFrame = new PublicationTimeFrameBuilder()
                .WithDateRange(DateTime.MinValue, DateTime.MaxValue)
                .Build();

            var publication = new PublicationBuilder()
                .WithID(Guid.NewGuid())
                .AddTimeFrame(publicationTimeFrame)
                .Build();

            var context = new ContextBuilder().Build();

            var carService = A.Fake<ICarService>();
            A.CallTo(() => carService.GetCars(A<Guid>._, A<Guid>._, A<Context>._)).Returns(new[] { repoCar });

            var bodyTypeFactory = new BodyTypeFactoryBuilder().Build();

            var carFactory = new CarFactoryBuilder()
                .WithCarService(carService)
                .WithBodyTypeFactory(bodyTypeFactory)
                .Build();

            _car = carFactory.GetCars(publication, context).Single();

            _firstBodyType = _car.BodyType;
        }

        protected override void Act()
        {
            _secondBodyType = _car.BodyType;
        }

        [Fact]
        public void ThenItShouldNotRecalculateTheBodyType()
        {
            _secondBodyType.Should().Be(_firstBodyType);
        }

        [Fact]
        public void ThenTheBodyTypeShouldBeCorrect()
        {
            _secondBodyType.ID.Should().Be(_repoBodyType.ID);
        }


    }
}
