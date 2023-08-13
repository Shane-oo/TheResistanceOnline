using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheResistanceOnline.Infrastructure.Data.Migrations;

public static class MigrationExtensions
{
    public static void RunSqlScript(this MigrationBuilder migrationBuilder, string script)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith($"{script}"));
        using var stream = assembly.GetManifestResourceStream(resourceName ?? throw new InvalidOperationException($"Cannot Find Sql Script {script}"));
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException());
        var sqlResult = reader.ReadToEnd();
        migrationBuilder.Sql(sqlResult);
    }
}