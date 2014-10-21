﻿using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Interfaces.Enums;
using TME.CarConfigurator.Interfaces.Equipment;
using TME.CarConfigurator.LegacyAdapter.Extensions;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public abstract class EquipmentItem : BaseObject, IEquipmentItem
    {

        #region Dependencies (Adaptee)
        private Legacy.CarEquipmentItem Adaptee
        {
            get;
            set;
        }
        private Legacy.Generation GenerationOfAdaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        protected EquipmentItem(Legacy.CarEquipmentItem adaptee, Legacy.Generation generationOfAdaptee)
            : base(adaptee)
        {
            Adaptee = adaptee;
            GenerationOfAdaptee = generationOfAdaptee;
        }
        #endregion

        public int ShortID
        {
            get { return Adaptee.ShortID; }
        }

        public string InternalName
        {
            get { return Adaptee.InternalName; }
        }

        public string PartNumber
        {
            get { return Adaptee.PartNumber; }
        }

        public string Path
        {
            get { return Adaptee.Path; }
        }

        public bool KeyFeature
        {
            get { return Adaptee.KeyFeature; }
        }

        public bool GradeFeature
        {
            get { return Adaptee.GradeFeature; }
        }

        public bool OptionalGradeFeature
        {
            get { return Adaptee.OptionalGradeFeature; }
        }

        public bool Brochure
        {
            get { return Adaptee.Brochure; }
        }

        public Visibility Visibility
        {
            get { return Adaptee.Visibility.ToVisibility(); }
        }

        public ICategoryInfo Category
        {
            get { return new CategoryInfo(Adaptee.Category); }
        }

        public IExteriorColour ExteriorColour
        {
            get
            {
                var colour = Adaptee.Colour;
                if (colour.IsEmpty()) return null;

                
                var carColour = GenerationOfAdaptee.ExteriorColours[colour.ID];
                if (carColour == null) return null;
                return new ExteriorColour(carColour);
            }
        }

        public IEnumerable<IAsset> Assets
        {
            get { return Adaptee.Assets.GetPlainAssets(); }
        }

        public IEnumerable<ILink> Links
        {
            get { return Adaptee.Links.Cast<TMME.CarConfigurator.Link>().Select(x => new Link(x)); }
        }
    }
}