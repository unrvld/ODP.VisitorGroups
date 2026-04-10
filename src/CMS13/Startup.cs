using alloy13preview.Extensions;

using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Cms.UI.VisitorGroups;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Scheduler;

using EPiServer.Web.Routing;

using Optimizely.Cms.Service.V1.Authentication;
using Optimizely.Graph.DependencyInjection;
using UNRVLD.ODP.VisitorGroups.Initilization;

namespace alloy13preview;

public class Startup
{
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAlloy()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>()
            .AddVisitorGroupsMvc()
            .AddVisitorGroupsUI();

        services.AddOptions<CmsServiceOauthOptions>()
                .Configure(o => {
                        o.AddDevelopmentSigningCredentials();
                        o.Clients.Add(new OauthClient
                        {
                            ClientId = "client_id",
                            ClientSecret = "client_secret"
                        });
                    }   
                );
        
        //DisplayTemplate
        services.AddContentGraph();
        services.AddContentManager();

        services.AddODPVisitorGroups();
        
        // Required by Wangkanai.Detection
        services.AddDetection();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.Configure<DataAccessOptions>(options =>
        {
            options.UpdateDatabaseCompatibilityLevel = true;
        });


    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Required by Wangkanai.Detection
        app.UseDetection();
        app.UseSession();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
