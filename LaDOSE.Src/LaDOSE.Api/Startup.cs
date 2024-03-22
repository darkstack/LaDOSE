using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Service;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using LaDOSE.Api.Helpers;
using LaDOSE.Business.Helper;
using LaDOSE.Business.Provider.ChallongProvider;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Wordpress;
using Result = LaDOSE.Entity.Challonge.Result;
using LaDOSE.Entity.BotEvent;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

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
            services.AddMvc().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                x.SerializerSettings.MaxDepth= 4;
            });
            // services.AddDbContextPool<LaDOSEDbContext>( // replace "YourDbContext" with the class name of your DbContext
            //     
            //     options => options.UseMySql($"Server={MySqlServer};Database={MySqlDatabase};User={MySqlUser};Password={MySqlPassword};", // replace with your Connection String
            //         mysqlOptions =>
            //         {
            //             mysqlOptions.ServerVersion(new Version(10, 1, 16), ServerType.MariaDb); // replace with your Server Version and Type
            //         }
            //     ));
            services.AddDbContextPool<LaDOSEDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DbContext")).ReplaceService<ISqlGenerationHelper,NpgsqlSqlGenerationLowercaseHelper>();
            });
            
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
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WPUser, LaDOSE.DTO.WPUserDTO>();
                cfg.CreateMap<WPUser, LaDOSE.DTO.WPUserDTO>();
                cfg.CreateMap<WPEvent, LaDOSE.DTO.WPEventDTO>();
                cfg.CreateMap<Result, LaDOSE.DTO.ResultDTO>();
                cfg.CreateMap<Event, LaDOSE.DTO.EventDTO>();
                cfg.CreateMap<BotEventResult, LaDOSE.DTO.BotEventResultDTO>();
                cfg.CreateMap<BotEvent, LaDOSE.DTO.BotEventDTO>();

                cfg.CreateMap<TournamentsResult, LaDOSE.DTO.TournamentsResultDTO>();
                cfg.CreateMap<ChallongeParticipent, LaDOSE.DTO.ParticipentDTO>();
                cfg.CreateMap<ChallongeTournament, LaDOSE.DTO.TournamentDTO>();


                cfg.CreateMap<ApplicationUser, LaDOSE.DTO.ApplicationUserDTO>();
                cfg.CreateMap<WPBooking, LaDOSE.DTO.WPBookingDTO>().ForMember(e=>e.Meta,opt=>opt.MapFrom(s=>s.Meta.CleanWpMeta()));
                cfg.CreateMapTwoWay<Game, LaDOSE.DTO.GameDTO>();
                cfg.CreateMapTwoWay<Todo, LaDOSE.DTO.TodoDTO>();

            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        private void AddDIConfig(IServiceCollection services)
        {
            
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IWordPressService, WordPressService>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IBotEventService, BotEventService>();
            
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddTransient<IChallongeProvider>(p => new ChallongeProvider(   p.GetRequiredService<IGameService>(),
                                                                                    p.GetRequiredService<IEventService>(),
                                                                                    p.GetRequiredService<IPlayerService>(), 
                                                                                    this.Configuration["ApiKey:ChallongeApiKey"]));

            services.AddTransient<ISmashProvider>(p => new SmashProvider(   p.GetRequiredService<IGameService>(), 
                                                                            p.GetRequiredService<IEventService>(),
                                                                            p.GetRequiredService<IPlayerService>(),
                                                                            this.Configuration["ApiKey:SmashApiKey"]));
            services.AddScoped<IExternalProviderService, ExternalProviderService>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(x => x
                //.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
