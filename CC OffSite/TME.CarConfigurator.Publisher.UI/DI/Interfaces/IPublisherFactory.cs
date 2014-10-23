﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.CommandServices;
using TME.CarConfigurator.Publisher.Common.Enums;
using TME.CarConfigurator.Publisher.Interfaces;

namespace TME.CarConfigurator.Publisher.UI.DI.Interfaces
{
    public interface IPublisherFactory
    {
        IModelPublisher GetModelPublisher(IModelService service);
        IPublicationPublisher GetPublicationPublisher(IPublicationService service);
        IBodyTypePublisher GetBodyTypePublisher(IBodyTypeService service);
        IEnginePublisher GetEnginePublisher(IEngineService service);
        ITransmissionPublisher GetTransmissionPublisher(ITransmissionService service);
        IWheelDrivePublisher GetWheelDrivePublisher(IWheelDriveService service);
        ISteeringPublisher GetSteeringPublisher(ISteeringService service);
        IGradePublisher GetGradePublisher(IGradeService service);
        ICarPublisher GetCarPublisher(ICarService service);
        IAssetPublisher GetAssetPublisher(IAssetService service);
        ISubModelPublisher GetSubModelPublisher(ISubModelService service);
        IGradeAccessoryPublisher GetGradeAccessoryPublisher(IGradeAccessoryService service);
        IGradeOptionPublisher GetGradeOptionPublisher(IGradeOptionService service);
    }
}
