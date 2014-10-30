﻿using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Colours;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Interfaces.Factories
{
    public interface IColourFactory
    {
        IReadOnlyList<IColourCombination> GetColourCombinations(Publication publication, Context context);
        IUpholstery GetUpholstery(Repository.Objects.Colours.Upholstery repository);
        IExteriorColour GetExteriorColour(Repository.Objects.Colours.ExteriorColour repoExteriorColour);

    }
}