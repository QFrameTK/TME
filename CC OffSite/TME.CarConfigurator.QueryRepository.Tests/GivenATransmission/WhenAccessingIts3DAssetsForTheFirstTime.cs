﻿using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Query.Tests.TestBuilders;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Tests.Shared;
using TME.CarConfigurator.Tests.Shared.TestBuilders;
using Xunit;

namespace TME.CarConfigurator.Query.Tests.GivenATransmission
{
    public class WhenAccessingIts3DAssetsForTheFirstTime: TestBase
    {
        private ITransmission _transmission;
        private IEnumerable<IAsset> _assets;
        private const string MODE = "MyMode";
        private const string VIEW = "MyView";
        private Repository.Objects.Assets.Asset _asset1;
        private Repository.Objects.Assets.Asset _asset2;
        private IAssetService _assetService;

        protected override void Arrange()
        {
            _asset1 = new AssetBuilder().WithId(Guid.NewGuid()).Build();
            _asset2 = new AssetBuilder().WithId(Guid.NewGuid()).Build();

            var transmission = new TransmissionBuilder()
                .WithId(Guid.NewGuid())
                .AddVisibleIn(VIEW,MODE, true)
                .Build();

            var publicationTimeFrame = new PublicationTimeFrameBuilder()
                .WithDateRange(DateTime.MinValue,DateTime.MaxValue)
                .Build();

            var publication = new PublicationBuilder()
                .WithID(Guid.NewGuid())
                .AddTimeFrame(publicationTimeFrame)
                .Build();

            var context = new ContextBuilder().Build();

            var transmissionService = A.Fake<ITransmissionService>();
            A.CallTo(() => transmissionService.GetTransmissions(A<Guid>._, A<Guid>._, A<Context>._))
                .Returns(new List<Repository.Objects.Transmission>(){transmission});

            _assetService = A.Fake<IAssetService>();
            A.CallTo(() => _assetService.GetAssets(publication.ID,transmission.ID,context,VIEW,MODE))
                .Returns(new List<Repository.Objects.Assets.Asset>() {_asset1, _asset2});

            var assetFactory = new AssetFactoryBuilder()
                .WithAssetService(_assetService)
                .Build();

            var transmissionFactory = new TransmissionFactoryBuilder()
                .WithAssetFactory(assetFactory)
                .WithTransmissionService(transmissionService)
                .Build();

            _transmission = transmissionFactory.GetTransmissions(publication, context).Single();
        }

        protected override void Act()
        {
            _assets = _transmission.VisibleIn.Single(v => v.Mode == MODE && v.View == VIEW).Assets;
        }

        [Fact]
        public void ThenItShouldFetchTheAssetsFromTheService()
        {
            A.CallTo(() => _assetService.GetAssets(A<Guid>._,A<Guid>._,A<Context>._,A<string>._,A<string>._)).MustHaveHappened(Repeated.Exactly.Once);
        }
        
        [Fact]
        public void ThenItShouldHaveTheCorrectAssets()
        {
            _assets.Should().HaveCount(2);

            _assets.Should().Contain(a => a.ID == _asset1.ID);
            _assets.Should().Contain(a => a.ID == _asset2.ID);
        }
    }
}