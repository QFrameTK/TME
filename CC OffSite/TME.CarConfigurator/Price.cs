﻿using System;
using TME.CarConfigurator.Interfaces.Core;

namespace TME.CarConfigurator
{
    public class Price : IPrice
    {
        private Repository.Objects.Core.Price _price;

        public Price(Repository.Objects.Core.Price price)
        {
            if (price == null) throw new ArgumentNullException("price");
            
            _price = price;
        }

        public Decimal PriceInVat { get { return _price.IncludingVat; } }
        public Decimal PriceExVat { get { return _price.ExcludingVat; } }
    }
}