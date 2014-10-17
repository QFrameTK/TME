﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Assets;
using TME.CarConfigurator.Repository.Objects.Core;

namespace TME.CarConfigurator.Tests.Shared.TestBuilders
{
    public class CarBuilder
    {
        private readonly Car _car;

        public CarBuilder()
        {
            _car = new Car();
        }

        public CarBuilder WithId(Guid id)
        {
            _car.ID = id;

            return this;
        }

        public CarBuilder WithLabels(params Repository.Objects.Core.Label[] labels)
        {
            _car.Labels = labels.ToList();

            return this;
        }

        public CarBuilder WithBasePrice(Price price)
        {
            _car.BasePrice = price;

            return this;
        }

        public CarBuilder WithStartingPrice(Price price)
        {
            _car.StartingPrice = price;

            return this;
        }

        public CarBuilder WithBodyType(BodyType bodyType)
        {
            _car.BodyType = bodyType;

            return this;
        }

        public Car Build()
        {
            return _car;
        }
    }
}
