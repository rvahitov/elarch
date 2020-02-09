using JetBrains.Annotations;

namespace ElArch.Storage
{
    public interface IElArchStorageContextFactory
    {
        [NotNull] ElArchStorageContext Create();
    }
}