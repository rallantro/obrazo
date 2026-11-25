namespace obrazo.Models.Entities
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<UserMedia> UserMedias { get; set; } = new List<UserMedia>();
    }
}