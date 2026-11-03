using NotificationLibrary.Enums;

namespace NotificationLibrary.Models;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Priority { get; set; } // 0 (низкий) до 5 (высокий)
    public bool IsRead { get; set; }
}
