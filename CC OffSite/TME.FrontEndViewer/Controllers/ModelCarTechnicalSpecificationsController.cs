﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Packs;
using TME.CarConfigurator.Interfaces.TechnicalSpecifications;
using TME.CarConfigurator.Repository.Objects;
using TME.FrontEndViewer.Models;
using TMME.CarConfigurator;

namespace TME.FrontEndViewer.Controllers
{
    public class ModelCarTechnicalSpecificationsController : Controller
    {
        public ActionResult Index(Guid modelID, Guid carID)
        {
            var context = (Context)Session["context"];
            var oldContext = MyContext.NewContext(context.Brand, context.Country, context.Language);

            ViewBag.ModelID = modelID;
            ViewBag.CarID = carID;

            var model = new CompareView<IReadOnlyList<ICarTechnicalSpecification>>
            {
                OldReaderModel = GetOldReaderModelWithMetrics(oldContext, modelID, carID),
                NewReaderModel = GetNewReaderModelWithMetrics(context, modelID, carID)
            };

            return View(model);
        }
        private static ModelWithMetrics<IReadOnlyList<ICarTechnicalSpecification>> GetOldReaderModelWithMetrics(MyContext oldContext, Guid modelID, Guid carID)
        {
            var start = DateTime.Now;
            var model = new CarConfigurator.LegacyAdapter.Model(TMME.CarConfigurator.Model.GetModel(oldContext, modelID));
            var list = GetList(model, carID);

            return new ModelWithMetrics<IReadOnlyList<ICarTechnicalSpecification>>
            {
                Model = list,
                TimeToLoad = DateTime.Now.Subtract(start)
            };
        }
        private static ModelWithMetrics<IReadOnlyList<ICarTechnicalSpecification>> GetNewReaderModelWithMetrics(Context context, Guid modelID, Guid carID)
        {
            var start = DateTime.Now;
            var model = CarConfigurator.DI.Models.GetModels(context).First(x => x.ID == modelID);
            var list = GetList(model, carID);

            return new ModelWithMetrics<IReadOnlyList<ICarTechnicalSpecification>>
            {
                Model = list,
                TimeToLoad = DateTime.Now.Subtract(start)
            };
        }

        private static IReadOnlyList<ICarTechnicalSpecification> GetList(IModel model, Guid carID)
        {
            return model.Cars.First(x => x.ID == carID).TechnicalSpecifications;
        }
    }
}