using System;
using System.Text;
using AutoMapper;
using ListaccFinance.Api.Data;
using ListaccFinance.Api.Repo;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ListaccFinance.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // Local DB connection string: "Server=OLUWAMUYIWA\\SQLSERVER;Database=ListaccFinance;user id=sa;password=listacc1"
        // Better Online Db Conn string : "Server=tcp:s19.winhost.com;Database=DB_135236_listaccfin;user id=DB_135236_listaccfin_user;password=Oghuan6789"
        // Online Db connection strinG :  // "Data Source=tcp:s19.winhost.com;Initial Catalog=DB_135236_listaccfin;User ID=DB_135236_listaccfin_user;Password=******;Integrated Security=False;"
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //DI
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDesktopService, DesktopService>();
            services.AddScoped<ISyncService, SyncService>();
            services.AddScoped<IOtherServices, OtherServices>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IDeptService, DeptService>();

            //services.AddScoped(typeof(ISyncService<>),typeof(SyncService<>));
            //DBContext

            services.AddDbContext<DatingContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // services.AddDbContext<DataContext>(con => con.UseSqlServer(
            //     Configuration.GetConnectionString("DefaultConnection")));


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc(opt => {
                opt.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Latest).AddJsonOptions(Options =>
                            {

                            });


            // Identity Builder
            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(
            Configuration.GetConnectionString("DefaultConnection"))
            );


            // Token Verification 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetSection("LoginSettings:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


            app.UseAuthorization();
            app.UseAuthentication();
            app.UseMvc();

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });*/


        }
    }
}
