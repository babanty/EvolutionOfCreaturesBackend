namespace Infrastructure.HostExtensions.ServiceCollectionExtensions.DatabaseExtension
{
    public class DbOptions
    {
        public string ConnectionString { get; set; }

        /// <summary> 
        /// Whether to update the database automatically when the application starts. 
        /// It rolls only new migrations, and if the database does not need to be updated, then it does nothing.
        /// </summary>
        public bool AutoDatabaseUpdate { get; set; } = true;
    }
}
