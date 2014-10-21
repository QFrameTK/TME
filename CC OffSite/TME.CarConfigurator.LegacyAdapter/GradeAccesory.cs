﻿using TME.CarConfigurator.Interfaces.Equipment;
using Legacy = TMME.CarConfigurator;

namespace TME.CarConfigurator.LegacyAdapter
{
    public class GradeAccesory : GradeEquipmentItem, IGradeAccessory
    {
        
        #region Dependencies (Adaptee)
        private Legacy.EquipmentCompareAccessory Adaptee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public GradeAccesory(Legacy.EquipmentCompareAccessory adaptee)
            : base(adaptee)
        {
            Adaptee = adaptee;
        }
        #endregion

    }
}