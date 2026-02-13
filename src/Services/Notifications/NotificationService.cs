using pingword.src.DTOs.Notifications;
using pingword.src.Enums.Notifications;
using pingword.src.Interfaces.Generic;
using pingword.src.Interfaces.Notifications;
using pingword.src.Interfaces.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Mappers.Notifications;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;

namespace pingword.src.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _generic;
        private readonly IGenericRepository<Study> _studyGeneric;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudyStateRepository _studyStateRepository;

        public NotificationService(
            IGenericRepository<Notification> generic, 
            IUserRepository userRepository, 
            INotificationRepository notificationRepository,
            IGenericRepository<Study> studyGeneric,
            IStudyStateRepository studyStateRepository)
        {
            _generic = generic;
            _studyGeneric = studyGeneric;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _studyStateRepository = studyStateRepository;
        }

        public async Task<NotificationResponseDto> AddNotificationAsync(string userId, NotificationRequestDto request)
        {
            var user = await _userRepository.GetUserById(userId)
                ?? throw new InvalidOperationException("User already exists.");

            var result  = NotificationMapper.ToEntity(request, user.Language!);

            await _generic.AddAsync(result);

            return NotificationMapper.ToDto(result);

        }

        public async Task<NotificationResponseDto> UpdateNotificationAsync(Guid id, NotificationEnum status)
        {
            var notification = await _notificationRepository.GetNotificationById(id)
                ?? throw new InvalidOperationException("Notification already exists.");

            notification.Action = DateTime.UtcNow;
            notification.NotificationEnum = status;

            await _generic.UpdateAsync(notification);

            var study = GetStudyState(notification.UserId!);


            return NotificationMapper.ToDto(notification);
        }

        private async Task<Study?> GetStudyState(string userId)
        {
            var study = await _studyStateRepository.GetStudyByUserId(userId)
                ?? throw new InvalidOperationException("State already exists.");

            var day = DateTime.UtcNow - study.LastInteraction;

            if (day.TotalDays >= 7)
            {
                // Fazer logica
            }

            return study;
        }
    }
}
