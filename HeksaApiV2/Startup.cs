using DBMaster = HeksaApiV2.DataAccess.MasterData;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using HeksaApiV2.Logic;

namespace HeksaApiV2
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
            services.AddDataProtection();         // Fixes the 'IDataProtectionProvider' error
            services.AddDistributedMemoryCache(); // Provides storage for the session
            services.AddSession(options => {      // Configures session behavior
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Database Component Injection

            ///Database Connection String Master 
            services.AddScoped<DBMaster.Providers.IDbConnectionFactory>(options =>
            {
                var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DBMasterConnectionString"));

                return new DBMaster.Providers.DbConnectionFactory(() =>
                {
                    var conn = new SqlConnection(builder.ConnectionString);

                    conn.Open();
                    return conn;
                });
            });

            services.AddScoped<DBMaster.Providers.IDbContext, DBMaster.Providers.DbContext>();
            services.AddScoped<DBMaster.Objects.ISQLParam, DBMaster.Objects.CustomSQLParam>();

            #endregion Database Component Injection


            #region Database Entity Injection

            services.AddScoped<DBMaster.Entities.IEntity, DBMaster.Entities.ListStoredProcedureEntity>();
            services.AddScoped<DBMaster.Entities.IEntity, DBMaster.Entities.AgenEntity>();

            #endregion

            #region Database Repository Injection

            services.AddScoped<DBMaster.Repositories.IListStoredProcedureRepository, DBMaster.Repositories.ListStoredProcedureRepository>();
            services.AddScoped<DBMaster.Repositories.IAgenRepository, DBMaster.Repositories.AgenRepository>();

            #endregion

            #region Logic Injection

            services.AddScoped<IAgenCoreLogic, AgenCoreLogic>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            /// Global Cors Policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication(); // penting: ini HARUS sebelum UseAuthorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
