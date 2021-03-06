﻿using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.GivenACar
{
    public class WhenAccessingItsSteeringForTheSecondTime : TestBase
    {
        ICar _car;
        private Repository.Objects.Steering _repoSteering;
        private ISteering _secondSteering;
        private ISteering _firstSteering;

        protected override void Arrange()
        {
            _repoSteering = new SteeringBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            var repoCar = new CarBuilder()
                .WithSteering(_repoSteering)
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

            var steeringFactory = new SteeringFactoryBuilder().Build();

            var carFactory = new CarFactoryBuilder()
                .WithCarService(carService)
                .WithSteeringFactory(steeringFactory)
                .Build();

            _car = carFactory.GetCars(publication, context).Single();

            _firstSteering = _car.Steering;
        }

        protected override void Act()
        {
            _secondSteering = _car.Steering;
        }

        [Fact]
        public void ThenItShouldNotRecalculateTheSteering()
        {
            _secondSteering.Should().BeSameAs(_firstSteering);
        }

        [Fact]
        public void ThenTheSteeringShouldBeCorrect()
        {
            _secondSteering.ID.Should().Be(_repoSteering.ID);
        }
    }
}