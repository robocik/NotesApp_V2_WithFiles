using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.NetCore;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Mappings;

namespace NoteBookApp.Server.Infrastructure
{
    public static class NHibernateSqlServerInstaller
    {
        public static IServiceCollection AddNHibernateSqlServer(this IServiceCollection services, string cnString)
        {
            var cfg = new Configuration();

            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<MicrosoftDataSqlClientDriver>();
                db.ConnectionProvider<DriverConnectionProvider>();
                db.LogSqlInConsole = true;
                db.ConnectionString = cnString;
                db.Timeout = 30;/*seconds*/
                db.SchemaAction = SchemaAutoAction.Validate;
            });

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .AddHibernateStores();
            cfg.Cache(c => c.UseQueryCache = false);

            var mapping = new ModelMapper();
            mapping.AddMappings(typeof(ApplicationUserMapping).Assembly.GetTypes());
            mapping.AddMapping(typeof(NHibernate.AspNetCore.Identity.Mappings.IdentityUserMappingMsSql));
            mapping.AddMapping(typeof(NHibernate.AspNetCore.Identity.Mappings.IdentityUserLoginMappingMsSql));
            mapping.AddMapping(typeof(NHibernate.AspNetCore.Identity.Mappings.IdentityUserTokenMappingMsSql));
            var mappingDocument = mapping.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(mappingDocument);
            services.AddHibernate(cfg);
#if DEBUG
            cfg.BuildSessionFactory();
#endif

            return services;
        }
    }
}