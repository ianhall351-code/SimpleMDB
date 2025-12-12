using Shared.Http;
using Smdb.Core.Db;
using Smdb.Core.Movies;
using Xunit;

namespace Smdb.Core.Test;

public class MovieServiceTests
{
    private IMovieService CreateService()
    {
        var db = new MemoryDatabase();
        var repo = new MemoryMovieRepository(db);
        return new DefaultMovieService(repo);
    }

    [Fact]
    public async Task ReadMovies_ValidPageAndSize_ReturnsPagedResult()
    {
        var service = CreateService();
        var result = await service.ReadMovies(1, 10);
        Assert.False(result.IsError);
        Assert.NotNull(result.Payload);
        Assert.Equal(50, result.Payload.TotalCount); // Seeded with 50 movies
    }

    [Fact]
    public async Task ReadMovies_InvalidPage_ReturnsBadRequest()
    {
        var service = CreateService();
        var result = await service.ReadMovies(0, 10);
        Assert.True(result.IsError);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task CreateMovie_ValidMovie_ReturnsCreatedMovie()
    {
        var service = CreateService();
        var movie = new Movie(0, "Test Movie", 2023, "Description");
        var result = await service.CreateMovie(movie);
        Assert.False(result.IsError);
        Assert.NotNull(result.Payload);
        Assert.Equal("Test Movie", result.Payload.Title);
    }

    [Fact]
    public async Task CreateMovie_InvalidTitle_ReturnsBadRequest()
    {
        var service = CreateService();
        var movie = new Movie(0, "", 2023, "Description");
        var result = await service.CreateMovie(movie);
        Assert.True(result.IsError);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task ReadMovie_ExistingId_ReturnsMovie()
    {
        var service = CreateService();
        var movie = new Movie(0, "Test Movie", 2023, "Description");
        var createResult = await service.CreateMovie(movie);
        var readResult = await service.ReadMovie(createResult.Payload!.Id);
        Assert.False(readResult.IsError);
        Assert.Equal("Test Movie", readResult.Payload!.Title);
    }

    [Fact]
    public async Task ReadMovie_NonExistingId_ReturnsNotFound()
    {
        var service = CreateService();
        var result = await service.ReadMovie(999);
        Assert.True(result.IsError);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_ValidData_ReturnsUpdatedMovie()
    {
        var service = CreateService();
        var movie = new Movie(0, "Test Movie", 2023, "Description");
        var createResult = await service.CreateMovie(movie);
        var updatedMovie = new Movie(0, "Updated Title", 2024, "Updated Description");
        var updateResult = await service.UpdateMovie(createResult.Payload!.Id, updatedMovie);
        Assert.False(updateResult.IsError);
        Assert.Equal("Updated Title", updateResult.Payload!.Title);
    }

    [Fact]
    public async Task DeleteMovie_ExistingId_ReturnsDeletedMovie()
    {
        var service = CreateService();
        var movie = new Movie(0, "Test Movie", 2023, "Description");
        var createResult = await service.CreateMovie(movie);
        var deleteResult = await service.DeleteMovie(createResult.Payload!.Id);
        Assert.False(deleteResult.IsError);
        Assert.Equal("Test Movie", deleteResult.Payload!.Title);
    }
}
