using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace TheResistanceOnline.Infrastructure.Data.Migrations;

public static class MigrationExtensions
{
    #region Public Methods

    public static OperationBuilder<SqlOperation> SqlResource(this MigrationBuilder migrationBuilder, string scriptName)
    {
        var assembly = typeof(MigrationExtensions).Assembly;
        var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(scriptName, StringComparison.CurrentCultureIgnoreCase));

        string sql;
        using(var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load resource stream");
            }

            using var reader = new StreamReader(stream);
            sql = reader.ReadToEnd();
        }

        return migrationBuilder.Sql(sql);
    }

    #endregion
}
