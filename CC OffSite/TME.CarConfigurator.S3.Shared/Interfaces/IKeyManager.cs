﻿using System;

namespace TME.CarConfigurator.S3.Shared.Interfaces
{
    public interface IKeyManager
    {
        String GetLanguagesKey();
        String GetPublicationKey(Guid publicationID);
        String GetGenerationBodyTypesKey(Guid publicationID, Guid timeFrameID);
        String GetGenerationEnginesKey(Guid publicationID, Guid timeFrameID);
        String GetAssetsKey(Guid publicationId, Guid timeFrameId, Guid objectId);
    }
}
