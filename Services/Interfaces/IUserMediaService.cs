using obrazo.Models.Entities;

namespace obrazo.Services.Interfaces
{
    public interface IUserMediaService
    {
        Task<UserMedia?> GetById(int id, int userId);

        Task<List<UserMedia>> GetByUser(int userId);

        Task<List<UserMedia>> GetByType(int userId, int type);
        Task<List<UserMedia>> GetByStatus(int userId, int status);
        Task<List<UserMedia>> Search(int userId, string? name, int? type);
        Task<UserMedia> Create(UserMedia media, int userId);

        Task<bool> Update(int id, UserMedia updated, int userId);

        Task<bool> Delete(int id, int userId);
    }
}