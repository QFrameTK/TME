using System.Collections.Generic;
using TME.CarConfigurator.Interfaces.Packs;
using TME.CarConfigurator.Repository.Objects;

namespace TME.CarConfigurator.Interfaces.Factories
{
    public interface IPackFactory
    {
        IEnumerable<IGradePack> GetGradePacks(Publication publication, Context context, Grade grade);
    }
}