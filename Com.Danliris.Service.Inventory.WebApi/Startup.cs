using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AccessTokenValidation;
using Newtonsoft.Json.Serialization;
using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Com.Danliris.Service.Inventory.Lib.Services.MaterialDistributionNoteService;
using Com.Danliris.Service.Inventory.Lib.Services.StockTransferNoteService;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Services.MaterialsRequestNoteServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Com.Danliris.Service.Inventory.Lib.Services.FPReturnInvToPurchasingService;
using Com.Danliris.Service.Inventory.Lib.Facades;
using AutoMapper;
using Com.Danliris.Service.Inventory.Lib.Facades.InventoryFacades;
using Com.DanLiris.Service.Inventory.External.MicroService.Lib;
using Com.Danliris.Service.Inventory.Mongo.Lib;
using MongoDB.Driver;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryDocument;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryMovement;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventorySummary;
using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventorySummaries;
using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryMovements;
using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryDocuments;
using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationIntegrationServices;
using FluentScheduler;
using Com.Danliris.Service.Inventory.WebApi.Helpers.Scheduler;

namespace Com.Danliris.Service.Inventory.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void RegisterEndpoint()
        {
            APIEndpoint.Core = Configuration.GetValue<string>("CoreEndpoint") ?? Configuration["CoreEndpoint"];
            APIEndpoint.Inventory = Configuration.GetValue<string>("InventoryEndpoint") ?? Configuration["InventoryEndpoint"];
            APIEndpoint.Production = Configuration.GetValue<string>("ProductionEndpoint") ?? Configuration["ProductionEndpoint"];
            APIEndpoint.Purchasing = Configuration.GetValue<string>("PurchasingEndpoint") ?? Configuration["PurchasingEndpoint"];
        }


        public void RegisterMasterDataSettings()
        {
            MasterDataSettings.Endpoint = Configuration.GetSection("DanLirisSettings").GetValue<string>("MasterDataEndpoint");
            MasterDataSettings.TokenEndpoint = Configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
            MasterDataSettings.Username = Configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
            MasterDataSettings.Password = Configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
        }

        public void RegisterFacades(IServiceCollection services)
        {
            services
                .AddTransient<FPReturnInvToPurchasingFacade>()
                .AddTransient<FpRegradingResultDocsReportFacade>()
                .AddTransient<InventoryDocumentFacade>()
                .AddTransient<InventoryMovementFacade>()
                .AddTransient<InventorySummaryFacade>();
        }

        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddScoped<MaterialsRequestNoteService>()
                .AddScoped<MaterialsRequestNote_ItemService>()
                .AddScoped<MaterialDistributionNoteService>()
                .AddTransient<MaterialDistributionNoteItemService>()
                .AddTransient<StockTransferNoteService>()
                .AddTransient<StockTransferNote_ItemService>()
                .AddTransient<MaterialDistributionNoteDetailService>()
                .AddTransient<FpRegradingResultDetailsDocsService>()
                .AddTransient<FpRegradingResultDocsService>()
                .AddTransient<FPReturnInvToPurchasingService>()
                .AddTransient<FPReturnInvToPurchasingDetailService>()
                .AddScoped<IdentityService>()
                .AddScoped<HttpClientService>()
                .AddScoped<ValidateService>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];
            

            services
                .AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString))
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });


            this.RegisterServices(services);
            this.RegisterFacades(services);
            this.RegisterEndpoint();
            RegisterMasterDataSettings();

            services.Configure<MongoDbSettings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("MongoConnection") ?? Configuration["MongoConnection"];
                    options.Database = Configuration.GetConnectionString("MongoDatabase") ?? Configuration["MongoDatabase"];
                });

            services.AddSingleton<IMongoClient, MongoClient>(
               _ => new MongoClient(Configuration.GetConnectionString("MongoConnection") ?? Configuration["MongoConnection"]));

            services.AddTransient<IMongoDbContext, MongoDbMigrationContext>();
            services.AddTransient<IInventoryDocumentMongoRepository, InventoryDocumentMongoRepository>();
            services.AddTransient<IInventoryMovementMongoRepository, InventoryMovementMongoRepository>();
            services.AddTransient<IInventorySummaryMongoRepository, InventorySummaryMongoRepository>();

            services.AddTransient<IInventoryDocumentMigrationService, InventoryDocumentMigrationService>();
            services.AddTransient<IInventoryMovementMigrationService, InventoryMovementMigrationService>();
            services.AddTransient<IInventorySummaryMigrationService, InventorySummaryMigrationService>();

            services.AddTransient<IInventoryDocumentIntegrationMigrationService, InventoryDocumentIntegrationMigrationService>();
            services.AddTransient<IInventoryMovementIntegrationMigrationService, InventoryMovementIntegrationMigrationService>();
            services.AddTransient<IInventorySummaryIntegrationMigrationService, InventorySummaryIntegrationMigrationService>();

            var Secret = Configuration.GetValue<string>("Secret") ?? Configuration["Secret"];
            var Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddJsonFormatters();

            services.AddAutoMapper();
            services.AddCors(options => options.AddPolicy("InventoryPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetService<InventoryDbContext>();
            //    context.Database.Migrate();
            //}

            JobManager.Initialize(new SetMasterRegistry(app.ApplicationServices));
            app.UseAuthentication();
            app.UseCors("InventoryPolicy");
            app.UseMvc();
        }
    }
}
