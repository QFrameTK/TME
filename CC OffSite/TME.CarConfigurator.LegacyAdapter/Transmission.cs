﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public class Transmission : BaseObject, ITransmission
    {
        #region Dependencies (Adaptee)
        private Legacy.Transmission Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public Transmission(Legacy.Transmission adaptee)
            : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion
        

        public ITransmissionType Type
        {
            get { return new TransmissionType(Adaptee.Type);}
        }


        public bool KeyFeature
        {
            get { return Adaptee.KeyFeature; }
        }

        public bool Brochure
        {
            get { return Adaptee.Brochure; }
        }

        public int NumberOfGears
        {
            get { return Adaptee.NumberOfGears; }
        }

        public bool VisibleInExteriorSpin
        {
            get { throw new NotImplementedException(); }
        }

        public bool VisibleInInteriorSpin
        {
            get { throw new NotImplementedException(); }
        }

        public bool VisibleInXRay4X4Spin
        {
            get { throw new NotImplementedException(); }
        }

        public bool VisibleInXRayHybridSpin
        {
            get { throw new NotImplementedException(); }
        }

        public bool VisibleInXRaySafetySpin
        {
            get { throw new NotImplementedException(); }
        }


        public IEnumerable<IAsset> Assets
        {
            get { return Adaptee.Assets.Cast<Legacy.Asset>().Select(x => new Asset(x)); }
        }
    }
}