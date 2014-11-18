﻿using System;
using System.Collections.Generic;
using System.Linq;
using TME.CarConfigurator.Administration;
using TME.CarConfigurator.Administration.Enums;
using TME.CarConfigurator.Publisher.Exceptions;
using TME.CarConfigurator.Publisher.Extensions;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Equipment;
using TME.CarConfigurator.Repository.Objects.Interfaces;
using TME.CarConfigurator.S3.QueryServices;
using Car = TME.CarConfigurator.Administration.Car;
using EquipmentItem = TME.CarConfigurator.Administration.EquipmentItem;
using ExteriorColour = TME.CarConfigurator.Repository.Objects.Colours.ExteriorColour;
using EquipmentExteriorColour = TME.CarConfigurator.Repository.Objects.Equipment.ExteriorColour;


namespace TME.CarConfigurator.Publisher.Mappers
{
    public class EquipmentMapper : IEquipmentMapper
    {
        readonly ILabelMapper _labelMapper;
        readonly ILinkMapper _linkMapper;
        readonly IVisibilityMapper _visibilityMapper;
        readonly ICategoryMapper _categoryInfoMapper;
        readonly IColourMapper _colourMapper;

        public EquipmentMapper(ILabelMapper labelMapper, ILinkMapper linkMapper, IVisibilityMapper visibilityMapper, ICategoryMapper categoryInfoMapper, IColourMapper colourMapper)
        {
            if (labelMapper == null) throw new ArgumentNullException("labelMapper");
            if (linkMapper == null) throw new ArgumentNullException("linkMapper");
            if (visibilityMapper == null) throw new ArgumentNullException("visibilityMapper");
            if (categoryInfoMapper == null) throw new ArgumentNullException("categoryInfoMapper");
            if (colourMapper == null) throw new ArgumentNullException("colourMapper");

            _labelMapper = labelMapper;
            _linkMapper = linkMapper;
            _visibilityMapper = visibilityMapper;
            _categoryInfoMapper = categoryInfoMapper;
            _colourMapper = colourMapper;
        }

        public GradeAccessory MapGradeAccessory(ModelGenerationGradeAccessory generationGradeAccessory, EquipmentItem crossModelAccessory, EquipmentCategories categories, IReadOnlyList<Car> cars, Boolean isPreview)
        {
            var mappedGradeEquipmentItem = new GradeAccessory();

            var generationAccessory = generationGradeAccessory.Grade.Generation.Equipment[generationGradeAccessory.ID];
            return MapGradeEquipmentItem(mappedGradeEquipmentItem, generationGradeAccessory, generationAccessory, crossModelAccessory, categories, cars, isPreview);
        }

        public GradeOption MapGradeOption(ModelGenerationGradeOption generationGradeOption, EquipmentItem crossModelOption, EquipmentCategories categories, IReadOnlyList<Car> cars, bool isPreview)
        {
            if (generationGradeOption.HasParentOption && !generationGradeOption.ParentOption.ShortID.HasValue)
                throw new CorruptDataException(String.Format("Please supply a ShortID for grade option {0}", generationGradeOption.ParentOption.Name));

            var generationOption = (ModelGenerationOption)generationGradeOption.Grade.Generation.Equipment[generationGradeOption.ID];

            var mappedGradeEquipmentItem = new GradeOption
            {
                TechnologyItem = generationOption.TechnologyItem,
                ParentOptionShortID = generationGradeOption.HasParentOption ? generationGradeOption.ParentOption.ShortID.Value : 0
            };

            return MapGradeEquipmentItem(mappedGradeEquipmentItem, generationGradeOption, generationOption, crossModelOption, categories, cars, isPreview);
        }

        T MapGradeEquipmentItem<T>(T mappedEquipmentItem, ModelGenerationGradeEquipmentItem generationGradeEquipmentItem, ModelGenerationEquipmentItem generationEquipmentItem, EquipmentItem crossModelEquipmentItem, EquipmentCategories categories, IReadOnlyList<Car> cars, Boolean isPreview)
            where T : GradeEquipmentItem, IAvailabilityProperties
        {
            if (!generationGradeEquipmentItem.ShortID.HasValue)
                throw new CorruptDataException(String.Format("Please supply a ShortID for grade equipment item {0}", generationGradeEquipmentItem.Name));

            var hasColour = generationGradeEquipmentItem.Colour.ID != Guid.Empty;
            var isOwner = generationGradeEquipmentItem.Owner == MyContext.GetContext().CountryCode;

            mappedEquipmentItem.BestVisibleIn = new BestVisibleIn { Angle = generationEquipmentItem.BestVisibleInAngle, Mode = generationEquipmentItem.BestVisibleInMode, View = generationEquipmentItem.BestVisibleInView };
            mappedEquipmentItem.Category = _categoryInfoMapper.MapEquipmentCategoryInfo(generationGradeEquipmentItem.Category, categories); // ??
            mappedEquipmentItem.Description = generationGradeEquipmentItem.Translation.Description;
            mappedEquipmentItem.ExteriorColour = hasColour ? GetColour(generationEquipmentItem, generationGradeEquipmentItem, isPreview) : null;
            mappedEquipmentItem.FootNote = generationGradeEquipmentItem.Translation.FootNote;
            mappedEquipmentItem.GradeFeature = generationGradeEquipmentItem.GradeFeature;
            mappedEquipmentItem.ID = generationGradeEquipmentItem.ID;
            mappedEquipmentItem.InternalName = generationEquipmentItem.BaseName;
            mappedEquipmentItem.KeyFeature = generationEquipmentItem.KeyFeature;

            mappedEquipmentItem.Labels = _labelMapper.MapLabels(generationGradeEquipmentItem.Translation.Labels, generationEquipmentItem.Translation.Labels, crossModelEquipmentItem.Translation.Labels);

            mappedEquipmentItem.Links = crossModelEquipmentItem.Links.Select(link => _linkMapper.MapLink(link, isPreview)).ToList();
            mappedEquipmentItem.Name = generationGradeEquipmentItem.Translation.Name.DefaultIfEmpty(generationGradeEquipmentItem.Name);
            mappedEquipmentItem.NotAvailableOn = GetAvailabilityInfo(generationGradeEquipmentItem, Availability.NotAvailable, cars);
            mappedEquipmentItem.OptionalGradeFeature = generationGradeEquipmentItem.OptionalGradeFeature;
            mappedEquipmentItem.OptionalOn = GetAvailabilityInfo(generationGradeEquipmentItem, Availability.Optional, cars);
            mappedEquipmentItem.PartNumber = generationGradeEquipmentItem.PartNumber;
            mappedEquipmentItem.Path = MyContext.GetContext().EquipmentGroups.Find(generationGradeEquipmentItem.Group.ID).Path.ToLowerInvariant();
            mappedEquipmentItem.ShortID = generationGradeEquipmentItem.ShortID.Value;
            mappedEquipmentItem.SortIndex = generationGradeEquipmentItem.Index;
            mappedEquipmentItem.StandardOn = GetAvailabilityInfo(generationGradeEquipmentItem, Availability.Standard, cars);
            mappedEquipmentItem.Visibility = _visibilityMapper.MapVisibility(generationEquipmentItem.Visibility);
            mappedEquipmentItem.ToolTip = generationGradeEquipmentItem.Translation.ToolTip;
            mappedEquipmentItem.InternalCode = generationEquipmentItem.BaseCode;
            mappedEquipmentItem.LocalCode = generationGradeEquipmentItem.LocalCode.DefaultIfEmpty(isOwner ? generationEquipmentItem.BaseCode : String.Empty);

            mappedEquipmentItem.NotAvailable = mappedEquipmentItem.CalculateNotAvailable();
            mappedEquipmentItem.Optional = mappedEquipmentItem.CalculateOptional();
            mappedEquipmentItem.Standard = mappedEquipmentItem.CalculateStandard();

            return mappedEquipmentItem;
        }

        EquipmentExteriorColour GetColour(ModelGenerationEquipmentItem generationEquipmentItem, ModelGenerationGradeEquipmentItem generationGradeEquipmentItem, bool isPreview)
        {
            var mappedExteriorColour = GetMappedExteriorColour(generationEquipmentItem, isPreview);

            return new EquipmentExteriorColour()
            {
                ID = mappedExteriorColour.ID,
                InternalCode = mappedExteriorColour.InternalCode,
                LocalCode = mappedExteriorColour.LocalCode,
                Name = mappedExteriorColour.Name,
                Description = mappedExteriorColour.Description,
                FootNote = mappedExteriorColour.FootNote,
                ToolTip = mappedExteriorColour.ToolTip,
                Labels = mappedExteriorColour.Labels,
                SortIndex = mappedExteriorColour.SortIndex,
                Transformation = mappedExteriorColour.Transformation
            };
        }

        private ExteriorColour GetMappedExteriorColour(ModelGenerationEquipmentItem generationEquipmentItem, bool isPreview)
        {
            var generationExteriorColour = generationEquipmentItem.Generation.ColourCombinations.ExteriorColours()
                    .FirstOrDefault(clr => clr.ID == generationEquipmentItem.Colour.ID);

            if (generationExteriorColour != null)
                return _colourMapper.MapExteriorColour(generationEquipmentItem.Generation, generationExteriorColour, isPreview);

            var crossModelColour = ExteriorColours.GetExteriorColours()[generationEquipmentItem.Colour.ID];
            return _colourMapper.MapExteriorColour(generationEquipmentItem.Generation, crossModelColour, isPreview);
        }

        static IReadOnlyList<CarInfo> GetAvailabilityInfo(ModelGenerationGradeEquipmentItem generationGradeEquipmentItem, Availability availability, IEnumerable<Car> cars)
        {
            return cars.Where(car => car.Equipment[generationGradeEquipmentItem.ID] != null ? car.Equipment[generationGradeEquipmentItem.ID].Availability == availability : availability == Availability.NotAvailable)
                       .Select(car => new CarInfo
                       {
                           Name = car.Translation.Name.DefaultIfEmpty(car.Name),
                           ShortID = car.ShortID.Value
                       })
                       .ToList();
        }

    }
}
