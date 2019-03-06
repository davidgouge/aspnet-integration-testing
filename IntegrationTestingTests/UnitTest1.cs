using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IntegrationTestingWeb;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [Test]
        public async Task GetPrivacy_Through_Test_Startup_In_Web_Project()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartupInWebProject>();
            var client = new TestServer(builder).CreateClient();

            var result = await client.GetAsync("/Home/Privacy");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Test]
        public async Task GetPrivacy_Through_Test_Startup_In_Test_Project()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartupInTestProject>();
            var client = new TestServer(builder).CreateClient();

            var result = await client.GetAsync("/Home/Privacy");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Test]
        public async Task GetPrivacy_Through_Normal_Startup()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var client = new TestServer(builder).CreateClient();

            var result = await client.GetAsync("/Home/Privacy");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
    
    public class TestStartupInTestProject : Startup
    {
        public TestStartupInTestProject(IConfiguration configuration) : base(configuration)
        {
        }
    }
}