namespace Web.Models;

public record HistoryRequest(DateTimeOffset Start, DateTimeOffset End, Guid AccountId);
