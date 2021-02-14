using Microsoft.Extensions.Logging;
using JobsityChatroom.Controllers;
using JobsityChatroom.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobsityChatroom.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public async void SendMessageWhenHandlingCommandCorrectlySendsToRabbitMQ()
        {
            // Arrange
            var message = "/stock=appl.us";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            var rabbitMQService = new Mock<IRabbitMQService>();
            rabbitMQService.Setup(serv => serv.SendToQueue(It.IsAny<string>()));

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);

            // Act
            await controller.SendMessage(message);

            // Assert
            rabbitMQService.Verify(mock => mock.SendToQueue(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async void SendMessageWhenHandlingCommandCorrectlyReturnsOk()
        {
            // Arrange
            var message = "/stock=appl.us";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            var rabbitMQService = new Mock<IRabbitMQService>();

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);

            // Act
            var response = (StatusCodeResult)await controller.SendMessage(message);

            // Assert
            Assert.True(response.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public async void SendMessageWhenHandlingCommandWithErrorReturns500()
        {
            // Arrange
            var message = "/stock=%%%%";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            var rabbitMQService = new Mock<IRabbitMQService>();
            rabbitMQService.Setup(serv => serv.SendToQueue(It.IsAny<string>()))
                            .Throws(new Exception());

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);

            // Act
            var response = (StatusCodeResult)await controller.SendMessage(message);

            // Assert
            Assert.True(response.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void SendMessageWhenHandlingMessageCorrectlySendsToMessageService()
        {
            // Arrange
            var message = "Hello world!";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            messageService.Setup(serv => serv.Send(It.IsAny<string>(), It.IsAny<string>()));
            var rabbitMQService = new Mock<IRabbitMQService>();

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            await controller.SendMessage(message);

            // Assert
            messageService.Verify(mock => mock.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public async void SendMessageWhenHandlingMessageCorrectlyReturnsOk()
        {
            // Arrange
            var message = "Hello world!";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            var rabbitMQService = new Mock<IRabbitMQService>();

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var response = (StatusCodeResult)await controller.SendMessage(message);

            // Assert
            Assert.True(response.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public async void SendMessageWhenHandlingMessageWithErrorReturns500()
        {
            // Arrange
            var message = "Hello world!";
            var logger = new Mock<ILogger<HomeController>>();
            var messageService = new Mock<IMessageService>();
            messageService.Setup(serv => serv.Send(It.IsAny<string>(), It.IsAny<string>()))
                            .Throws(new Exception());

            var rabbitMQService = new Mock<IRabbitMQService>();

            var controller = new HomeController(logger.Object, messageService.Object, rabbitMQService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var response = (StatusCodeResult)await controller.SendMessage(message);

            // Assert
            Assert.True(response.StatusCode == (int)HttpStatusCode.InternalServerError);
        }
    }
}
