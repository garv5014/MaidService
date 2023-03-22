using Maid.Library.Interfaces;
using MaidService.ViewModels;
using Moq;

namespace ViewModel.Test
{
    public class MainPageTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void nothing()
        {
            var mockSupabase = new Mock<ISupabaseService>();
            new MainPageViewModel(mockSupabase.Object);
        }
    }
}