using pingword.src.Interfaces.StudyState;
using pingword.src.Models.StudyState;
using Serilog;

namespace pingword.src.Services.StudyState
{
    public class StudyStateService : IStudyStateService
    { 
        private readonly IStudyStateRepository _studyStateRepository;
        private readonly ILogger<StudyStateService> _logger; 

        public StudyStateService(IStudyStateRepository studyStateRepository, ILogger<StudyStateService> logger)
        {
            _studyStateRepository = studyStateRepository;
            _logger = logger;
        }

        public async Task<int> ProcessExpireStateAsync(DateTime limitDate)
        {
            Log.Information("Processing expired study states with limit date {LimitDate}", limitDate);

            var expiredStates = await _studyStateRepository.GetStateActiveAsync(limitDate);
            if (expiredStates == null || !expiredStates.Any())
            {
                return 0;
            }

            foreach (var study in expiredStates)
            {
                study.MarkAsInactive();
            }

            await _studyStateRepository.SaveChangesAsync();

            return expiredStates.Count;
        }

        public async Task RegisterIteractionAsync(string userId)
        {
            _logger.LogInformation("Registering interaction for user {UserId}", userId);


            var study = await _studyStateRepository.GetStudyByUserId(userId);

            if (study == null)
            {
                study = new Study(userId);
                await _studyStateRepository.AddStateAsync(study);
                return;
            }

            study.RegisterInteraction();
            await _studyStateRepository.SaveChangesAsync();
        }
    }
}
