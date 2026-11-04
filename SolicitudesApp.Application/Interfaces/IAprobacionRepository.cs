using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Interfaces
{
    public interface IAprobacionRepository : IRepository<Aprobacion>
    {
        Task<Aprobacion?> GetByIdConResponsableAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Aprobacion>> ListarConResponsableAsync(int? solicitudId, CancellationToken ct = default);
    }
}
