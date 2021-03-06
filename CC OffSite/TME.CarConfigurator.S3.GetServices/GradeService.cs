﻿using System;
using System.Collections.Generic;
using TME.CarConfigurator.QueryServices;
using TME.CarConfigurator.Repository.Objects;
using TME.CarConfigurator.S3.Shared.Interfaces;

namespace TME.CarConfigurator.S3.QueryServices
{
    public class GradeService : IGradeService
    {
        private readonly ISerialiser _serializer;
        private readonly IService _service;
        private readonly IKeyManager _keyManager;

        public GradeService(ISerialiser serializer, IService service, IKeyManager keyManager)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (service == null) throw new ArgumentNullException("service");
            if (keyManager == null) throw new ArgumentNullException("keyManager");

            _serializer = serializer;
            _service = service;
            _keyManager = keyManager;
        }

        public IEnumerable<Grade> GetGrades(Guid publicationId, Guid publicationTimeFrameId, Context context)
        {
            var key = _keyManager.GetGradesKey(publicationId, publicationTimeFrameId);
            var serializedObject = _service.GetObject(context.Brand, context.Country, key);
            return _serializer.Deserialise<IEnumerable<Grade>>(serializedObject);
        }

        public IEnumerable<Grade> GetSubModelGrades(Guid publicationID, Guid publicationTimeFrameID, Guid subModelID, Context context)
        {
            var key = _keyManager.GetSubModelGradesKey(publicationID, publicationTimeFrameID, subModelID);
            var serialisedObject = _service.GetObject(context.Brand, context.Country, key);
            return _serializer.Deserialise<IEnumerable<Grade>>(serialisedObject);
        }
    }
}
