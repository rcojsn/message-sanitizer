namespace AdminService.Domain.SensitiveWords;

public record SensitiveWord
{
    public Guid Id { get; set; }
    public required string Word { get; set; }

    public void Deconstruct(out Guid id, out string word)
    {
        id = Id;
        word = Word;
    }
};