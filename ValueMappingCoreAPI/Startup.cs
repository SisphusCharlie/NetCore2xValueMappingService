using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ValueMappingCoreAPI.Data;
using ValueMappingCoreAPI.Models;
using ValueMappingCoreAPI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ValueMappingCoreAPI.IOC;
using ValueMappingCoreAPI.Services.MailHelper;
using ValueMappingCoreAPI.Repository;
using Microsoft.Extensions.Options;
using ValueMappingCoreAPI.Areas.APIArea.Models;
using static ValueMappingCoreAPI.Repository.ValueMapsRepository;
using Microsoft.Extensions.Logging;
using ValueMappingCoreAPI.Services.RedisHelper;
using StackExchange.Redis;
using ValueMappingCoreAPI.Options;
using RawRabbit.vNext;
using RawRabbit;
using ValueMappingCoreAPI.MiddleWare;

namespace ValueMappingCoreAPI
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        //public static T GetAppSettings<T>(string key) where T : class, new()
        //{
        //    var appconfig = new ServiceCollection()
        //        .AddOptions()
        //        .Configure<T>(Configuration.GetSection(key))
        //        .BuildServiceProvider()
        //        .GetService<IOptions<T>>()
        //        .Value;
        //    return appconfig;
        //}
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddDistributedRedisCache();
            //services.AddDistributedSqlServerCache();
            //services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            string redisConnectiong = Configuration.GetConnectionString("Redis");

            services.AddSession(
                options => {
                    options.IdleTimeout = TimeSpan.FromSeconds(15);
                }
                );
            //redis

            services.AddDistributedRedisCache(
                option => {
                    option.Configuration = redisConnectiong;
                }
                );

            services.AddScoped<IRedisServiceHelper, RedisServiceHelper>();


            ////目录浏览，默认禁用
            //services.AddDirectoryBrowser();

            var connection = "Server=DELL;Database=Trade;UID=sa;PWD=";
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<ValueMapContext>();
            //services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connection));
            //services.AddDbContext<TradeContext>();
            //会员系统

            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection))
            //    .AddDbContext<TradeDbContext>(options => options.UseSqlServer(connection));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<TradeDbContext>(options => options.UseSqlServer(connection));
            //services.AddDbContext<TradeDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            //services.AddTransient<ISubscriberService, ValueMapsRepository>();
            //services.AddScoped<ISystemRepository, SystemRepository>();
            //services.AddScoped<IValueMapsRepository, ValueMapsRepository>();
            //services.AddCap(x =>
            //{
            //    // 如果你的 SqlServer 使用的 EF 进行数据操作，你需要添加如下配置：
            //    // 注意: 你不需要再次配置 x.UseSqlServer(""")
            //    x.UseEntityFramework<TradeDbContext>();

            //        // 如果你使用的Dapper，你需要添加如下配置：
            //        x.UseSqlServer("数据库连接字符串");

            //    // 如果你使用的 RabbitMQ 作为MQ，你需要添加如下配置：
            //    x.UseRabbitMQ("localhost");

            //    ////如果你使用的 Kafka 作为MQ，你需要添加如下配置：
            //    //x.UseKafka("localhost：9092");
            //});


            //services.AddScoped<IViewRenderService, ViewRenderService>();
            //services.AddMvc();

            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 6;

                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.OutputFormatters.Add(new ProtobufFormatter());


                options.CacheProfiles.Add(
                    "Never",
                    new Microsoft.AspNetCore.Mvc.CacheProfile()
                    {
                        Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.None,
                        NoStore = true
                    });

                options.CacheProfiles.Add(
                    "Normal",
                    new Microsoft.AspNetCore.Mvc.CacheProfile()
                    {
                        Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client,
                        Duration = 90

                    });
            }
            );


            //SWAGGER
            //services.AddSwaggerGen();

            //services.ConfigureSwaggerGen(options =>
            //{
            //    options.SingleApiVersion(new Info
            //        {
            //                Version = "V1",
            //                Title="Value Map",
            //                Description="Get the specific value map"
            //         });

            //        options.IgnoreObsoleteActions();
            //        options.IgnoreObsoleteProperties();
            //        options.DescribeAllEnumsAsStrings();
            // });
            //redis server
            var connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1");
            var database = connectionMultiplexer.GetDatabase(0);
            services.AddScoped<IDatabase>(_ => database);

            services.Configure<IdentityOptions>(
            options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            services.AddOptions();
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("rabbitmq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);
            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            this.ApplicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<AntiForgery>();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            //app.UseCap();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());

        }
    }
}
