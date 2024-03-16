using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;

namespace LaDOSE.Api;

#pragma warning disable EF1001
public class NpgsqlSqlGenerationLowercaseHelper : NpgsqlSqlGenerationHelper

{
        
    static string ToLowerCase(string input) => input.ToLower();
    public NpgsqlSqlGenerationLowercaseHelper(RelationalSqlGenerationHelperDependencies dependencies) 
        : base(dependencies) { }
    public override string DelimitIdentifier(string identifier)
        => base.DelimitIdentifier(ToLowerCase(identifier));
    public override void DelimitIdentifier(StringBuilder builder, string identifier)
        => base.DelimitIdentifier(builder, ToLowerCase(identifier));
}
#pragma warning restore EF1001