using System.Reflection;
using ApplicationTracker.ApiClient;
using ApplicationTracker.Web.Components.Shared;
using ApplicationTrackerBlazorTests.Setups;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;

namespace ApplicationTrackerBlazorTests.Shared
{
	[TestFixture]
	[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
	internal class JSModuleLoaderTests : BunitContext
	{

        [Test]
        public async Task ClipboardModule_Is_Loaded()
		{
			this.SetAuthorized();
			Services.RegisterApplicationTrackerApi();
			JSInterop.SetupModule("./js/clipboardModule.js");
            // Use the concrete service instance with the test IJSRuntime
            var jsRuntime = Services.GetRequiredService<IJSRuntime>();
            var loader = new JSModuleLoader(jsRuntime);
            await loader.RegisterAsync(ModuleType.ClipBoardModule);

            // Inspect private _modules dictionary to assert the module was loaded
            var modulesField = typeof(JSModuleLoader).GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = (Dictionary<int, IJSObjectReference>?)modulesField!.GetValue(loader);
            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules!.Count);
            Assert.IsTrue(modules.ContainsKey((int)ModuleType.ClipBoardModule));
		}

        [Test]
        public async Task ModuleLoaderLoadsMultipleModules()
		{
			Services.RegisterApplicationTrackerApi();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());

			JSInterop.SetupModule("./js/browserInterop.js");
			JSInterop.SetupModule("./js/pdfModule.js");

			Services.AddSingleton<ApplicationTrackerApiClient>(apiClientMock.Object);
            var jsRuntime = Services.GetRequiredService<IJSRuntime>();
            var loader = new JSModuleLoader(jsRuntime);
            await loader.RegisterAsync(ModuleType.PdfModule, ModuleType.BrowserInterop);

            var modulesField = typeof(JSModuleLoader).GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = (Dictionary<int, IJSObjectReference>?)modulesField!.GetValue(loader);
            Assert.IsNotNull(modules);
            Assert.AreEqual(2, modules!.Count);
            Assert.IsTrue(modules.ContainsKey((int)ModuleType.PdfModule));
            Assert.IsTrue(modules.ContainsKey((int)ModuleType.BrowserInterop));
		}



	}
}
