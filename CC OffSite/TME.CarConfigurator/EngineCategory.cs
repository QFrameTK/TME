using System;
using System.Collections.Generic;
using TME.CarConfigurator.Interfaces;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Core;

namespace TME.CarConfigurator
{
    public class EngineCategory : IEngineCategory
    {
        public Guid ID { get; private set; }
        public string InternalCode { get; private set; }
        public string LocalCode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string FootNote { get; private set; }
        public string ToolTip { get; private set; }
        public int SortIndex { get; private set; }
        public IEnumerable<ILabel> Labels { get; private set; }
        public IEnumerable<IAsset> Assets { get; private set; }
    }
}