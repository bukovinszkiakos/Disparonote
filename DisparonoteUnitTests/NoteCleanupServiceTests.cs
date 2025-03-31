using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class NoteCleanupServiceTests
{
    [Test]
    public async Task ExecuteAsync_CallsDeleteExpiredNotesAsync_AndLogsInfo()
    {
        var mockNoteRepo = new Mock<INoteRepository>();
        var mockLogger = new Mock<ILogger<NoteCleanupService>>();

        var mockScope = new Mock<IServiceScope>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider
            .Setup(sp => sp.GetService(typeof(INoteRepository)))
            .Returns(mockNoteRepo.Object);
        mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);

        var rootProvider = new Mock<IServiceProvider>();
        rootProvider
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(mockScopeFactory.Object);

        var service = new NoteCleanupService(rootProvider.Object, mockLogger.Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await service.StartAsync(cts.Token);

        mockNoteRepo.Verify(r => r.DeleteExpiredNotesAsync(), Times.AtLeastOnce);
        mockLogger.Verify(
            log => log.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Expired notes cleanup executed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
    }
}
