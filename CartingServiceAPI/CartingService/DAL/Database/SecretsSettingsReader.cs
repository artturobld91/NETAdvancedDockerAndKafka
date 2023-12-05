namespace CartingService.DAL.Database
{
    public class SecretsSettingsReader
    {
        public T ReadSection<T>(string sectionName)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();
            var configurationRoot = builder.Build();

            return configurationRoot.GetSection(sectionName).Get<T>();
        }
    }
}
