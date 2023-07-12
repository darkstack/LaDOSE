using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;

namespace LaDOSE.Api;

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