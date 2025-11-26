using System;
using System.ComponentModel.DataAnnotations;

public class SessionEntry
{
    [Key]
    public string Id { get; set; }  // Session Id

    [Required]
    public byte[] Value { get; set; }  // Session verisi

    [Required]
    public DateTimeOffset ExpiresAtTime { get; set; }  // Session bitiş zamanı

    public long? SlidingExpirationInSeconds { get; set; }  // Opsiyonel

    public DateTimeOffset? AbsoluteExpiration { get; set; }  // Opsiyonel
}
