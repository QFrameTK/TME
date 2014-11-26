using System;
using FluentAssertions;
using TME.CarConfigurator.S3.Shared;
using TME.CarConfigurator.S3.Shared.Interfaces;
using TME.CarConfigurator.Tests.Shared;
using Xunit;

namespace TME.Carconfigurator.Tests.GivenAKeyManager
{
    public class WhenRequestingAnEquipmentCategoriesKey : TestBase
    {
        private IKeyManager _keyManager;
        private string _expectedKey;
        private string _actualKey;
        private Guid _publicationId;

        protected override void Arrange()
        {
            _keyManager = new KeyManager();

            _publicationId = Guid.NewGuid();

            _expectedKey = "publication/" + _publicationId + "/equipment-categories.json";
        }

        protected override void Act()
        {
            _actualKey = _keyManager.GetEquipmentCategoriesKey(_publicationId);
        }

        [Fact]
        public void ThenTheKeyShouldBeCorrect()
        {
            _actualKey.Should().Be(_expectedKey);
        }
    }
}