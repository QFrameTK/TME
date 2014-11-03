﻿using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.LegacyAdapter.Extensions;

namespace TME.CarConfigurator.LegacyAdapter.Colours
{
    public class ExteriorColour : BaseObject, IExteriorColour
    {
        #region Dependencies (Adaptee)
        private TMME.CarConfigurator.CarExteriorColour Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public ExteriorColour(TMME.CarConfigurator.CarExteriorColour adaptee) : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion

        public bool Promoted
        {
            get { return Adaptee.IsPromoted; }
        }

        public IColourTransformation Transformation
        {
            get { return new ColourTransformation(Adaptee.Transformation);}
        }

        public IExteriorColourType Type
        {
            get { return new ExteriorColourType(Adaptee.Type); }
        }

        public IReadOnlyList<IAsset> Assets
        {
            get { return Adaptee.Assets.GetPlainAssets(); }
        }
    }
}
