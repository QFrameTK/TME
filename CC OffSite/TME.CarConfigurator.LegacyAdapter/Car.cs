﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.Interfaces.Packs;
using TME.CarConfigurator.Interfaces.TechnicalSpecifications;
using TME.CarConfigurator.LegacyAdapter.Colours;
using TME.CarConfigurator.LegacyAdapter.Equipment;
using TME.CarConfigurator.LegacyAdapter.Packs;
using TME.CarConfigurator.LegacyAdapter.TechnicalSpecifications;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public class Car : BaseObject, ICar
    {
        #region Dependencies (Adaptee)
        private Legacy.Car Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public Car(Legacy.Car adaptee)
            : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion

        public int ShortID
        {
            get { return Adaptee.ShortID; }
        }

        public bool Promoted
        {
            get { return Adaptee.Promoted; }
        }

        public bool WebVisible
        {
            get { return Adaptee.WebVisible; }
        }

        public bool ConfigVisible
        {
            get { return Adaptee.ConfigVisible; }
        }

        public bool FinanceVisible
        {
            get { return Adaptee.FinanceVisible; }
        }

        public IPrice BasePrice
        {
            get { return new Price(Adaptee); }
        }

        public IPrice StartingPrice
        {
            get
            {
                var startingPrice = new Price(Adaptee);
                var cheapestColourPriceIncludingTax =
                    ColourCombinations
                        .OrderBy(x => x.ExteriorColour.Price.PriceInVat + x.Upholstery.Price.PriceInVat)
                        .Select(x => x.ExteriorColour.Price.PriceInVat + x.Upholstery.Price.PriceInVat)
                        .FirstOrDefault();

                var cheapestColourPriceExcludingTax =
                    ColourCombinations
                        .OrderBy(x => x.ExteriorColour.Price.PriceExVat + x.Upholstery.Price.PriceExVat)
                        .Select(x => x.ExteriorColour.Price.PriceExVat + x.Upholstery.Price.PriceExVat)
                        .FirstOrDefault();

                startingPrice.PriceInVat += cheapestColourPriceIncludingTax;
                startingPrice.PriceInVat += cheapestColourPriceExcludingTax;
                return startingPrice;

            }
        }

        public IBodyType BodyType
        {
            get { return new BodyType(Adaptee.BodyType, true); }
        }

        public IEngine Engine
        {
            get { return new Engine(Adaptee.Engine, true); }
        }

        public ITransmission Transmission
        {
            get { return new Transmission(Adaptee.Transmission, true); }
        }

        public IWheelDrive WheelDrive
        {
            get { return new WheelDrive(Adaptee.WheelDrive, true); }
        }

        public ISteering Steering
        {
            get { return new Steering(Adaptee.Steering); }
        }

        public IGrade Grade
        {
            get { return new Grade(Adaptee.Grade, true); }
        }
        public ISubModel SubModel
        {
            get
            {
                return Adaptee.SubModel != null && Adaptee.SubModel.ID != Guid.Empty
                    ? new SubModel(Adaptee.SubModel, true)
                    : null;
            }
        }

        public IReadOnlyList<ICarPart> Parts
        {
            get { return Adaptee.Parts.Cast<Legacy.CarPart>().Select(x => new CarPart(x)).ToList(); }
        }

        public ICarEquipment Equipment
        {
            get { return new CarEquipment(Adaptee); }
        }

        public IReadOnlyList<ICarPack> Packs
        {
            get { return Adaptee.Packs.Cast<Legacy.CarPack>().Select(x => new CarPack(x, Adaptee)).ToList(); }
        }

        public IReadOnlyList<ICarTechnicalSpecification> TechnicalSpecifications
        {
            get { return Adaptee.TechnicalSpecifications.Cast<Legacy.TechnicalSpecification>().Select(x => new CarTechnicalSpecification(x)).ToList(); }
        }

        public IReadOnlyList<ICarColourCombination> ColourCombinations
        {
            get
            {
                return
                    Adaptee.Colours.Cast<Legacy.CarColourCombination>()
                        .Select(x => new CarColourCombination(x))
                        .ToList();
            }
        }
    }

}
