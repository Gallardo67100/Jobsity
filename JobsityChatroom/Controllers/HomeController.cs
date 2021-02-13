﻿using JobsityChatroom.Models;
using JobsityChatroom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JobsityChatroom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService messageService;
        private readonly IRabbitMQService rabbitMQService;

        public HomeController(ILogger<HomeController> logger, IMessageService messageService, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            this.messageService = messageService;
            this.rabbitMQService = rabbitMQService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var storedMessages = messageService.GetAll();
            ViewBag.UserName = HttpContext.User.Identity.Name;
            return View(storedMessages);
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(string messageText)
        {
            if (!messageText.StartsWith("/"))
            {
                await messageService.Send(HttpContext.User.Identity.Name, messageText);
            }
            else
            {
                rabbitMQService.SendToQueue(messageText);
            }
            return Json(null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
