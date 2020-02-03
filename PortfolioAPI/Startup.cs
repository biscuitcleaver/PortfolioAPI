using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using PortfolioAPI.Logic.Configuration;
using PortfolioAPI.Logic.DB;
using PortfolioAPI.Logic.DI.Interfaces;
using PortfolioAPI.Logic.Extensions;
using PortfolioAPI.Logic.Puzzles;
using PortfolioAPI.Models.Settings;

namespace PortfolioAPI
{
    public class Startup
    {

        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        private IKernel Kernel { get; set; }
        private object Resolve(Type type) => Kernel.Get(type);
        private object RequestScope(IContext context) => scopeProvider.Value;
        private sealed class Scope : DisposableObject { }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
           
            Configuration = configuration;
        }

     

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /** MVC setup **/
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
          
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());
            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);

            /*CORS*/
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://enigmaexample.com")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            this.Kernel = this.RegisterApplicationComponents(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IKernel RegisterApplicationComponents(IApplicationBuilder app)
        {
            // IKernelConfiguration config = new KernelConfiguration();
            var kernel = new StandardKernel();

            // Register application services
            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
            }


            var appConfiguration = Configuration.GetSection("AppConfiguration").Get<AppSettings>();

            kernel.Bind(typeof(IOptions<AppSettings>)).To(typeof(AppConfigOption)).WithConstructorArgument(appConfiguration);
            // Cross-wire required framework services
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            // Dependency Injection binding - In a large project, this should really be it's own class. 
            kernel.Bind<IPuzzle>().To<EnigmaPuzzle>().InScope(RequestScope);
            kernel.Bind<INoSql>().To<DynamoLogic>().InScope(RequestScope);
          
           
            return kernel;
        }
    }
}
