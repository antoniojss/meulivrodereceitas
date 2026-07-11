using Konscious.Security.Cryptography;
using myRecipeBook.Domain.Security.PasswordHashing;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace myRecipeBook.Infrastructure.Security.PasswordHashing
{
    // OWASP o proque de não cncrypitar a senha do usuario(não deve ser quebrada(descriptografar))
    // esta classe é responsavel por implementar o algoritmo de hash de senha Argon2,
    // que e um algoritmo de hash de senha seguro e resistente a ataques de força bruta.
    // A classe Argon2PasswordHasher e usada para gerar hashes de senha e verificar senhas fornecidas pelo usuário.
    // ela não permite descriptografar a senha original a partir do hash, garantindo a segurança das senhas armazenadas.

    // precisa ser sealed para que não possa ser herdada, e internal para que não possa ser acessada fora do assembly
    internal sealed class Argon2PasswordHasher : IPasswordHasher
    {
        private const int DEGREE_OF_PARALLELISM = 1; // Número de threads a serem usadas para o cálculo do hash
        private const int ITERATIONS = 2; // Número de (vezes) iterações a serem realizadas para o cálculo do hash
        private const int MEMORY_SIZE = 20 * 1024;  // Tamanho da memória a ser usada para o cálculo do hash (em KB)
        private const int SALT_SIZE = 16; // Tamanho do salt a ser gerado (em bytes)  
        private const int HASH_SIZE = 32; // Tamanho do hash a ser gerado (em bytes)    
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SALT_SIZE); // Gera um salt aleatório 

            var hash = HasPassword(password, salt); // Gera o hash da senha usando o salt gerado

            var combinedBytes = new byte[salt.Length + hash.Length]; // Cria um array para armazenar o salt e o
                                                                     // hash juntos

            salt.CopyTo(combinedBytes); // Copia o salt para o array combinado

            salt.CopyTo(combinedBytes, index: salt.Length); // Copia o hash para o array combinado apartir da 16 posição

            return Convert.ToBase64String(combinedBytes); // Retorna o hash da senha em formato Base64
        }
        public bool VerifyPassword(string password, string passwordHash)
        {
            var combinedBytes = Convert.FromBase64String(passwordHash); // Converte o hash da senha de Base64
                                                                        // para bytes    

            var salt = new byte[SALT_SIZE]; // Cria um array para armazenar o salt

            var hash = new byte[HASH_SIZE]; // Cria um array para armazenar o hash

            Array.Copy(combinedBytes, 0, salt, 0, SALT_SIZE); // Copia o salt do array combinado para o array de salt
                                                              // da posição 0 ate a 15 

            Array.Copy(combinedBytes, SALT_SIZE, hash, 0, HASH_SIZE); // Copia o hash do array combinado para o array
                                                                      // de hash aqui vai pegar da 16 ate a 32 

            var newHash = HasPassword(password, salt); // Gera um novo hash da senha fornecida pelo usuário
                                                       // usando o salt extraído do hash armazenado

            return CryptographicOperations.FixedTimeEquals(newHash, hash); // Compara o novo hash com o hash
                                                                           // armazenado de forma segura (tempo fixo)
                                                                           // e retorna true se forem iguais,
                                                                           // caso contrario false
        }
        private byte[] HasPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var hashAlgorithm = new Argon2id(passwordBytes)
            {
                DegreeOfParallelism = DEGREE_OF_PARALLELISM,
                Iterations = ITERATIONS,
                MemorySize = MEMORY_SIZE,
                Salt = salt,
            };

            return hashAlgorithm.GetBytes(HASH_SIZE); // devolve a hash da senha apos a validação do salt e da senha
        }
    }
}
