﻿using FakeItEasy;
using System;
using System.Collections.Generic;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.Carconfigurator.Tests.Builders;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared.Interfaces;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.S3.CommandServices;
using TME.CarConfigurator.S3.Publisher;

namespace TME.Carconfigurator.Tests.GivenAS3GradePublisher
{
    public class WhenPublishingGenerationGrades : TestBase
    {
        const String _brand = "Toyota";
        const String _country = "DE";
        const String _serialisedGrade1 = "serialised grade 1";
        const String _serialisedGrade12 = "serialised grade 1+2";
        const String _serialisedGrade34 = "serialised grade 3+4";
        const String _serialisedGrade4 = "serialised grade 4";
        const String _language1 = "lang 1";
        const String _language2 = "lang 2";
        const String _timeFrame1GradesKey = "time frame 1 grades key";
        const String _timeFrame2GradesKey = "time frame 2 grades key";
        const String _timeFrame3GradesKey = "time frame 3 grades key";
        const String _timeFrame4GradesKey = "time frame 4 grades key";
        IService _s3Service;
        IGradeService _service;
        IGradePublisher _publisher;
        IContext _context;

        protected override void Arrange()
        {
            var gradeId1 = Guid.NewGuid();
            var gradeId2 = Guid.NewGuid();
            var gradeId3 = Guid.NewGuid();
            var gradeId4 = Guid.NewGuid();

            var car1 = new Car { Grade = new Grade { ID = gradeId1 } };
            var car2 = new Car { Grade = new Grade { ID = gradeId2 } };
            var car3 = new Car { Grade = new Grade { ID = gradeId3 } };
            var car4 = new Car { Grade = new Grade { ID = gradeId4 } };

            var timeFrame1 = new TimeFrame(DateTime.MinValue, DateTime.MaxValue, new[] { car1 });
            var timeFrame2 = new TimeFrame(DateTime.MinValue, DateTime.MaxValue, new[] { car1, car2 });
            var timeFrame3 = new TimeFrame(DateTime.MinValue, DateTime.MaxValue, new[] { car3, car4 });
            var timeFrame4 = new TimeFrame(DateTime.MinValue, DateTime.MaxValue, new[] { car4 });

            var publicationTimeFrame1 = new PublicationTimeFrame { ID = timeFrame1.ID };
            var publicationTimeFrame2 = new PublicationTimeFrame { ID = timeFrame2.ID };
            var publicationTimeFrame3 = new PublicationTimeFrame { ID = timeFrame3.ID };
            var publicationTimeFrame4 = new PublicationTimeFrame { ID = timeFrame4.ID };

            var publication1 = PublicationBuilder.Initialize()
                                                 .WithTimeFrames(publicationTimeFrame1,
                                                                 publicationTimeFrame2)
                                                 .Build();

            var publication2 = PublicationBuilder.Initialize()
                                                 .WithTimeFrames(publicationTimeFrame3,
                                                                 publicationTimeFrame4)
                                                 .Build();

            var generationGrade1 = new Grade { ID = gradeId1 };
            var generationGrade2 = new Grade { ID = gradeId2 };
            var generationGrade3 = new Grade { ID = gradeId3 };
            var generationGrade4 = new Grade { ID = gradeId4 };

            _context = new ContextBuilder()
                        .WithBrand(_brand)
                        .WithCountry(_country)
                        .WithLanguages(_language1, _language2)
                        .WithPublication(_language1, publication1)
                        .WithPublication(_language2, publication2)
                        .WithCars(_language1, car1, car2)
                        .WithCars(_language2, car3, car4)
                        .WithTimeFrames(_language1, timeFrame1, timeFrame2)
                        .WithTimeFrames(_language2, timeFrame3, timeFrame4)
                        .WithGrades(_language1, generationGrade1, generationGrade2)
                        .WithGrades(_language2, generationGrade3, generationGrade4)
                        .Build();

            _s3Service = A.Fake<IService>();

            var serialiser = A.Fake<ISerialiser>();
            var keyManager = A.Fake<IKeyManager>();

            _service = new GradeService(_s3Service, serialiser, keyManager);
            _publisher = new GradePublisher(_service);

            A.CallTo(() => serialiser.Serialise((IEnumerable<Grade>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationGrade1))
                .Returns(_serialisedGrade1);
            A.CallTo(() => serialiser.Serialise((IEnumerable<Grade>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationGrade1, generationGrade2))
                .Returns(_serialisedGrade12);
            A.CallTo(() => serialiser.Serialise((IEnumerable<Grade>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationGrade3, generationGrade4))
                .Returns(_serialisedGrade34);
            A.CallTo(() => serialiser.Serialise((IEnumerable<Grade>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationGrade4))
                .Returns(_serialisedGrade4);

            A.CallTo(() => keyManager.GetGradesKey(publication1.ID, publicationTimeFrame1.ID)).Returns(_timeFrame1GradesKey);
            A.CallTo(() => keyManager.GetGradesKey(publication1.ID, publicationTimeFrame2.ID)).Returns(_timeFrame2GradesKey);
            A.CallTo(() => keyManager.GetGradesKey(publication2.ID, publicationTimeFrame3.ID)).Returns(_timeFrame3GradesKey);
            A.CallTo(() => keyManager.GetGradesKey(publication2.ID, publicationTimeFrame4.ID)).Returns(_timeFrame4GradesKey);
        }

        protected override void Act()
        {
            var result = _publisher.PublishGenerationGrades(_context).Result;
        }

        [Fact]
        public void ThenGenerationGradesShouldBePutForAllLanguagesAndTimeFrames()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(null, null, null, null))
                .WithAnyArguments()
                .MustHaveHappened(Repeated.Exactly.Times(4));
        }

        [Fact]
        public void ThenGenerationGradesShouldBePutForTimeFrame1()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame1GradesKey, _serialisedGrade1))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationGradesShouldBePutForTimeFrame2()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame2GradesKey, _serialisedGrade12))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationGradesShouldBePutForTimeFrame3()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame3GradesKey, _serialisedGrade34))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationGradesShouldBePutForTimeFrame4()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame4GradesKey, _serialisedGrade4))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}