using NotificationLibrary.Enums;
using NotificationLibrary.Interfaces;
using NotificationLibrary.Models;

namespace NotificationLibrary.services;

public sealed class NotificationFilterAndSortService : INotificationFilterAndSortService
{
    private static readonly Dictionary<
        string,
        Func<NotificationFilterOptions, Predicate<Notification>?>> FilterRules = new()
        {
            ["IsRead"] = o => o.IsRead is bool isRead
                ? n => n.IsRead == isRead
                : null,

            ["Types"] = o => o.Types is { Length: > 0 } types
                ? n => types.Contains(n.Type)
                : null,

            ["SearchText"] = o => string.IsNullOrWhiteSpace(o.SearchText)
                ? null
                : n => n.Title.Contains(o.SearchText!, StringComparison.OrdinalIgnoreCase)
                    || (n.Content?.Contains(o.SearchText!, StringComparison.OrdinalIgnoreCase) ?? false),

            ["MinPriority"] = o => o.MinPriority is int min
                ? n => n.Priority >= min
                : null
        };

    private static readonly Dictionary<
        SortNotificationBy,
        Func<IEnumerable<Notification>, bool, IOrderedEnumerable<Notification>>> SortRules = new()
        {
            [SortNotificationBy.Date] = (q, desc) => desc
                ? q.OrderByDescending(n => n.CreatedAt)
                : q.OrderBy(n => n.CreatedAt),

            [SortNotificationBy.Priority] = (q, desc) => desc
                ? q.OrderByDescending(n => n.Priority)
                : q.OrderBy(n => n.Priority),

            [SortNotificationBy.Title] = (q, desc) => desc
                ? q.OrderByDescending(n => n.Title, StringComparer.OrdinalIgnoreCase)
                : q.OrderBy(n => n.Title, StringComparer.OrdinalIgnoreCase)
        };

    public IEnumerable<Notification> FilterAndSort(
        IEnumerable<Notification> notifications,
        NotificationFilterOptions options)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        ArgumentNullException.ThrowIfNull(options);

        var query = notifications;
        foreach (var makeRule in FilterRules.Values)
        {
            var predicate = makeRule(options);
            if (predicate is not null)
            {
                var captured = predicate;
                query = query.Where(n => captured(n));
            }
        }

        if (options.SortBy is SortNotificationBy sortBy &&
            SortRules.TryGetValue(sortBy, out var sortRule))
        {
            query = sortRule(query, options.Descending);
        }

        return [.. query];
    }
}