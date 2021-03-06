﻿using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using TME.CarConfigurator.Assets;
using TME.CarConfigurator.DI;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared.Exceptions;
using TME.CarConfigurator.S3.Shared.Interfaces;
using TME.CarConfigurator.Tests.Shared;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.Services.GivenAnAssetService
{
    public class WhenGetSubModelAssetsIsCalledForNonExistent3DAssets : TestBase
    {
        private IEnumerable<Repository.Objects.Assets.Asset> _assets;
        private IAssetService _assetService;
        private Context _context;
        private ISerialiser _serialiser;

        protected override void Arrange()
        {
            var view = "the view";
            var mode = "the mode";

            _context = new ContextBuilder().Build();

            const string s3Key = "fake s3 key";

            _serialiser = A.Fake<ISerialiser>();
            var service = A.Fake<IService>();
            var keyManager = A.Fake<IKeyManager>();

            A.CallTo(() => keyManager.GetSubModelAssetsKey(A<Guid>._, A<Guid>._, A<Guid>._, view, mode)).Returns(s3Key);
            A.CallTo(() => service.GetObject(_context.Brand, _context.Country, s3Key)).Throws(new ObjectNotFoundException(null, s3Key));

            var serviceFacade = new S3ServiceFacade()
                .WithService(service)
                .WithSerializer(_serialiser)
                .WithKeyManager(keyManager);

            _assetService = serviceFacade.CreateAssetService();
        }

        protected override void Act()
        {
            _assets = _assetService.GetSubModelAssets(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _context);
        }

        [Fact]
        public void ThenItShouldReturnTheCorrectListOfAssets()
        {
            _assets.Should().BeEmpty();
        }

        [Fact]
        public void ThenDeserialiseShouldNotHappen()
        {
            A.CallTo(() => _serialiser.Deserialise<IEnumerable<Asset>>(A<String>._)).MustNotHaveHappened();
        }
    }
}