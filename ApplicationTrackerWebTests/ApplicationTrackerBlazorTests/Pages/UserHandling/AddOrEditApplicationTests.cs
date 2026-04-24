using ApplicationTracker.ApiClient;
using ApplicationTracker.Web.Components.Pages.ApplicationHandling;
using ApplicationTracker.Web.Components.Shared;
using ApplicationTracker.Web.Services.Authentication;
using ApplicationTrackerBlazorTests.Setups;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ApplicationTrackerBlazorTests.Pages.UserHandling
{
	[TestFixture]
	[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
	class AddOrEditApplicationTests : BunitContext
	{
		[Test]
		public void EditFormLoadsExistingApplication()
		{
			this.SetAuthorized();
			var userId = Guid.NewGuid();
			var applicationGuid = Guid.NewGuid();

			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			apiClientMock.Setup(client => client.ApplicationAsync(It.IsAny<Guid>())).ReturnsAsync(new JobApplicationDisplayDto
			{

				JobApplicationId = applicationGuid,
				Company = "Test Company",
				JobTitle = "Test Position",
				AppliedDate = DateTime.UtcNow,
				ResumePdf = new PdfFile
				{
					FileName = "resume.pdf",
					ContentType = "application/pdf",
					Content = new byte[] { 0x25, 0x50, 0x44, 0x46 }
				},
				ApplicationPdf = new PdfFile
				{
					FileName = "application.pdf",
					ContentType = "application/pdf",
					Content = new byte[] { 0x25, 0x50, 0x44, 0x46 }
				}
			});


			Services.AddSingleton(apiClientMock.Object);

			var authenticationService = new Mock<IAuthenticationService>();
			authenticationService.Setup(service => service.GetUserIdAsync()).ReturnsAsync(userId);
			Services.AddSingleton<IAuthenticationService>(authenticationService.Object);
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/fileInputInteropModule.js");
			var cut = Render<CascadingValue<IAuthenticationService>>(parameters => parameters
			.Add(p => p.Value, authenticationService.Object)
				.AddChildContent<AddOrEditApplication>(child =>
					child.Add(p => p.ApplicationId, applicationGuid)
			));


			Assert.That(cut.Markup.Contains("Test Company"));
			Assert.That(cut.Markup.Contains("Test Position"));
			var companyInput = cut.Find("input[id='company']");
			Assert.That(companyInput.GetAttribute("value")!.Equals("Test Company"));
			apiClientMock.Verify(x => x.ApplicationAsync(applicationGuid), Times.Once);

		}


		[Test]
		public void NewFormLoadsDefaults()
		{
			this.SetAuthorized();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			Services.AddSingleton(apiClientMock.Object);

			var authenticationService = new Mock<IAuthenticationService>();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/fileInputInteropModule.js");

			var cut = Render<AddOrEditApplication>();

			var companyInput = cut.Find("input[id='company']");
			Assert.That(string.IsNullOrEmpty(companyInput.GetAttribute("value")));
			apiClientMock.Verify(x => x.ApplicationAsync(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		public void CreateSubmitCallsNewAsync()
		{
			this.SetAuthorized();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			apiClientMock.Setup(client => client.NewAsync(It.IsAny<NewJobApplicationDto>())).Returns(Task.CompletedTask);
			Services.AddSingleton(apiClientMock.Object);

			var authenticationService = new Mock<IAuthenticationService>();
			authenticationService.Setup(service => service.GetUserIdAsync()).ReturnsAsync(Guid.NewGuid());
			Services.AddSingleton<IAuthenticationService>(authenticationService.Object);
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/fileInputInteropModule.js");

			var cut = Render<AddOrEditApplication>();

			var companyInput = cut.Find("input[id='company']");
			companyInput.Change("My New Company");
			var jobTitleInput = cut.Find("input[id='jobTitle']");
			jobTitleInput.Change("Developer");

			var submit = cut.Find("button[type='submit']");
			submit.Click();

			apiClientMock.Verify(x => x.NewAsync(It.Is<NewJobApplicationDto>(d => d.Company == "My New Company" && d.JobTitle == "Developer")), Times.Once);
		}

		[Test]
		public void EditSubmitCallsUpdateAsync()
		{
			this.SetAuthorized();
			var userId = Guid.NewGuid();
			var applicationGuid = Guid.NewGuid();

			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			apiClientMock.Setup(client => client.ApplicationAsync(It.IsAny<Guid>())).ReturnsAsync(new JobApplicationDisplayDto
			{
				JobApplicationId = applicationGuid,
				Company = "Test Company",
				JobTitle = "Test Position",
				AppliedDate = DateTime.UtcNow,
				ResumePdf = new PdfFile
				{
					FileName = "resume.pdf",
					ContentType = "application/pdf",
					Content = new byte[] { 0x25, 0x50, 0x44, 0x46 }
				},
				ApplicationPdf = new PdfFile
				{
					FileName = "application.pdf",
					ContentType = "application/pdf",
					Content = new byte[] { 0x25, 0x50, 0x44, 0x46 }
				}

			});

			apiClientMock.Setup(client => client.UpdateAsync(It.IsAny<JobApplicationDisplayDto>())).Returns(Task.CompletedTask);
			Services.AddSingleton(apiClientMock.Object);

			var authenticationService = new Mock<IAuthenticationService>();
			authenticationService.Setup(service => service.GetUserIdAsync()).ReturnsAsync(userId);
			Services.AddSingleton<IAuthenticationService>(authenticationService.Object);
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/fileInputInteropModule.js");

			var cut = Render<CascadingValue<IAuthenticationService>>(parameters => parameters
				.Add(p => p.Value, authenticationService.Object)
				.AddChildContent<AddOrEditApplication>(child => child.Add(p => p.ApplicationId, applicationGuid)));

			var submit = cut.Find("button[type='submit']");
			submit.Click();

			apiClientMock.Verify(x => x.UpdateAsync(It.Is<JobApplicationDisplayDto>(d => d.JobApplicationId == applicationGuid)), Times.Once);
		}

		[Test]
		public void ValidationPreventsSubmit_WhenRequiredMissing()
		{
			this.SetAuthorized();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			apiClientMock.Setup(client => client.NewAsync(It.IsAny<NewJobApplicationDto>())).Returns(Task.CompletedTask);
			Services.AddSingleton(apiClientMock.Object);

			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/fileInputInteropModule.js");

			var cut = Render<AddOrEditApplication>();

			
			var submit = cut.Find("button[type='submit']");
			submit.Click();

		
			apiClientMock.Verify(x => x.NewAsync(It.IsAny<NewJobApplicationDto>()), Times.Never);
		}


	}
}
