namespace DockerDotnetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionInformation = new ConnectionInformation
            {
                DatabaseName = "MyCoolDb",
                ContainerPort = 1555,
                Password = "MyCoolPassword#123",
                UserName = "MyCoolUserName"
            };
            var containerType = ContainerType.SqlServer;
            
            //TODO: uncomment for sql
            // connectionInformation.HostPort = 1433;
            // containerType = ContainerType.SqlServer;
            
            //TODO: uncomment for postgres
            // connectionInformation.HostPort = 5432;
            // containerType = ContainerType.Postgres;
            
            //TODO: uncomment for commandPrompt
            // var commandPromptService = new CommandPrompt();
            // commandPromptService.CreateContainer(containerType, connectionInformation);

            //TODO: uncomment for Docker DotNet
            // var service = new DockerDotNet();
            // service.CreateContainer(containerType, connectionInformation);
        }
    }
}