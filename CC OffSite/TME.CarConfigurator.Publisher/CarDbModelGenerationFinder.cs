﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Administration;

namespace TME.CarConfigurator.Publisher
{
    public interface ICarDbModelGenerationFinder
    {
        IReadOnlyDictionary<String, ModelGeneration> GetModelGeneration(String brand, String countryCode, Guid generationID);
    }

    public class CarDbModelGenerationFinder : ICarDbModelGenerationFinder
    {
        public IReadOnlyDictionary<String, ModelGeneration> GetModelGeneration(String brand, String countryCode, Guid generationID)
        {
            // Is ensuring a context can be retrieved by setting to the know global context necessary?
            MyContext.SetSystemContext("Toyota", "ZZ", "en");

            var country = MyContext.GetContext().Countries.Single(ctry => ctry.Code == countryCode);
            return country.Languages.ToDictionary(lang => lang.Code, lang =>
            {
                MyContext.SetSystemContext(brand, countryCode, lang.Code);
                return Models.GetModels().SelectMany(model => model.Generations).Single(generation => generation.ID == generationID);
            });
        }
    }
}