namespace myRecipeBook.Domain.Entities
{
    public class User
    {
        // Id e a chave primaria do usuario, e o Guid.CreateVersion7()
        // gera um novo Guid unico para cada usuario criado
        // as 48 primeiras posicoes do Guid sao baseadas no tempo,
        // e as 12 ultimas posicoes sao aleatorias, garantindo a unicidade do Id
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public bool Active { get; set; } = true;  
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
