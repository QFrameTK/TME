﻿using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Enums;

namespace TME.CarConfigurator.Interfaces.Equipment
{
    public interface IEquipmentItem : IBaseObject
    {
        int ShortID { get; }
        string InternalName { get; }
        string PartNumber { get; }
        string Path {get; }

        bool KeyFeature { get; }
        bool GradeFeature { get; }
        bool OptionalGradeFeature { get; }
        bool Brochure { get; }

        Visibility Visibility { get; }
 
        ICategoryInfo Category { get; }
        IExteriorColour ExteriorColour { get; }
        
        IEnumerable<IAsset> Assets { get; }
        IEnumerable<ILink> Links { get; }
    }
}