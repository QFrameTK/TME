﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public class Model : BaseObject, IModel
    {

        #region Dependencies (Adaptee)
        private Legacy.Model Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public Model(Legacy.Model adaptee) : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion
        

        public string Brand
        {
            get { return Adaptee.Brand;}
        }

        public string SSN
        {
            get { return Adaptee.Generation.SSN; }
        }

        public bool Promoted
        {
            get
            {
                return Legacy.Model.GetPromotedModel(Legacy.MyContext.CurrentContext).Equals(Adaptee); 
            }
        }

        public ICarConfiguratorVersion CarConfiguratorVersion
        {
            get { return new CarConfiguratorVersion(Adaptee.CarConfiguratorVersion); }
        }

        public IEnumerable<ILink> Links
        {
            get { return Adaptee.Links.Cast<Legacy.Link>().Select(x => new Link(x)); }
        }

        public IEnumerable<IAsset> Assets
        {
            get { return Adaptee.Assets.Cast<Legacy.Asset>().Select(x => new Asset(x)); }
        }

        public IEnumerable<IBodyType> BodyTypes
        {
            get { return Adaptee.BodyTypes.Cast<Legacy.BodyType>().Select(x => new BodyType(x)); }
        }

        public IEnumerable<IEngine> Engines
        {
            get { return Adaptee.Engines.Cast<Legacy.Engine>().Select(x => new Engine(x)); }
        }

        public IEnumerable<IFuelType> FuelTypes
        {
            get { return Adaptee.FuelTypes.Cast<Legacy.FuelType>().Select(x => new FuelType(x)); }
        }

        public IEnumerable<ICar> Cars
        {
            get { return Adaptee.Cars.Cast<Legacy.Car>().Select(x => new Car(x)); }
        }
    }
}