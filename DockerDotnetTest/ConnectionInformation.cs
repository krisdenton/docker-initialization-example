namespace DockerDotnetTest
{
    public class ConnectionInformation
    {
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ContainerPort { get; set; }
        public int HostPort { get; set; }
    }
}