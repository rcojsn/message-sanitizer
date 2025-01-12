namespace AdminService.Domain.SensitiveWords;

public record SensitiveWord
{
    public Guid Id { get; set; }
    public required string Word { get; set; }
};