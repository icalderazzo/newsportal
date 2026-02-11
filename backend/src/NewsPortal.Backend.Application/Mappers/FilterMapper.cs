using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Domain.Filters;
using Riok.Mapperly.Abstractions;

namespace NewsPortal.Backend.Application.Mappers;

[Mapper]
public partial class FilterMapper
{
    // DTO to Domain
    public partial DbPaginationFilter? MapToDomain(PaginationFilter? filter);
}