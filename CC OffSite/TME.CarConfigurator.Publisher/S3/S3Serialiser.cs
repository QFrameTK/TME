﻿using System.Collections.Generic;
using TME.CarConfigurator.Publisher.Interfaces;
using Newtonsoft.Json;
using System;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.Repository.Objects.Assets;

namespace TME.CarConfigurator.Publisher.S3
{
    public class S3Serialiser : IS3Serialiser
    {
        public String Serialise(Publication publication)
        {
            return SerialiseObject(publication);
        }
        
        public String Serialise(Languages languages)
        {
            return SerialiseObject(languages);
        }

        public string Serialise(IEnumerable<BodyType> bodyTypes)
        {
            return SerialiseObject(bodyTypes);
        }

        public string Serialise(IEnumerable<Asset> assets)
        {
            return SerialiseObject(assets);
        }

        private String SerialiseObject(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialise<T>(String value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
