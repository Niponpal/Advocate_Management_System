using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AMS.Extension;

public static class ModelBuilderExtension
{
    public static IEnumerable<IMutableEntityType> EntityTypes(this ModelBuilder builder)
    {
        return builder.Model.GetEntityTypes();
    }

    public static IEnumerable<IMutableProperty> Properties(this ModelBuilder builder)
    {
        return builder.EntityTypes().SelectMany(entityType => entityType.GetProperties());
    }

    public static IEnumerable<IMutableProperty> Properties<T>(this ModelBuilder builder)
    {
        return builder.EntityTypes().SelectMany(entityType => entityType.GetProperties().Where(x => x.ClrType == typeof(T)));
    }

    public static void Configure(this IEnumerable<IMutableEntityType> entityTypes, Action<IMutableEntityType> convention)
    {
        foreach (var entityType in entityTypes)
        {
            convention(entityType);
        }
    }

    public static void Configure(this IEnumerable<IMutableProperty> propertyTypes, Action<IMutableProperty> convention)
    {
        foreach (var propertyType in propertyTypes)
        {
            convention(propertyType);
        }
    }


    public static void DateTimeConvention(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(DateTime)
                    || p.ClrType == typeof(DateTime?))
           .ToList()
           .ForEach(p =>
           {
               p.SetColumnType("datetime2");
           });
    }

    public static void ConfigureDecimalProperties(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal)
                    || p.ClrType == typeof(decimal?))
            .ToList()
            .ForEach(p =>
            {
                p.SetColumnType("decimal(18,2)");
                p.SetPrecision(18);
                p.SetScale(2);
            });
    }

    public static void DecimalConvention(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal)
                     || p.ClrType == typeof(decimal?))
            .ToList()
            .ForEach(p =>
            {
                if (p.GetPrecision() is null)
                    p.SetPrecision(18);
                if (p.GetScale() is null)
                    p.SetScale(4);
            });
    }

    public static void RelationConvetion(this ModelBuilder modelBuilder)
    {
        modelBuilder.EntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .ToList()
            .ForEach(relationship =>
            {
                // Ensure foreign key type matches the principal key type
                if (relationship.PrincipalKey.Properties[0].ClrType == typeof(long))
                {
                    relationship.Properties[0].SetColumnType("bigint");
                    relationship.Properties[0].SetDefaultValueSql("0");
                }
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            });
    }

    public static void PluralzseTableNameConventions(this ModelBuilder modelBuilder, bool pluralize = true)
    {
        if (!pluralize)
            modelBuilder.EntityTypes().Configure(e => e.SetTableName(e.DisplayName()));
    }
}