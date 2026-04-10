using Dapper;
using Npgsql;
using System.Data;
using VocTrainer.Models;

namespace VocTrainer.Services;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        parameter.DbType = DbType.Date;
    }

    public override DateOnly Parse(object value) => value is DateOnly d ? d : DateOnly.FromDateTime((DateTime)value);
}

public class VocService(IConfiguration config)
{
    private IDbConnection Db => new NpgsqlConnection(
        config.GetConnectionString("Postgres"));

    public async Task<IEnumerable<Vocabulary>> GetAllAsync() =>
        await Db.QueryAsync<Vocabulary>(
            "SELECT * FROM vocabulary ORDER BY date DESC");

    public async Task<Vocabulary?> GetRandomAsync() =>
        await Db.QueryFirstOrDefaultAsync<Vocabulary>(
            "SELECT * FROM vocabulary ORDER BY RANDOM() LIMIT 1");

    public async Task AddAsync(Vocabulary vocab) =>
        await Db.ExecuteAsync(
            "INSERT INTO vocabulary (date, german, english) VALUES (@Date, @German, @English)", vocab);

    public async Task UpdateAsync(Vocabulary vocab) =>
        await Db.ExecuteAsync(
            "UPDATE vocabulary SET date = @Date, german = @German, english = @English WHERE id = @Id", vocab);

    public async Task DeleteAsync(int id) =>
        await Db.ExecuteAsync(
            "DELETE FROM vocabulary WHERE id = @Id", new { Id = id });
}
