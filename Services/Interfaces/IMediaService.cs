using obrazo.Models.Entities;

namespace obrazo.Services.Interfaces
{
    public interface IMediaService
    {
        Task<Media> GetById(int id);

        Task<List<Media>> GetByType(int type);
        Task<List<Media>> Search(string? name, int? type);
        Task<Media> Create(Media media);

        Task<bool> Update(int id, Media updated);

        Task<bool> Delete(int id);
    }
}