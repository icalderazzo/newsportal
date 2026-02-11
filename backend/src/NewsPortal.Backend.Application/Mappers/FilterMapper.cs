using Riok.Mapperly.Abstractions;

namespace NewsPortal.Backend.Application.Mappers;

[Mapper]
public partial class FilterMapper
{
    // DTO to Domain
    public partial Domain.Filters.DbPaginationFilter? MapToDomain(Contracts.Filters.PaginationFilter? filter);
}