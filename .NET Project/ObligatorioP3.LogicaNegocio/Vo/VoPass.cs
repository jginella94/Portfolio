using ObligatorioP3.LogicaNegocio.Excepciones;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoPass
    {
        private const int Iteraciones = 210000;
        private const int LargoSal = 16;
        private const int LargoHash = 32;
        private const string Algoritmo = "PBKDF2-SHA256";

        public string Value { get; private set; } = string.Empty;

        private VoPass() { }

        public VoPass(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new PassInvalidoException("La contraseña no puede estar vacía.");

            if (!Regex.IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$"))
                throw new PassInvalidoException("La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y caracteres especiales.");

            byte[] sal = RandomNumberGenerator.GetBytes(LargoSal);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(value, sal, Iteraciones, HashAlgorithmName.SHA256, LargoHash);
            Value = $"{Algoritmo}${Iteraciones}${Convert.ToBase64String(sal)}${Convert.ToBase64String(hash)}";
        }

        public bool Verificar(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            string[] partes = Value.Split('$');
            if (partes.Length != 4 || partes[0] != Algoritmo ||
                !int.TryParse(partes[1], out int iteraciones) ||
                iteraciones < 100000 || iteraciones > 1000000)
                return false;

            try
            {
                byte[] sal = Convert.FromBase64String(partes[2]);
                byte[] hashAlmacenado = Convert.FromBase64String(partes[3]);
                if (sal.Length != LargoSal || hashAlmacenado.Length != LargoHash)
                    return false;

                byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(password, sal, iteraciones, HashAlgorithmName.SHA256, hashAlmacenado.Length);
                return CryptographicOperations.FixedTimeEquals(hashIngresado, hashAlmacenado);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
