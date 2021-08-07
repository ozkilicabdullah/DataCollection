using DataCollection.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataCollection.Consumers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketConsumer>();
                config.AddConsumer<ReturnedConsumer>();
                config.AddConsumer<SearchConsumer>();
                config.AddConsumer<SuccessFullCheckoutConsumer>();
                config.AddConsumer<ViewConsumer>();
                config.AddConsumer<WishConsumer>();

                config.UsingRabbitMq((contex, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");


                    cfg.ReceiveEndpoint("basket_queue", c =>
                    {
                        c.ConfigureConsumer<BasketConsumer>(contex);
                    });
                    cfg.ReceiveEndpoint("returned_queue", c =>
                    {
                        c.ConfigureConsumer<ReturnedConsumer>(contex);
                    });
                    cfg.ReceiveEndpoint("search_queue", c =>
                    {
                        c.ConfigureConsumer<SearchConsumer>(contex);
                    });
                    cfg.ReceiveEndpoint("successfullcheckout_queue", c =>
                    {
                        c.ConfigureConsumer<SuccessFullCheckoutConsumer>(contex);
                    });
                    cfg.ReceiveEndpoint("view_queue", c =>
                    {
                        c.ConfigureConsumer<ViewConsumer>(contex);
                    });
                    cfg.ReceiveEndpoint("wish_queue", c =>
                    {
                        c.ConfigureConsumer<WishConsumer>(contex);
                    });
                });
            });

            services.AddMassTransitHostedService();


            services.AddScoped<IConnectionService, ConnectionService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
