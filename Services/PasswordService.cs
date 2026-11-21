using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace obrazo.Services
{
    public class PasswordService
    {
        public static string HashPassword (string senha) => BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);

        public static bool VerifyPassword (string senha, string hashedSenha) => BCrypt.Net.BCrypt.Verify(senha, hashedSenha);
    }
}