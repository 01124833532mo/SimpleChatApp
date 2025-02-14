using Microsoft.AspNetCore.SignalR;
using SignalRProject.Context;
using SignalRProject.Models;

namespace ChatDemo.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ChatDbContext context, ILogger<ChatHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Send a message to all clients except the sender
        public async Task Send(string userName, string message)
        {
            try
            {
                // Broadcast the message to all clients (including the sender)
                await Clients.All.SendAsync("ReciveMessage", userName, message);

                // Save the message to the database
                var msg = new Message
                {
                    UserName = userName,
                    Text = message
                };
                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Message sent by {userName}: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                throw; // Re-throw the exception to notify the client
            }
        }

        // Join a group
        public async Task JoinGroup(string groupName, string userName)
        {
            try
            {
                // Add the user to the group
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                // Notify other group members that a new user has joined
                await Clients.OthersInGroup(groupName).SendAsync("NewMemberJoin", userName, groupName);

                _logger.LogInformation($"{userName} joined group {groupName} (ConnectionId: {Context.ConnectionId})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining group");
                throw;
            }
        }

        // Send a message to a specific group
        public async Task SendToGroup(string groupName, string userName, string message)
        {
            try
            {
                // Broadcast the message to all members of the group except the sender
                await Clients.OthersInGroup(groupName).SendAsync("ReciveMessageFromGroup", userName, message);

                // Save the message to the database
                var msg = new Message
                {
                    UserName = userName,
                    Text = message
                };
                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Group message sent by {userName} in {groupName}: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending group message");
                throw;
            }
        }
    }
}