﻿using FakeItEasy;
using System;
using TME.CarConfigurator.Publisher.Common.Interfaces;
using TME.CarConfigurator.S3.CommandServices;
using TME.Carconfigurator.Tests.Builders;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared.Interfaces;
using TME.CarConfigurator.Tests.Shared;
using Xunit;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.S3.Publisher;

namespace TME.Carconfigurator.Tests.GivenAS3LanguagePublisher
{
    public class WhenPublishingALanguage : TestBase
    {
        const String _brand = "Toyota";
        const String _country = "DE";
        const String _serialisedLanguages = "serialised languages";
        const String _languagesKey = "languages key";
        IService _s3Service;
        IModelService _service;
        IModelPublisher _publisher;
        IContext _context;
        Languages _languages;

        protected override void Arrange()
        {
            _context = new ContextBuilder()
                        .WithBrand(_brand)
                        .WithCountry(_country)
                        .Build();

            _languages = new Languages();

            _s3Service = A.Fake<IService>();

            var serialiser = A.Fake<ISerialiser>();
            var keyManager = A.Fake<IKeyManager>();

            _service = new ModelService(_s3Service, serialiser, keyManager);
            _publisher = new ModelPublisher(_service);

            A.CallTo(() => serialiser.Serialise(_languages)).Returns(_serialisedLanguages);
            A.CallTo(() => keyManager.GetModelsKey()).Returns(_languagesKey);
            
        }

        protected override void Act()
        {
           _publisher.PublishModelsByLanguage(_context, _languages).Wait();
        }

        [Fact]
        public void ThenLanguagesShouldBePut()
        {
            A.CallTo(() => _s3Service.PutObjectAsync(_brand, _country, _languagesKey, _serialisedLanguages))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
