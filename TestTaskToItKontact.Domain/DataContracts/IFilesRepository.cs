namespace TestTaskToItKontact.Domain.DataContracts;

public interface IFilesRepository
{
    Task AddAsync(IEnumerable<byte> file);

    Task<ICollection<byte>> GetFileByIdAsync(Guid id);

    Task UpdateFileAsync(Guid id, IEnumerable<byte> file);

    Task DeleteFileAsync(IEnumerable<byte> file);
}