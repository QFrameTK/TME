﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public class Engine : BaseObject, IEngine
    {
        #region Dependencies (Adaptee)
        private Legacy.Engine Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public Engine(Legacy.Engine adaptee) : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion
        

        public IEngineType Type
        {
            get { return new EngineType(Adaptee);}
        }

        public IEngineCategory Category
        {
            get { return new EngineCategory(Adaptee.Category); }
        }

        public bool KeyFeature
        {
            get { return Adaptee.KeyFeature; }
        }

        public bool Brochure
        {
            get { return Adaptee.Brochure; }
        }

        public bool VisibleInExteriorSpin
        {
            get { return Adaptee.VisibleInExteriorSpin; }
        }

        public bool VisibleInInteriorSpin
        {
            get { return Adaptee.VisibleInInteriorSpin; }
        }

        public bool VisibleInXRay4X4Spin
        {
            get { return Adaptee.VisibleInXRay4x4Spin; }
        }

        public bool VisibleInXRayHybridSpin
        {
            get { return Adaptee.VisibleInXRayHybridSpin; }
        }

        public bool VisibleInXRaySafetySpin
        {
            get { return Adaptee.VisibleInXRaySafetySpin; }
        }

        public IEnumerable<IAsset> Assets
        {
            get { return Adaptee.Assets.Cast<Legacy.Asset>().Select(x => new Asset(x)); }
        }
    }
}