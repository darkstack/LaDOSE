using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider;
using LaDOSE.Business.Service;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using LaDOSE.Api.Helpers;
using LaDOSE.Business.Helper;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Api
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
            //Fix Gentoo Issue.
   
            var MySqlServer = this.Configuration["MySql:Server"];
            var MySqlDatabase = this.Configuration["MySql:Database"];
            var MySqlUser = this.Configuration["MySql:User"];
            var MySqlPassword = this.Configuration["MySql:Password"];
            if (Convert.ToBoolean(this.Configuration["FixGentoo"]))
            {
                try
                {
                    var loadFrom = Assembly.LoadFrom("ChallongeCSharpDriver.dll");
                    Console.WriteLine($"Fix Gentoo Ok : {loadFrom.FullName}");
                }
                catch(Exception exception)
                {
                    Console.WriteLine($"Fix Gentoo NOK : {exception.Message}");
                }
            }
            
            services.AddCors();
            services.AddMvc().AddJsonOptions(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                x.SerializerSettings.MaxDepth= 4;
            });
            services.AddDbContextPool<LaDOSEDbContext>( // replace "YourDbContext" with the class name of your DbContext
                options => options.UseMySql($"Server={MySqlServer};Database={MySqlDatabase};User={MySqlUser};Password={MySqlPassword};", // replace with your Connection String
                    mysqlOptions =>
                    {
                        
                        mysqlOptions.ServerVersion(new Version(10, 1, 16), ServerType.MariaDb); // replace with your Server Version and Type
                    }
                ));

            var key = Encoding.ASCII.GetBytes(this.Configuration["JWTTokenSecret"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = int.Parse(context.Principal.Identity.Name);
                            var user = userService.GetById(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }

                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            // configure DI for application services
            AddDIConfig(services);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WPUser, LaDOSE.DTO.WPUserDTO>();
                cfg.CreateMap<WPUser, LaDOSE.DTO.WPUserDTO>();
                cfg.CreateMap<WPEvent, LaDOSE.DTO.WPEventDTO>();
                cfg.CreateMap<Result, LaDOSE.DTO.ResultDTO>();
                cfg.CreateMap<TournamentsResult, LaDOSE.DTO.TournamentsResultDTO>();
                cfg.CreateMap<Participent, LaDOSE.DTO.ParticipentDTO>();
                cfg.CreateMap<Tournament, LaDOSE.DTO.TournamentDTO>();


                cfg.CreateMap<ApplicationUser, LaDOSE.DTO.ApplicationUserDTO>();
                cfg.CreateMap<WPBooking, LaDOSE.DTO.WPBookingDTO>().ForMember(e=>e.Meta,opt=>opt.MapFrom(s=>s.Meta.CleanWpMeta()));
                cfg.CreateMapTwoWay<Game, LaDOSE.DTO.GameDTO>();
                cfg.CreateMapTwoWay<Todo, LaDOSE.DTO.TodoDTO>();

            });
        }

        private void AddDIConfig(IServiceCollection services)
        {
            
            services.AddTransient<IChallongeProvider>(p => new ChallongeProvider(this.Configuration["ApiKey:ChallongeApiKey"]));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ISeasonService, SeasonService>();
            services.AddScoped<IWordPressService, WordPressService>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ITournamentService, TournamentService>();
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
