using NotificationLibrary.Enums;

namespace NotificationLibrary.Models;

public class NotificationFilterOptions
{
    public bool? IsRead { get; set; }
    public NotificationType[]? Types { get; set; }
    public string? SearchText { get; set; }
    public int? MinPriority { get; set; }
    public SortNotificationBy? SortBy { get; set; }
    public bool Descending { get; set; }
}
