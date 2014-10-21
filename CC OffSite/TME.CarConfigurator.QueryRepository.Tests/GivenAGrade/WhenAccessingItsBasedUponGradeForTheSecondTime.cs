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
using TME.CarConfigurator.Repository.Objects.Core;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.GradeConfigurator.Query.Tests.GivenAGrade
{
    public class WhenAccessingItsStartingPriceForTheSecondTime : TestBase
    {
        IGrade _grade;
        IGrade _firstGrade;
        IGrade _secondGrade;
        Grade _repoBasedUponGrade;

        protected override void Arrange()
        {
            _repoBasedUponGrade = new GradeBuilder()
                            .WithId(Guid.NewGuid())
                            .Build();

            var repoGrade = new GradeBuilder()
                .WithId(Guid.NewGuid())
                .WithBasedUponGradeID(_repoBasedUponGrade.ID)
                .Build();

            var publicationTimeFrame = new PublicationTimeFrameBuilder()
                .WithDateRange(DateTime.MinValue, DateTime.MaxValue)
                .Build();

            var publication = new PublicationBuilder()
                .WithID(Guid.NewGuid())
                .AddTimeFrame(publicationTimeFrame)
                .Build();

            var context = new ContextBuilder().Build();

            var gradeService = A.Fake<IGradeService>();
            A.CallTo(() => gradeService.GetGrades(A<Guid>._, A<Guid>._, A<Context>._)).Returns(new[] { repoGrade, _repoBasedUponGrade });

            var gradeFactory = new GradeFactoryBuilder()
                .WithGradeService(gradeService)
                .Build();

            _grade = gradeFactory.GetGrades(publication, context).Single(grade => grade.ID == repoGrade.ID);
            
            _firstGrade = _grade.BasedUpon;
        }

        protected override void Act()
        {
            _secondGrade = _grade.BasedUpon;
        }

        [Fact]
        public void ThenItShouldNotRecalculateTheGrade()
        {
            _secondGrade.Should().Be(_firstGrade);
        }

        [Fact]
        public void ThenTheGradeShouldBeCorrect()
        {
            _secondGrade.ID.Should().Be(_repoBasedUponGrade.ID);
        }


    }
}