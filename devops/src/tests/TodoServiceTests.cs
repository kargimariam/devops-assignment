using devops.src.TodoApi.Services;
using Xunit;

namespace devops.src.tests
{
    public class TodoServiceTests
    {
        private readonly TodoService _service = new();

        // ── CREATE ──────────────────────────────────────────────────────────────

        [Fact]
        public void Create_ValidTitle_ReturnsTodoWithCorrectTitle()
        {
            var item = _service.Create("Buy groceries");

            Assert.Equal("Buy groceries", item.Title);
        }

        [Fact]
        public void Create_ValidTitle_AssignsIncrementingId()
        {
            var first = _service.Create("Task 1");
            var second = _service.Create("Task 2");

            Assert.Equal(1, first.Id);
            Assert.Equal(2, second.Id);
        }

        [Fact]
        public void Create_ValidTitle_IsNotCompleted()
        {
            var item = _service.Create("New task");

            Assert.False(item.IsCompleted);
        }

        [Fact]
        public void Create_EmptyTitle_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.Create(""));
        }

        [Fact]
        public void Create_WhitespaceTitle_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.Create("   "));
        }

        [Fact]
        public void Create_TitleWithLeadingSpaces_TrimsTitleBeforeSaving()
        {
            var item = _service.Create("  Clean me  ");

            Assert.Equal("Clean me", item.Title);
        }

        // ── GET ALL ─────────────────────────────────────────────────────────────

        [Fact]
        public void GetAll_NoTodos_ReturnsEmptyList()
        {
            var result = _service.GetAll();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_AfterCreatingTwo_ReturnsBothItems()
        {
            _service.Create("First");
            _service.Create("Second");

            var result = _service.GetAll();

            Assert.Equal(2, result.Count);
        }

        // ── GET BY ID ───────────────────────────────────────────────────────────

        [Fact]
        public void GetById_ExistingId_ReturnsCorrectItem()
        {
            var created = _service.Create("Find me");

            var found = _service.GetById(created.Id);

            Assert.NotNull(found);
            Assert.Equal("Find me", found.Title);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }

        // ── COMPLETE ────────────────────────────────────────────────────────────

        [Fact]
        public void Complete_ExistingId_MarksItemAsCompleted()
        {
            var item = _service.Create("Finish me");

            _service.Complete(item.Id);

            Assert.True(_service.GetById(item.Id)!.IsCompleted);
        }

        [Fact]
        public void Complete_NonExistingId_ReturnsNull()
        {
            var result = _service.Complete(999);

            Assert.Null(result);
        }

        // ── DELETE ──────────────────────────────────────────────────────────────

        [Fact]
        public void Delete_ExistingId_ReturnsTrueAndRemovesItem()
        {
            var item = _service.Create("Delete me");

            var result = _service.Delete(item.Id);

            Assert.True(result);
            Assert.Empty(_service.GetAll());
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsFalse()
        {
            var result = _service.Delete(999);

            Assert.False(result);
        }
    }
}
