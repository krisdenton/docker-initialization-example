using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace DockerDotnetTest
{
    public class DockerDotNet
    {
        public void CreateContainer(ContainerType containerType, ConnectionInformation connectionInfo)
        {
            var dockerClient = new DockerClientConfiguration()
                .CreateClient();
            
            var parameters = new CreateContainerParameters();
            switch (containerType)
            {
                case ContainerType.SqlServer:
                    parameters = new CreateContainerParameters
                    {
                        Name = connectionInfo.DatabaseName,
                        Image = "mcr.microsoft.com/mssql/server:2017-latest",
                        Env = new List<string>
                        {
                            "ACCEPT_EULA=Y",
                            $"SA_PASSWORD={connectionInfo.Password}",
                        },
                        ExposedPorts = new Dictionary<string, EmptyStruct>
                        {
                            { connectionInfo.ContainerPort.ToString(), new EmptyStruct() }
                        },
                        HostConfig = new HostConfig
                        {
                            PortBindings = new Dictionary<string, IList<PortBinding>> {
                                {
                                    connectionInfo.ContainerPort.ToString(), new List<PortBinding> {
                                        new PortBinding { HostPort = connectionInfo.HostPort.ToString() }
                                    }
                                }
                            }
                        }
                    };
                    break;
                case ContainerType.Postgres:
                    parameters = new CreateContainerParameters
                    {
                        Name = connectionInfo.DatabaseName,
                        Image = "postgres",
                        Env = new List<string>
                        {
                            $"POSTGRES_DB={connectionInfo.DatabaseName}",
                            $"POSTGRES_USER={connectionInfo.UserName}",
                            $"POSTGRES_PASSWORD={connectionInfo.Password}",
                        },
                        ExposedPorts = new Dictionary<string, EmptyStruct>
                        {
                            { connectionInfo.ContainerPort.ToString(), new EmptyStruct() }
                        },
                        HostConfig = new HostConfig
                        {
                            PortBindings = new Dictionary<string, IList<PortBinding>> {
                                {
                                    connectionInfo.ContainerPort.ToString(), new List<PortBinding> {
                                        new PortBinding { HostPort = connectionInfo.HostPort.ToString() }
                                    }
                                }
                            }
                        }
                    };
                    break;
            }
            
            var containerStartRequest = new ContainerStartParameters();
            var createContainerResponse = dockerClient.Containers.CreateContainerAsync(parameters).Result;
            var createdContainerId = createContainerResponse.ID;
            dockerClient.Containers.StartContainerAsync(createdContainerId, containerStartRequest).Wait();
        }
    }
}