using GQ.WebApi.Entities;

namespace GQ.WebApi.Tests.Entities;

public sealed class ExampleItemTests
{
    [Fact]
    public void Create_WithValidName_ReturnsEntity()
    {
        var item = ExampleItem.Create("test");

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal("test", item.Name);
    }

    [Fact]
    public void Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => ExampleItem.Create("  "));
    }

    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        var item = ExampleItem.Create("original");
        item.Rename("updated");
        Assert.Equal("updated", item.Name);
    }

    [Fact]
    public void Rename_WithEmptyName_Throws()
    {
        var item = ExampleItem.Create("original");
        Assert.Throws<ArgumentException>(() => item.Rename("  "));
    }
}
