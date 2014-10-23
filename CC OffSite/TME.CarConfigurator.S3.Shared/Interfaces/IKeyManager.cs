﻿using System;

namespace TME.CarConfigurator.S3.Shared.Interfaces
{
    public interface IKeyManager
    {
        String GetModelsKey();
        String GetPublicationKey(Guid publicationID);
        String GetBodyTypesKey(Guid publicationID, Guid timeFrameID);
        String GetEnginesKey(Guid publicationID, Guid timeFrameID);
        String GetTransmissionsKey(Guid publicationID, Guid timeFrameID);
        String GetWheelDrivesKey(Guid publicationID, Guid timeFrameID);
        String GetSteeringsKey(Guid publicationID, Guid timeFrameID);
        String GetGradesKey(Guid publicationID, Guid timeFrameID);
        String GetCarsKey(Guid publicationID, Guid timeFrameID);
        String GetSubModelsKey(Guid publicationID, Guid timeFrameID);
        String GetGradeAccessoriesKey(Guid publicationID, Guid timeFrameID, Guid gradeID);
        String GetGradeOptionsKey(Guid publicationID, Guid timeFrameID, Guid gradeID);
        String GetDefaultAssetsKey(Guid publicationID, Guid objectID);
        String GetDefaultAssetsKey(Guid publicationID, Guid carID, Guid objectID);
        String GetAssetsKey(Guid publicationID, Guid objectID, String view, String mode);
        String GetAssetsKey(Guid publicationID, Guid carID, Guid objectID, String view, String mode);



    }
}
