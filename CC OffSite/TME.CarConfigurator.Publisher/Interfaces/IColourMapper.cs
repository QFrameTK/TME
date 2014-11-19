﻿using TME.CarConfigurator.Administration;
using TME.CarConfigurator.Repository.Objects.Colours;
using ExteriorColour = TME.CarConfigurator.Repository.Objects.Colours.ExteriorColour;
using ExteriorColourInfo = TME.CarConfigurator.Repository.Objects.Colours.ExteriorColourInfo;
using UpholsteryInfo = TME.CarConfigurator.Repository.Objects.Colours.UpholsteryInfo;

namespace TME.CarConfigurator.Publisher.Interfaces
{
    public interface IColourMapper
    {
        ExteriorColour MapExteriorColour(ModelGeneration modelGeneration, ModelGenerationExteriorColour colour, bool isPreview, ExteriorColourTypes exteriorColourTypes, string assetUrl);
        ExteriorColour MapExteriorColour(ModelGeneration modelGeneration, Administration.ExteriorColour crossModelColour, bool isPreview, ExteriorColourTypes exteriorColourTypes, string assetUrl);
        ColourCombination MapColourCombination(ModelGeneration modelGeneration, ModelGenerationColourCombination colourCombination, bool isPreview, ExteriorColourTypes exteriorColourTypes, string assetUrl);
        ExteriorColourInfo MapExteriorColourInfo(Administration.ExteriorColourInfo colour);
        ExteriorColourInfo MapExteriorColourApplicability(ExteriorColourApplicability applicability);
        UpholsteryInfo MapUpholsteryApplicability(UpholsteryApplicability applicability);
        UpholsteryInfo MapUpholsteryInfo(LinkedUpholstery upholstery);
    }
}
