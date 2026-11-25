using obrazo.Models.Enum;

namespace obrazo.Models.Entities
{
    public class Media
    {
        public Media() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MediaType Type { get; set; }

        public Media(string name, MediaType type)
        {
            Name = name;
            Type = type;
        }

        public ICollection<UserMedia> UserMedias { get; set; } = new List<UserMedia>();
    }
}