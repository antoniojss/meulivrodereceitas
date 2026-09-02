namespace myRecipeBook.Domain.Entities
{
    public abstract class EntityBase
    {
        // o indice da tabela e gerado automatomaticamente 
        // por esta função Guid .CreateVersion7() que gera um Id automatico.
        // garantindo a unicidade e ordenação temporal.
        // Só que ao tentar usar o endpoint UPDATE da api na entidade 
        // (deletando e recriando a tabela DishTypes por exemplo)
        // O guid gerado não esta no banco de dados ainda, então a receita não é encontrada
        // retornndo erro, e não é possivel dar Update.
        // wellison 02-09-2026 explicação 
        // colocar ValueGenerateNever() no modelbuilder para não gerar o guid automatico,
        // e sim pegar o guid do objeto que esta sendo atualizado.
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public bool Active { get; set; } = true;
    }
}