namespace Infrastructure.HostExtensions.ServiceCollectionExtensions.DatabaseExtension
{
    public class DbOptions
    {
        public string ConnectionString { get; set; }

        /// <summary> 
        /// Автоматически ли накатывать миграции на базу данных при старте приложения. Накатаывает только новые миграции,
        /// а если БД обновлять не надо, то ни чего не делает
        /// </summary>
        public bool AutoDatabaseUpdate { get; set; } = true;
    }
}
