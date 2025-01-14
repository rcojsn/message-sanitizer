namespace BleepGuard.Contracts.SensitiveWords;

public record SensitiveWordResponse(Guid Id, string Word)
{
    public void Deconstruct(out Guid id, out string word)
    {
        id = Id;
        word = Word;
    }
}
