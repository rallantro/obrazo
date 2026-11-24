using System.ComponentModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using obrazo.Data;
using obrazo.Models.Entities;
using obrazo.Models.Enum;
using obrazo.Services.Interfaces;

namespace obrazo.Services
{
    public class UserMediaService : IUserMediaService
    {
        private readonly AppDbContext Context;

        public UserMediaService(AppDbContext context)
        {
            Context = context;
        }
        public async Task<UserMedia?> GetById(int id, int userId)
        {
            var userMedia = await Context.UserMedias.Where(x => x.Id == id).Include(um => um.Media).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Nenhuma mídia encontrada.");
            if (userMedia.UserId != userId) throw new UnauthorizedAccessException("Acesso Negado.");
            return userMedia;
        }

        public async Task<List<UserMedia>> GetByUser(int userId)
        {
            var userMedia = await Context.UserMedias.Where(x => x.UserId == userId).Include(um => um.Media).ToListAsync();
            if (userMedia.Count == 0)
            {
                throw new ArgumentException("Você não possui nenhuma mídia armazenada... :p");
            }
            return userMedia;
        }

        public async Task<List<UserMedia>> GetByStatus(int userId, int status)
        {
            var userMedia = await Context.UserMedias.Include(um => um.Media).Where(x => x.UserId == userId && x.Status == (MediaStatus)status).ToListAsync();
            if (userMedia.Count == 0)
            {
                string nameStatus = ((MediaStatus)status).ToString();
                throw new ArgumentException($"Você não possui nenhuma obra {nameStatus} armazenada... :p");
            }
            return userMedia;
        }

        public async Task<List<UserMedia>> GetByType(int userId, int type)
        {
            var userMedia = await Context.UserMedias.Include(um => um.Media).Where(x => x.UserId == userId && x.Media.Type == (MediaType)type).ToListAsync();
            if (userMedia.Count == 0)
            {
                string nameType = ((MediaType)type).ToString();
                throw new ArgumentException($"Você não possui nenhum(a) {nameType} armazenado(a)... :p");
            }
            return userMedia;
        }

        public async Task<List<UserMedia>> Search(int userId, string? search, int? type)
        {
            var query = Context.UserMedias.Include(um => um.Media).Where(x => x.UserId == userId);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => EF.Functions.Like(x.Media.Name, $"%{search}%"));
            if (type != null)
            {
                if (!Enum.IsDefined(typeof(MediaType), type.Value))
                {
                    throw new InvalidEnumArgumentException("Tipo de mídia inválido.");
                }
                query = query.Where(x => x.Media.Type == (MediaType)type);
            }
            var userMedia = await query.ToListAsync();
            if (userMedia.Count == 0)
            {
                throw new ArgumentException($"Nenhuma obra encontrada com os critérios informados.");
            }
            return userMedia;
        }

        public async Task<UserMedia> Create(UserMedia userMedia, int userId)
        {
            userMedia.UserId = userId;
            string validExist = await ValidateExistence(userMedia);
            if (!string.IsNullOrWhiteSpace(validExist))
                throw new KeyNotFoundException(validExist);
            string validDuplicate = await ValidateDuplicate(userMedia);
            if(!string.IsNullOrWhiteSpace(validDuplicate))
                throw new DuplicateNameException(validDuplicate);
            string validUser = ValidateUserMedia(userMedia);
            if (!string.IsNullOrWhiteSpace(validUser))
                throw new ArgumentException(validUser);
            Context.UserMedias.Add(userMedia);
            await Context.SaveChangesAsync();
            return userMedia;
        }

        public async Task<bool> Update(int id, UserMedia updated, int userId)
        {
            var existing = await Context.UserMedias.FindAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Mídia não encontrada.");
            if (existing.UserId != userId)
                throw new UnauthorizedAccessException("Acesso Negado!");
            string validUser = ValidateUserMedia(updated);
            if (!string.IsNullOrWhiteSpace(validUser))
                throw new ArgumentException(validUser);
            existing.Nota = updated.Nota;
            existing.Opiniao = updated.Opiniao;
            existing.Status = updated.Status;
            existing.DataAssistiu = updated.DataAssistiu;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var existing = await Context.UserMedias.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException("Mídia não encontrada.");
            if (existing.UserId != userId) throw new UnauthorizedAccessException("Acesso Negado!");
            Context.UserMedias.Remove(existing);
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<string> ValidateExistence(UserMedia newUserMedia)
        {
            if (! await Context.Medias.AnyAsync(m => m.Id == newUserMedia.MediaId))
            {
                return "Mídia não encontrada.";
            }
            return "";
        }

        private async Task<string> ValidateDuplicate(UserMedia newUserMedia)
        {
            if (await Context.UserMedias.AnyAsync(x => x.UserId == newUserMedia.UserId && x.MediaId == newUserMedia.MediaId))
            {
                return "Esta mídia já está no diário do usuário.";
            }
            return "";
        }

        private string ValidateUserMedia(UserMedia newUserMedia)
        {
            if (!Enum.IsDefined(typeof(MediaStatus), newUserMedia.Status))
            {
                return "Status da mídia inválido.";

            }
            if (newUserMedia.Nota < 0 || newUserMedia.Nota > 5)
            {
                return "A nota deve ser entre 0 e 5.";

            }
            if (newUserMedia.DataAssistiu > DateTime.Today)
            {
                return "A data não pode ser no futuro.";

            }
            return "";
        }

    }
}