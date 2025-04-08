using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using TestTaskToItKontact.Data.Settings;
using TestTaskToItKontact.Domain.DataContracts;

namespace TestTaskToItKontact.Data.Repositories;

internal sealed class FileRepository : IFilesRepository
{
    private readonly IOptionsSnapshot<DataAccessOptions> _options;

    public FileRepository(IOptionsSnapshot<DataAccessOptions> options)
    {
        _options = options;
    }

    public async Task AddAsync(IEnumerable<byte> file)
    {
        await using (var connection = new SqlConnection(_options.Value.ConnectionString))
        {
            const string sql = "INSERT INTO StoredFile (Id, FileData) VALUES (@Id, @FileData)";

            try
            {
                await connection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    FileData = file.ToArray()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task<ICollection<byte>> GetFileByIdAsync(Guid id)
    {
        await using (var connection = new SqlConnection(_options.Value.ConnectionString))
        {
            const string sql = "SELECT TOP 1 FileData FROM StoredFile WHERE Id = @Id";

            var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql, new
            {
                Id = id
            });
            
            return result ?? Array.Empty<byte>();
        }
    }

    public async Task UpdateFileAsync(Guid id, IEnumerable<byte> file)
    {
        await using (var connection = new SqlConnection(_options.Value.ConnectionString))
        {
            const string sql = "UPDATE StoredFile SET FileData = @FileData, CreatedAt = GETUTCDATE() WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                FileData = file.ToArray(),
                Id = Guid.NewGuid()
            });
        }
    }

    public async Task DeleteFileAsync(IEnumerable<byte> file)
    {
        await using (var connection = new SqlConnection(_options.Value.ConnectionString))
        {
            const string sql = "DELETE FROM StoredFile WHERE FileData = @FileData";

            await connection.ExecuteAsync(sql, new
            {
                FileData = file.ToArray()
            });
        }
    }
}