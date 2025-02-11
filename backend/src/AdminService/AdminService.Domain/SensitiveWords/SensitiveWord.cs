﻿namespace AdminService.Domain.SensitiveWords;

public record SensitiveWord(Guid Id, string Word)
{
    public void Deconstruct(out Guid id, out string word)
    {
        id = Id;
        word = Word;
    }
};