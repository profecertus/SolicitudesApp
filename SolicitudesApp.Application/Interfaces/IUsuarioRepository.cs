using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByIdConRolesAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Usuario>> ListarConRolesAsync(CancellationToken ct = default);
        Task<Usuario?> ObtenerPorUsuarioOCorreoAsync(string usuarioOCorreo, CancellationToken ct = default);
    }
}
