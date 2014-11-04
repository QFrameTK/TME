﻿using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Assets;

namespace TME.CarConfigurator.Interfaces
{
    public interface ICarPart
    {
         string Code { get; }
         string Name { get; }

         IReadOnlyList<IVisibleInModeAndView> VisibleIn { get; }
    }
}