﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TME.CarConfigurator.Core;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Core;

namespace TME.CarConfigurator
{
    public class Grade : BaseObject, IGrade
    {
        readonly Repository.Objects.Grade _repositoryGrade;

        Price _price;

        public Grade(Repository.Objects.Grade repositoryGrade)
            : base(repositoryGrade)
        {
            if (repositoryGrade == null) throw new ArgumentNullException("repositoryGrade");

            _repositoryGrade = repositoryGrade;
        }

        public string FeatureText { get { return _repositoryGrade.FeatureText; } }

        public string LongDescription { get { return _repositoryGrade.LongDescription; } }

        public bool Special { get { return _repositoryGrade.Special; } }

        public IGrade BasedUpon { get { throw new NotImplementedException(); } }

        public IPrice StartingPrice { get { return _price = _price ?? new Price(_repositoryGrade.StartingPrice); } }

        public IEnumerable<IAsset> Assets { get { throw new NotImplementedException(); } }
    }
}
