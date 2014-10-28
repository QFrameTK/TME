using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common.Result;
using TME.CarConfigurator.Repository.Objects.Packs;
using TME.CarConfigurator.S3.Shared.Interfaces;

namespace TME.CarConfigurator.S3.CommandServices
{
    public class GradePackService : IGradePackService
    {
        private readonly IService _service;
        private readonly ISerialiser _serialiser;
        private readonly IKeyManager _keyManager;

        public GradePackService(IService service, ISerialiser serialiser, IKeyManager keyManager)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (serialiser == null) throw new ArgumentNullException("serialiser");
            if (keyManager == null) throw new ArgumentNullException("keyManager");

            _service = service;
            _serialiser = serialiser;
            _keyManager = keyManager;
        }

        public async Task<Result> PutAsync(string brand, string country, Guid publicationId, Guid timeFrameId, Guid gradeId, IList<GradePack> packs)
        {
            var key = _keyManager.GetGradePacksKey(publicationId, timeFrameId, gradeId);
            var serializedPacks = _serialiser.Serialise(packs);

            return await _service.PutObjectAsync(brand, country, key, serializedPacks);
        }
    }
}