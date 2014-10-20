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

namespace TME.Carconfigurator.Tests.GivenAS3WheelDrivePublisher
{
    public class WhenPublishingGenerationWheelDrives : TestBase
    {
        const String _brand = "Toyota";
        const String _country = "DE";
        const String _serialisedWheelDrive1 = "serialised wheelDrive 1";
        const String _serialisedWheelDrive12 = "serialised wheelDrive 1+2";
        const String _serialisedWheelDrive34 = "serialised wheelDrive 3+4";
        const String _serialisedWheelDrive4 = "serialised wheelDrive 4";
        const String _language1 = "lang 1";
        const String _language2 = "lang 2";
        const String _timeFrame1WheelDrivesKey = "time frame 1 wheelDrives key";
        const String _timeFrame2WheelDrivesKey = "time frame 2 wheelDrives key";
        const String _timeFrame3WheelDrivesKey = "time frame 3 wheelDrives key";
        const String _timeFrame4WheelDrivesKey = "time frame 4 wheelDrives key";
        IService _s3Service;
        IWheelDriveService _service;
        IWheelDrivePublisher _publisher;
        IContext _context;

        protected override void Arrange()
        {
            var wheelDriveId1 = Guid.NewGuid();
            var wheelDriveId2 = Guid.NewGuid();
            var wheelDriveId3 = Guid.NewGuid();
            var wheelDriveId4 = Guid.NewGuid();

            var car1 = new Car { WheelDrive = new WheelDrive { ID = wheelDriveId1 } };
            var car2 = new Car { WheelDrive = new WheelDrive { ID = wheelDriveId2 } };
            var car3 = new Car { WheelDrive = new WheelDrive { ID = wheelDriveId3 } };
            var car4 = new Car { WheelDrive = new WheelDrive { ID = wheelDriveId4 } };

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

            var generationWheelDrive1 = new WheelDrive { ID = wheelDriveId1 };
            var generationWheelDrive2 = new WheelDrive { ID = wheelDriveId2 };
            var generationWheelDrive3 = new WheelDrive { ID = wheelDriveId3 };
            var generationWheelDrive4 = new WheelDrive { ID = wheelDriveId4 };

            _context = ContextBuilder.InitialiseFakeContext()
                        .WithBrand(_brand)
                        .WithCountry(_country)
                        .WithLanguages(_language1, _language2)
                        .WithPublication(_language1, publication1)
                        .WithPublication(_language2, publication2)
                        .WithCars(_language1, car1, car2)
                        .WithCars(_language2, car3, car4)
                        .WithTimeFrames(_language1, timeFrame1, timeFrame2)
                        .WithTimeFrames(_language2, timeFrame3, timeFrame4)
                        .WithWheelDrives(_language1, generationWheelDrive1, generationWheelDrive2)
                        .WithWheelDrives(_language2, generationWheelDrive3, generationWheelDrive4)
                        .Build();

            _s3Service = A.Fake<IService>();

            var serialiser = A.Fake<ISerialiser>();
            var keyManager = A.Fake<IKeyManager>();

            _service = new WheelDriveService(_s3Service, serialiser, keyManager);
            _publisher = new WheelDrivePublisher(_service);

            A.CallTo(() => serialiser.Serialise((IEnumerable<WheelDrive>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationWheelDrive1))
                .Returns(_serialisedWheelDrive1);
            A.CallTo(() => serialiser.Serialise((IEnumerable<WheelDrive>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationWheelDrive1, generationWheelDrive2))
                .Returns(_serialisedWheelDrive12);
            A.CallTo(() => serialiser.Serialise((IEnumerable<WheelDrive>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationWheelDrive3, generationWheelDrive4))
                .Returns(_serialisedWheelDrive34);
            A.CallTo(() => serialiser.Serialise((IEnumerable<WheelDrive>)null))
                .WhenArgumentsMatch(ArgumentMatchesList(generationWheelDrive4))
                .Returns(_serialisedWheelDrive4);

            A.CallTo(() => keyManager.GetWheelDrivesKey(publication1.ID, publicationTimeFrame1.ID)).Returns(_timeFrame1WheelDrivesKey);
            A.CallTo(() => keyManager.GetWheelDrivesKey(publication1.ID, publicationTimeFrame2.ID)).Returns(_timeFrame2WheelDrivesKey);
            A.CallTo(() => keyManager.GetWheelDrivesKey(publication2.ID, publicationTimeFrame3.ID)).Returns(_timeFrame3WheelDrivesKey);
            A.CallTo(() => keyManager.GetWheelDrivesKey(publication2.ID, publicationTimeFrame4.ID)).Returns(_timeFrame4WheelDrivesKey);
        }

        protected override void Act()
        {
            var result = _publisher.PublishGenerationWheelDrives(_context).Result;
        }

        [Fact]
        public void ThenGenerationWheelDrivesShouldBePutForAllLanguagesAndTimeFrames()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(null, null, null, null))
                .WithAnyArguments()
                .MustHaveHappened(Repeated.Exactly.Times(4));
        }

        [Fact]
        public void ThenGenerationWheelDrivesShouldBePutForTimeFrame1()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame1WheelDrivesKey, _serialisedWheelDrive1))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationWheelDrivesShouldBePutForTimeFrame2()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame2WheelDrivesKey, _serialisedWheelDrive12))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationWheelDrivesShouldBePutForTimeFrame3()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame3WheelDrivesKey, _serialisedWheelDrive34))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void ThenGenerationWheelDrivesShouldBePutForTimeFrame4()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _timeFrame4WheelDrivesKey, _serialisedWheelDrive4))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
