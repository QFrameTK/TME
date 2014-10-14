﻿using System;

namespace TME.CarConfigurator.S3.Shared.Interfaces
{
    public interface IKeyManager
    {
        String GetLanguagesKey();
        String GetPublicationKey(Guid publicationID);
        String GetBodyTypesKey(Guid publicationId, Guid timeFrameId);
        String GetEnginesKey(Guid publicationID, Guid timeFrameId);
        String GetDefaultAssetsKey(Guid publicationId, Guid objectId);
        String GetAssetsKey(Guid publicationId, Guid objectId, string view, string mode);
    }
}
