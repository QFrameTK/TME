﻿using System;

namespace TME.CarConfigurator.Interfaces.TechnicalSpecifications
{
    public interface ICategoryInfo
    {
        Guid ID { get; }
        String Path { get; }
        int SortIndex { get; }
    }
}
