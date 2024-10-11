using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskUpdateHistoryRepository(ReadContext readContext, WriteContext writeContext)
        : Repository<TaskUpdateHistory>(writeContext, readContext), ITaskUpdateHistoryRepository
    {
        private readonly ReadContext _readContext = readContext;
        private readonly WriteContext _writeContext = writeContext;
    }
}