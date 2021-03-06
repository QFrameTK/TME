﻿using TME.CarConfigurator.QueryServices;

namespace TME.CarConfigurator.DI.Interfaces
{
    public interface IServiceFacade
    {
        IServiceFacade WithModelService(IModelService modelService);
        IServiceFacade WithPublicationService(IPublicationService publicationService);
        IServiceFacade WithBodyTypeService(IBodyTypeService bodyTypeService);
        IServiceFacade WithEngineService(IEngineService engineService);
        IServiceFacade WithTransmissionService(ITransmissionService transmissionService);
        IServiceFacade WithWheelDriveService(IWheelDriveService wheelDriveService);
        IServiceFacade WithSteeringService(ISteeringService steeringService);
        IServiceFacade WithGradeService(IGradeService gradeService);
        IServiceFacade WithCarService(ICarService carService);
        IServiceFacade WithSubModelService(ISubModelService subModelService);
        IServiceFacade WithEquipmentService(IEquipmentService equipmentService);
        IServiceFacade WithSpecificationsService(ISpecificationsService specificationsService);
        IServiceFacade WithColourService(IColourService colourService);
        
        IModelService CreateModelService();
        IPublicationService CreatePublicationService();
        IBodyTypeService CreateBodyTypeService();
        IAssetService CreateAssetService();
        IEngineService CreateEngineService();
        ITransmissionService CreateTransmissionService();
        IWheelDriveService CreateWheelDriveService();
        ISteeringService CreateSteeringService();
        IGradeService CreateGradeService();
        ICarService CreateCarService();
        ISubModelService CreateSubModelService();
        IEquipmentService CreateEquipmentService();
        ISpecificationsService CreateSpecificationsService();
        IColourService CreateColourService();
        IPackService CreatePackService();
        ICarPartService CreateCarPartService();
        IRuleService CreateRuleService();
    }
}