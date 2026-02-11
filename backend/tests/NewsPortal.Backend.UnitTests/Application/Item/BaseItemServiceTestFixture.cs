using Moq;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Mappers;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Domain.Repositories;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;
using ItemMapper = NewsPortal.Backend.Application.Mappers.ItemMapper;

namespace NewsPortal.Backend.UnitTests.Application.Item;

public abstract class BaseItemServiceTestFixture
{
    protected Mock<IHackerNewsClient> HackerNewsClient;
    protected Mock<IItemsRepository> ItemsRepository;
    protected Mock<IItemsCacheService> ItemsCacheService;
    protected ItemMapper ItemMapper;
    protected FilterMapper FilterMapper;
    

    protected BaseItemServiceTestFixture()
    {
        ItemMapper = new ItemMapper();
        FilterMapper = new FilterMapper();
    }
}