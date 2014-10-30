﻿using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.GivenAExteriorColour
{
    public class WhenAccessingItsExteriorColourTypeForTheSecondTime : TestBase
    {
        IExteriorColour _exteriorColour;
        IExteriorColourType _firstExteriorColourType;
        IExteriorColourType _secondExteriorColourType;
        Repository.Objects.Colours.ExteriorColourType _repoExteriorColourType;

        protected override void Arrange()
        {
            _repoExteriorColourType = new ExteriorColourTypeBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            var repoExteriorColour = new ExteriorColourBuilder()
                .WithExteriorColourType(_repoExteriorColourType)
                .Build();

            var publicationTimeFrame = new PublicationTimeFrameBuilder()
                .WithDateRange(DateTime.MinValue, DateTime.MaxValue)
                .Build();

            var publication = new PublicationBuilder()
                .WithID(Guid.NewGuid())
                .AddTimeFrame(publicationTimeFrame)
                .Build();

            var context = new ContextBuilder().Build();

            var colourFactory = new ColourFactoryBuilder()
                .Build();

            _exteriorColour = colourFactory.GetExteriorColour(repoExteriorColour);

            _firstExteriorColourType = _exteriorColour.Type;
        }

        protected override void Act()
        {
            _secondExteriorColourType = _exteriorColour.Type;
        }

        [Fact]
        public void ThenItShouldNotRecalculateTheType()
        {
            _secondExteriorColourType.Should().Be(_firstExteriorColourType);
        }

        [Fact]
        public void ThenItShouldHaveTheType()
        {
            _secondExteriorColourType.ID.Should().Be(_repoExteriorColourType.ID);
        }
    }
}