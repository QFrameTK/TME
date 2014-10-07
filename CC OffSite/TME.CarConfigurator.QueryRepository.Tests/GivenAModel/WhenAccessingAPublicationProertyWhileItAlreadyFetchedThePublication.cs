using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using TME.CarConfigurator.Factories.Interfaces;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.QueryRepository.Interfaces;
using TME.CarConfigurator.QueryRepository.Tests.TestBuilders;
using TME.CarConfigurator.QueryRepository.Tests.TestBuilders.RepositoryObjects;
using TME.CarConfigurator.Tests.Shared;
using Xunit;

namespace TME.CarConfigurator.QueryRepository.Tests.GivenAModel
{
    public class WhenAccessingAPublicationProertyWhileItAlreadyFetchedThePublication : TestBase
    {
        private Guid _publicationID;
        private IModel _model;
        private string _actualSsn;
        private string _expectedSsn;
        private IPublicationFactory _publicationFactory;
        private IPublicationRepository _publicationRepository;

        protected override void Arrange()
        {
            _publicationID = Guid.NewGuid();
            _expectedSsn = "expected ssn";

            ArrangePublicationFactory();

            var modelFactory = ArrangeModelFactory();

            var context = ContextBuilder.InitializeFakeContext().Build();

            _model = modelFactory.Get(context).Single();

            var dumnmy = _model.SSN; // call publication property a first time
        }

        private void ArrangePublicationFactory()
        {
            _publicationRepository = A.Fake<IPublicationRepository>();

            A.CallTo(() => _publicationRepository.Get(_publicationID))
                .Returns(
                    PublicationBuilder.Initialize()
                        .WithGeneration(GenerationBuilder.Initialize()
                            .WithSsn(_expectedSsn)
                            .Build()
                        )
                        .Build());

            _publicationFactory = PublicationFactoryBuilder.Initialize()
                .WithPublicationRepository(_publicationRepository)
                .Build();
        }

        private IModelFactory ArrangeModelFactory()
        {
            var publicationInfo = PublicationInfoBuilder.Initialize()
                .WithID(_publicationID)
                .WithGeneration(GenerationBuilder.Initialize().Build())
                .CurrentlyActive()
                .Build();

            var repoModel = ModelBuilder.Initialize().AddPublication(publicationInfo).Build();

            var modelRepository = A.Fake<IModelRepository>();
            A.CallTo(() => modelRepository.Get(A<IContext>._)).Returns(new List<Repository.Objects.Model> { repoModel });

            return ModelFactoryBuilder.Initialize()
                .WithModelRepository(modelRepository)
                .WithPublicationFactory(_publicationFactory)
                .Build();
        }

        protected override void Act()
        {
            _actualSsn = _model.SSN; // call publication property a second time
        }

        [Fact]
        public void ThenItShouldNotFetchThePublicationAgain()
        {
            A.CallTo(() => _publicationRepository.Get(_publicationID)).MustHaveHappened(Repeated.Exactly.Once); // only fetch the publication once, no matter how many times the properties are being called
        }

        [Fact]
        public void ThenItShouldStillGetTheCorrectValue()
        {
            _actualSsn.Should().Be(_expectedSsn);
        }
    }
}