namespace FAP_API.DatabaseClasses
{
    public class DbManager
    {
        protected readonly string connectionString;

        public DbManager(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
