using AutoMapper;
using Moq;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.UnitTests.Application.Item;

public abstract class BaseItemServiceTestFixture
{
    protected Mock<IHackerNewsClient> HackerNewsClient;
    protected Mock<IItemsCacheService> ItemsCacheService;
    protected IMapper Mapper;

    protected BaseItemServiceTestFixture()
    {
        Mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Infrastructure.Http.HackerNews.Models.Contracts.Item, StoryDto>();
        }));
    }
}