using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Domain.Models.Items;
using Riok.Mapperly.Abstractions;

namespace NewsPortal.Backend.Application.Mappers;

[Mapper]
public partial class ItemMapper
{
    // HackerNewsApi to DTO
    public partial T MapToItemDto<T>(Infrastructure.Http.HackerNews.Models.Contracts.Item item);

    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Deleted))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.By))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Dead))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Parent))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Poll))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Kids))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Score))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Parts))]
    [MapperIgnoreSource(nameof(Infrastructure.Http.HackerNews.Models.Contracts.Item.Descendants))]
    private partial StoryDto MapToStoryDto(Infrastructure.Http.HackerNews.Models.Contracts.Item item);

    // Domain to DTO
    public partial T MapToItemDto<T>(Domain.Models.Items.Item item);
    private partial StoryDto MapToStoryDto(Domain.Models.Items.Item item);

    // DTO to Domain
    public partial T MapToDomainItem<T>(ItemDto itemDto);
    private partial Story MapToDomainItem(ItemDto itemDto);
}