using System.ComponentModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using obrazo.Data;
using obrazo.Models.Entities;
using obrazo.Models.Enum;
using obrazo.Services.Interfaces;

namespace obrazo.Services
{
    public class MediaService : IMediaService
    {
        private readonly AppDbContext Context;

        public MediaService(AppDbContext context)
        {
            Context = context;
        }
        public async Task<Media> GetById(int id)
        {
            var media = await Context.Medias.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Nenhuma mídia encontrada.");
            return media;
        }


        public async Task<List<Media>> GetByType(int type)
        {
            if (!Enum.IsDefined(typeof(MediaType), type))
                throw new InvalidEnumArgumentException("Tipo de mídia inválido.");
            var media = await Context.Medias.Where(x => x.Type == (MediaType)type).ToListAsync();
            if (media.Count == 0)
            {
                string nameType = ((MediaType)type).ToString();
                throw new ArgumentException($"Você não possui nenhum(a) {nameType} armazenado(a)... :p");
            }
            return media;
        }

        public async Task<List<Media>> Search(string? search, int? type)
        {
            var query = Context.Medias.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{search}%"));
            if (type != null)
            {
                if (!Enum.IsDefined(typeof(MediaType), type.Value))
                {
                    throw new InvalidEnumArgumentException("Tipo de mídia inválido.");
                }
                query = query.Where(x => x.Type == (MediaType)type.Value);
            }
            var media = await query.ToListAsync();
            if (media.Count == 0)
            {
                throw new ArgumentException($"Nenhuma obra encontrada com os critérios informados.");
            }
            return media;
        }

        public async Task<Media> Create(Media media)
        {
            string validName = ValidateName(media);
            if (!string.IsNullOrWhiteSpace(validName))
                throw new ArgumentException(validName);
            string validType = ValidateType(media);
            if (!string.IsNullOrWhiteSpace(validType))
                throw new InvalidEnumArgumentException(validType);
            string validDuplicate = await ValidateDuplicate(media, null);
            if (!string.IsNullOrWhiteSpace(validDuplicate))
                throw new DuplicateNameException(validDuplicate);
            await Context.Medias.AddAsync(media);
            await Context.SaveChangesAsync();
            return media;
        }

        public async Task<bool> Update(int id, Media updated)
        {
            var existing = await Context.Medias.FindAsync(id) ??
                throw new KeyNotFoundException("Mídia não encontrada.");
            string validName = ValidateName(updated);
            if (!string.IsNullOrWhiteSpace(validName))
                throw new ArgumentNullException(validName);
            string validType = ValidateType(updated);
            if (!string.IsNullOrWhiteSpace(validType))
                throw new InvalidEnumArgumentException(validType);
            string validDuplicate = await ValidateDuplicate(updated, id);
            if (!string.IsNullOrWhiteSpace(validDuplicate))
                throw new DuplicateNameException(validDuplicate);
            existing.Name = updated.Name;
            existing.Type = updated.Type;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await Context.Medias.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException("Mídia não encontrada.");
            Context.Medias.Remove(existing);
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<string> ValidateDuplicate(Media newmedia, int? id)
        {
            if (await Context.Medias.AnyAsync(x => x.Name == newmedia.Name && x.Type == newmedia.Type && x.Id != id))
            {
                return "Esta mídia já existe.";
            }
            return "";
        }
        private string ValidateName(Media newMedia)
        {
            if (string.IsNullOrWhiteSpace(newMedia.Name))
            {
                return "Insira um nome válido.";
            }
            return "";
        }
        private string ValidateType(Media newMedia)
        {
            if (!Enum.IsDefined(typeof(MediaType), newMedia.Type))
            {
                return "Tipo de mídia inválido.";
            }
            return "";
        }

    }
}