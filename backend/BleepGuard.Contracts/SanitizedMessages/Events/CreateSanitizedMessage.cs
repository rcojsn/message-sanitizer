namespace BleepGuard.Contracts.SanitizedMessages.Events;

public record CreateSanitizedMessage(string Message) : CreateSanitizedMessageRequest(Message);