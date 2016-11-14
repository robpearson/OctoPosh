using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Octoposh.Model;
using Octopus.Client;
using Octopus.Client.Model;

namespace Octoposh.Cmdlets
{
    [Cmdlet("Get", "OctopusMachine", DefaultParameterSetName = All)]
    public class GetOctopusMachine : PSCmdlet
    {
        private const string ByName = "ByName";
        private const string ByEnvironment = "ByEnvironment";
        private const string All = "all";

        [Alias("Name")]
        [Parameter(Position = 1, ValueFromPipeline = true, ParameterSetName = ByName)]
        public List<string> MachineName { get; set; }

        [Alias("Environment")]
        [Parameter(ValueFromPipeline = true, ParameterSetName = ByEnvironment)]
        public List<string> EnvironmentName { get; set; }

        private OctopusConnection _connection;
        private OctopusRepository _connectionRepository;

        public GetOctopusMachine()
        {
        }

        public GetOctopusMachine(IOctopusRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        protected override void BeginProcessing()
        {
            _connection = new NewOctopusConnection().Invoke<OctopusConnection>().ToList()[0];
        }

        protected override void ProcessRecord()
        {
            var list = new List<MachineResource>();

            var connectionRepository = GetRepository();

            if (this.ParameterSetName == All)
            {
                list = connectionRepository.Machines.FindAll();
            }
            else if (this.ParameterSetName == ByName)
            {
               //list = _connection.Repository.Machines.FindByNames(MachineName);
               foreach (string Name in MachineName)
               {
                   list.AddRange(connectionRepository.Machines.FindMany(x => x.Name.Contains(Name)));
               }
            }
            else if (this.ParameterSetName == ByEnvironment)
            {
                foreach (string name in EnvironmentName)
                {
                    var environment = connectionRepository.Environments.FindByName(name);
                    list.AddRange(connectionRepository.Environments.GetMachines(environment));
                }
            }

            WriteObject(list);
            
        }

        private OctopusRepository GetRepository()
        {
            OctopusRepository repo;

            if (_connectionRepository != null)
            {
                repo = _connectionRepository;
            }
            else
            {
                repo = _connection.Repository;
            }
            return repo;
        }
    }
}
