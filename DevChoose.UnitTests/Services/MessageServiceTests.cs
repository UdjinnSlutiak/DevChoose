using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Implementations;
using Moq;
using Xunit;

namespace DevChoose.UnitTests.Services
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task Can_Get_Messages()
        {
            var offset = 0;
            var count = 5;
            var testMessages = this.GetTestMessages(count);
            var mock = new Mock<IRepository<Message>>();
            mock.Setup(repo => repo.GetAsync(offset, count)).ReturnsAsync(testMessages);
            var service = new MessageService(messageRepository: mock.Object, null, null);

            var result = await service.GetAsync(offset, count, dialogId: 1);

            Assert.NotNull(result);
        }

        private IEnumerable<Message> GetTestMessages(int count = 5)
        {
            var messages = new List<Message>();

            for (int i = 1; i <= count; i++)
            {
                messages.Add(new Message
                {
                    Id = i,
                    DialogId = 1,
                    SenderId = 1,
                    Text = $"Message #{i}"
                });
            }

            return messages.AsEnumerable();
        }
    }
}
