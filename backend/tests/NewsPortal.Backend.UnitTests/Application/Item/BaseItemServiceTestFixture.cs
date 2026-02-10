using Moq;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.UnitTests.Application.Item;

public abstract class BaseItemServiceTestFixture
{
    protected Mock<IHackerNewsClient> HackerNewsClient;
    protected Mock<IItemsCacheService> ItemsCacheService;
    protected ItemMapper Mapper;

    protected BaseItemServiceTestFixture()
    {
        Mapper = new ItemMapper();
    }
}