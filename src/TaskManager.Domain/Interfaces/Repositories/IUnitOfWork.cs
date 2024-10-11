namespace TaskManager.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    ITaskRepository Tasks { get; }
    IProjectRepository Projects { get; }
    ITaskUpdateHistoryRepository TaskUpdateHistories { get; }

    Task<int> CompleteAsync();
    Task BeginTransactionAsync(); // Método para iniciar transação
    Task CommitAsync(); // Método para confirmar transação
    Task RollbackAsync(); // Método assíncrono para reverter a transação
}