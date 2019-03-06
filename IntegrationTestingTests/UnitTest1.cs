using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using IntegrationTestingWeb;
using IntegrationTestingWeb.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var builder = new WebHostBuilder().ConfigureServices(services =>
            {
                var startupAssembly = typeof(TestStartupInTestProject).GetTypeInfo().Assembly;

                var manager = new ApplicationPartManager();
                manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));
                manager.ApplicationParts.Add(new AssemblyPart(typeof(HomeController).Assembly));
                manager.FeatureProviders.Add(new ControllerFeatureProvider());
                manager.FeatureProviders.Add(new ViewComponentFeatureProvider());
                services.AddSingleton(manager);
            }).UseStartup<TestStartupInTestProject>();
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