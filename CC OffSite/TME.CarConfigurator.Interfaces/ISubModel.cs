using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Assets;
using TME.CarConfigurator.Interfaces.Core;
using TME.CarConfigurator.Interfaces.Equipment;

namespace TME.CarConfigurator.Interfaces
{
    public interface ISubModel : IBaseObject
    {
        IPrice StartingPrice { get; }
        IEnumerable<IEquipmentItem> Equipment { get; }
        IEnumerable<IGrade> Grades { get; }

        IEnumerable<IAsset> Assets { get; }
        IEnumerable<ILink> Links { get; }
    }
}