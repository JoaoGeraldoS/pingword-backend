using pingword.src.DTOs.Notifications;
using pingword.src.Enums.Notifications;
using pingword.src.Interfaces.Notifications;
using pingword.src.Interfaces.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Mappers.Notifications;

namespace pingword.src.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudyStateService _studyStateService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IUserRepository userRepository, 
            INotificationRepository notificationRepository,
            IStudyStateService studyStateService,
            ILogger<NotificationService> logger
           )
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _studyStateService = studyStateService;
            _logger = logger;
        }

        public async Task<NotificationResponseDto> AddNotificationAsync(string userId, NotificationRequestDto request)
        {
            _logger.LogInformation("Adding notification for user {UserId} with word {Word}", userId, request.Word);

            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                _logger.LogWarning("DEBUG: Usuário {Id} NÃO ENCONTRADO no banco. O banco pode ter resetado.", userId);
                throw new KeyNotFoundException($"User {userId} not found in database.");
            }

            var userNotification = new UserNotificationDto
            {
                Language = user.Language!,
                UserId = user.Id
            };

            var result  = NotificationMapper.ToEntity(request, userNotification);

            await _notificationRepository.AddNotificationAsync(result);

            return NotificationMapper.ToDto(result);
        }

        public async Task<NotificationResponseDto> UpdateNotificationAsync(Guid id, NotificationEnum status)
        {
            _logger.LogInformation("Updating notification {NotificationId} with status {Status}", id, status);


            var notification = await _notificationRepository.GetNotificationById(id)
                ?? throw new KeyNotFoundException("Notification already exists.");

            notification.Action = DateTime.UtcNow;
            notification.NotificationEnum = status;

            await _notificationRepository.UpdateNotificationAsync(notification);

            await _studyStateService.RegisterIteractionAsync(notification.UserId!);
               
            return NotificationMapper.ToDto(notification);
        }
    }
}
