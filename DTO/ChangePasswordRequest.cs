using System.ComponentModel.DataAnnotations;

namespace obrazo.DTO
{
    public class ChangePasswordRequest
    {
        public ChangePasswordRequest() { }

        [Required(ErrorMessage = "É necessário informar a senha antiga.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha precisa ter no mínimo 6 caracteres.")]
        [RegularExpression(@".*\d.*", ErrorMessage = "A senha precisa ter no mínimo 1 número.")]
        public string NewPassword { get; set; } = string.Empty;

        public ChangePasswordRequest(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}