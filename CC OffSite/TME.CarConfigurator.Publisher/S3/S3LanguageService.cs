﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.S3.Shared.Result;
using TME.CarConfigurator.Publisher.Interfaces;
using TME.CarConfigurator.S3.Shared.Exceptions;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared;
using TME.CarConfigurator.S3.Shared.Interfaces;

namespace TME.CarConfigurator.Publisher.S3
{
    public class S3LanguageService : IS3LanguageService
    {
        readonly IService _service;
        readonly ISerialiser _serialiser;
        readonly String _modelsOverviewPath = "models-per-language";

        public S3LanguageService(IService service, ISerialiser serialiser)
        {
            _service = service ?? new Service(null);
            _serialiser = serialiser ?? new Serialiser();
        }

        public Languages GetModelsOverviewPerLanguage(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                var value = _service.GetObject(context.Brand, context.Country, _modelsOverviewPath);
                return _serialiser.Deserialise<Languages>(value);
            }
            catch (ObjectNotFoundException)
            {
                return new Languages();
            }
        }

        public async Task<Result> PutModelsOverviewPerLanguage(IContext context, Languages languages)
        {
            if (context == null) throw new ArgumentNullException("context");

            return await _service.PutObjectAsync(context.Brand, context.Country, _modelsOverviewPath, _serialiser.Serialise(languages));
        }
    }
}