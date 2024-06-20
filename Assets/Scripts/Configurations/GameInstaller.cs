using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // context
        Container.Bind<IDbContext>().To<DbContext>().AsSingle();

        // repositories
        Container.Bind<IPlayerRepository>().To<PlayerRepository>().AsSingle();
        Container.Bind<IPlayerTowerRepository>().To<PlayerTowerRepository>().AsSingle();
        Container.Bind<ITowerRepository>().To<TowerRepository>().AsSingle();
        Container.Bind<IWorldRepository>().To<WorldRepository>().AsSingle();
        Container.Bind<IEnemyRepository>().To<EnemyRepository>().AsSingle();
        Container.Bind<ITowerCharacteristicRepository>().To<TowerCharacteristicRepository>().AsSingle();
        Container.Bind<IStageRepository>().To<StageRepository>().AsSingle();
        Container.Bind<IWaveRepository>().To<WaveRepository>().AsSingle();

        // services
        Container.Bind<IPlayerDataService>().To<PlayerDataService>().AsSingle();
        Container.Bind<ITowerDataService>().To<TowerDataService>().AsSingle();
        Container.Bind<IPlayerTowerDataService>().To<PlayerTowerDataService>().AsSingle();
        Container.Bind<IEnemyDataService>().To<EnemyDataService>().AsSingle();
        Container.Bind<IWaveDataService>().To<WaveDataService>().AsSingle();
        Container.Bind<IWorldDataService>().To<WorldDataService>().AsSingle();
    }
}