using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DockerDotnetTest
{
    public enum ContainerType
    {
        SqlServer,
        Postgres
    }

    public class CommandPrompt
    {
        private readonly bool _isWindows;
        
        public CommandPrompt()
        {
            _isWindows = RuntimeInformation
                .IsOSPlatform(OSPlatform.Windows);
        }
        
        public void CreateContainer(ContainerType containerType, ConnectionInformation connectionInfo)
        {
            var containerCommand = string.Empty;
                        
            switch (containerType)
            {
                case ContainerType.SqlServer:
                    containerCommand = $"docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD={connectionInfo.Password}' -d " +
                                       $"-p {connectionInfo.ContainerPort.ToString()}:{connectionInfo.HostPort.ToString()} --name {connectionInfo.DatabaseName} " +
                                       "mcr.microsoft.com/mssql/server:2017-latest";
                    if (_isWindows) containerCommand = containerCommand.Replace("'","\"");
                    break;
                case ContainerType.Postgres:
                    containerCommand = $"docker run --name {connectionInfo.DatabaseName} " +
                                       $"-e POSTGRES_DB={connectionInfo.DatabaseName} " +
                                       $"-e POSTGRES_USER={connectionInfo.UserName} " +
                                       $"-e POSTGRES_PASSWORD={connectionInfo.Password} -d " +
                                       $"-p {connectionInfo.ContainerPort.ToString()}:{connectionInfo.HostPort.ToString()} " +
                                       "postgres";
                    break;
            }
                
            RunCommandPromptCommand(containerCommand);
        }
        
        private void RunCommandPromptCommand(string argument)
        {
            using var process = new Process();
            
            ProcessStartInfo startInfo;
    
            if (_isWindows)
            {
                startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C {argument}"
                };
            }
            else
            {
                startInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{argument}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };   
            }
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}