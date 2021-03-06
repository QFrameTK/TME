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
    public class WhenAccessingItsWheelDriveForTheFirstTime : TestBase
    {
        ICar _car;
        private Repository.Objects.WheelDrive _repoWheelDrive;
        private IWheelDrive _wheelDrive;

        protected override void Arrange()
        {
            _repoWheelDrive = new WheelDriveBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            var repoCar = new CarBuilder()
                .WithWheelDrive(_repoWheelDrive)
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

            var wheelDriveFactory = new WheelDriveFactoryBuilder().Build();

            var carFactory = new CarFactoryBuilder()
                .WithCarService(carService)
                .WithWheelDriveFactory(wheelDriveFactory)
                .Build();

            _car = carFactory.GetCars(publication, context).Single();
        }

        protected override void Act()
        {
            _wheelDrive = _car.WheelDrive;
        }

        [Fact]
        public void ThenTheWheelDriveShouldBeCorrect()
        {
            _wheelDrive.ID.Should().Be(_repoWheelDrive.ID);
        }
    }
}