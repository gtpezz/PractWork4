using NotificationLibrary.Models;

namespace NotificationLibrary.Interfaces;

public interface INotificationFilterAndSortService
{
    IEnumerable<Notification> FilterAndSort(
        IEnumerable<Notification> notifications,
        NotificationFilterOptions options
    );
}
