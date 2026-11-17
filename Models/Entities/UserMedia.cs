using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using obrazo.Models.Enum;

namespace obrazo.Models.Entities
{
    public class UserMedia
    {
        public UserMedia() { }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public double Nota { get; set; }
        public string Opiniao { get; set; } = string.Empty;
        public MediaStatus Status { get; set; }
        public DateTime DataAssistiu { get; set; }

        public User? User { get; set; }
        public Media? Media { get; set; }

        public UserMedia(int userId, int mediaId, int nota, string opiniao, MediaStatus status, DateTime dataAssistiu){
            UserId = userId;
            MediaId = mediaId;
            Nota = nota;
            Opiniao = opiniao;
            Status = status;
            DataAssistiu = dataAssistiu;
        }
    }
}