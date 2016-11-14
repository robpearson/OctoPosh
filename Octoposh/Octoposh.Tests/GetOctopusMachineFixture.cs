using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Octoposh.Cmdlets;
using Octopus.Client;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace Octoposh.Tests
{
    [TestFixture]
    public class GetOctopusMachineFixture
    {
        [Test]
        public void TestWithFakeData()
        {
            // Given
            var octopusRepository = Substitute.For<IOctopusRepository>();
            var machineRepo = Substitute.For<IMachineRepository>();
            var machineResources = new List<MachineResource>
                {
                    new MachineResource { Name = "Test Machine" }
                };
            machineRepo.FindAll().Returns(machineResources);
            octopusRepository.Machines.Returns(machineRepo);

            // When
            var cmdlet = new GetOctopusMachine(octopusRepository);

            // Execute the cmdlet
            // SOmething like https://weblogs.asp.net/cazzu/PowerShellUnitTestCmdlet

            // Then
            // Assert the result of the cmdlet execution
        }
    }
}
