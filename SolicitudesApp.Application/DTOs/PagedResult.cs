namespace SolicitudesApp.Application.DTOs
{
    public record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalItems,
        int PaginaActual,
        int TamanoPagina,
        int TotalPaginas
        )
    {
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
    
}
