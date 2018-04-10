using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using ServiceFabric.Demo.EStore.Common;
using ServiceFabric.Demo.EStore.OrderService.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ServiceFabric.Demo.EStore.ProductService.Model;
using Microsoft.ServiceFabric.Services.Client;
using System;
using Newtonsoft.Json.Serialization;
using ServiceFabric.Demo.EStore.WebAPI.Services;

namespace ServiceFabric.Demo.EStore.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            var builder = new ContainerBuilder();

            builder.UseAutoMapper(Assembly.GetExecutingAssembly());

            builder.RegisterType<ServiceProxyFactory>().As<IServiceProxyFactory>();

            builder.Register<IOrderService>(c =>
            {
                var factory = c.Resolve<IServiceProxyFactory>();
                var uriBuilder = new ServiceUriBuilder(ServiceNames.OrderServiceName);
                return factory.CreateServiceProxy<IOrderService>(uriBuilder.ToUri());
            });

            builder.Register<IProductService>(c =>
            {
                var factory = c.Resolve<IServiceProxyFactory>();
                var uriBuilder = new ServiceUriBuilder(ServiceNames.ProductServiceName);
                return factory.CreateServiceProxy<IProductService>(uriBuilder.ToUri(), new ServicePartitionKey(0));
            });

            // We can easily swtitch to internal implementation
            // builder.RegisterType<DummyProductService>().As<IProductService>();

            builder.Populate(services);

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
