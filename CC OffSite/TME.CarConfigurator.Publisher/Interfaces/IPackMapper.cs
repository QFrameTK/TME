using TME.CarConfigurator.Administration;
using TME.CarConfigurator.Repository.Objects.Packs;

namespace TME.CarConfigurator.Publisher.Interfaces
{
    public interface IPackMapper
    {
        GradePack MapGradePack(ModelGenerationGradePack gradePack, ModelGenerationPack generationPack);
    }
}