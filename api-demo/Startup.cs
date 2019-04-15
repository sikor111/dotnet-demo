using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;


namespace api_demo
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
             var counter = Metrics.CreateCounter("ExceptionCounter","Counts exceptions",new CounterConfiguration{
                LabelNames = new []{"ExceptionType","IsBuildInException"}
            });
            
            services.AddSingleton<Counter>(sp=>{ return counter;});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
             var counter = Metrics.CreateCounter("PathCounter","Count requests to endpoints",new CounterConfiguration{
                LabelNames = new []{"method","endpoint"}
            });

            app.Use((context,next)=> {
                counter.WithLabels(context.Request.Method,context.Request.Path).Inc();
                return next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMetricServer();
            app.UseHttpMetrics(options =>
            {
                options.RequestCount.Enabled = false;

                options.RequestDuration.Histogram = Metrics.CreateHistogram("myapp_http_request_duration_seconds", "Some help text",
                    new HistogramConfiguration
                    {  
                        Buckets = Histogram.LinearBuckets(start: 1, width: 1, count: 64)
                    });
            });

        }
    }
}