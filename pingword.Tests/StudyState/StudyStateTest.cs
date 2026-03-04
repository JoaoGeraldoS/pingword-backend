using Microsoft.Extensions.Logging;
using Moq;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.StudyState;
using pingword.src.Models.StudyState;
using pingword.src.Services.StudyState;

namespace pingword.Tests.StudyState
{
    public class StudyStateTest
    {

        private readonly Mock<IStudyStateRepository> _studyStateRepository;
        private readonly ILogger<StudyStateService> _logger;

        public StudyStateTest()
        {
            _studyStateRepository = new Mock<IStudyStateRepository>();
            _logger = new Mock<ILogger<StudyStateService>>().Object;
        }

        [Fact(DisplayName = "Register interaction for user success")]
        public async Task Register_interaction_user_success()
        {
            var state = new Study(Guid.NewGuid().ToString());
            var date = DateTime.UtcNow.Date;

            _studyStateRepository.Setup(r => r.GetStudyByUserId(It.IsAny<string>()))
                .ReturnsAsync(state);

            var service = new StudyStateService(_studyStateRepository.Object, _logger);

            await service.RegisterIteractionAsync(state.UserId);

            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.Equal(Status.ACTIVE, state.Status);
            Assert.Equal(date, state.LastInteraction.Date);
        }

        [Fact(DisplayName = "Create interaction for new user success")]
        public async Task Create_interaction_new_user_success()
        {
            var userId = Guid.NewGuid().ToString();
            var date = DateTime.UtcNow.Date;

            _studyStateRepository.Setup(r => r.GetStudyByUserId(It.IsAny<string>()))
                .ReturnsAsync((Study)null);

            var service = new StudyStateService(_studyStateRepository.Object, _logger);
            await service.RegisterIteractionAsync(userId);
            _studyStateRepository.Verify(r => r.AddStateAsync(It.Is<Study>(s => s.UserId == userId && s.LastInteraction.Date == date)), Times.Once);
            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact(DisplayName = "Register interaction for returning user success")]
        public async Task Register_interaction_returning_user_success()
        {
            var state = new Study(Guid.NewGuid().ToString())
            {
                Status = Status.RETURNING,
                DaysInteractedCount = 2,
                LastInteraction = DateTime.UtcNow.AddDays(-1)
            };
            var date = DateTime.UtcNow.Date;

            _studyStateRepository.Setup(r => r.GetStudyByUserId(It.IsAny<string>()))
                .ReturnsAsync(state);

            var service = new StudyStateService(_studyStateRepository.Object, _logger);

            await service.RegisterIteractionAsync(state.UserId);

            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal(Status.ACTIVE, state.Status);
            Assert.Equal(0, state.DaysInteractedCount);
            Assert.Equal(date, state.LastInteraction.Date);
        }

        [Fact(DisplayName = "Register interaction for inactive user success")]
        public async Task Register_interaction_inactive_user_success()
        {
            var state = new Study(Guid.NewGuid().ToString())
            {
                Status = Status.INACTIVE,
                LastInteraction = DateTime.UtcNow.AddDays(-5)
            };
            var date = DateTime.UtcNow.Date;

            _studyStateRepository.Setup(r => r.GetStudyByUserId(It.IsAny<string>()))
                .ReturnsAsync(state);

            var service = new StudyStateService(_studyStateRepository.Object, _logger);

            await service.RegisterIteractionAsync(state.UserId);

            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal(Status.RETURNING, state.Status);
            Assert.Equal(1, state.DaysInteractedCount);
            Assert.Equal(date, state.LastInteraction.Date);
        }

        [Fact(DisplayName = "Process expired states with no expired states")]
        public async Task Process_expired_states_no_expired_states()
        {
            var limitDate = DateTime.UtcNow.AddDays(-7);

            _studyStateRepository.Setup(r => r.GetStateActiveAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Study>());

            var service = new StudyStateService(_studyStateRepository.Object, _logger);

            var result = await service.ProcessExpireStateAsync(limitDate);

            Assert.Equal(0, result);
            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact(DisplayName = "Process expired states with expired states")]
        public async Task Process_expired_states_with_expired_states()
        {
            var limitDate = DateTime.UtcNow.AddDays(-7);
            var expiredStates = new List<Study>
            {
                new Study(Guid.NewGuid().ToString()) { LastInteraction = DateTime.UtcNow.AddDays(-10) },
                new Study(Guid.NewGuid().ToString()) { LastInteraction = DateTime.UtcNow.AddDays(-8) }
            };

            _studyStateRepository.Setup(r => r.GetStateActiveAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(expiredStates);

            var service = new StudyStateService(_studyStateRepository.Object, _logger);
            var result = await service.ProcessExpireStateAsync(limitDate);

            Assert.Equal(2, result);

            foreach (var study in expiredStates)
            {
                Assert.Equal(Status.INACTIVE, study.Status);
            }
            
            _studyStateRepository.Verify(r => r.SaveChangesAsync(), Times.Once);

        }
    }
}
