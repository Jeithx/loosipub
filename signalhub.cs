using Microsoft.AspNetCore.SignalR;

namespace API
{
    public class SignalHub : Hub
    {
        
        private static Dictionary<string, HashSet<string>> rooms = new();

        public async Task JoinRoom(string room)
        {
            if (!rooms.ContainsKey(room))
                rooms[room] = new HashSet<string>();

            rooms[room].Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            
            await Clients.OthersInGroup(room).SendAsync("NewPeer", Context.ConnectionId);
        }

        public async Task SendSignal(string room, string targetConnectionId, string type, string data)
        {
            if (string.IsNullOrEmpty(targetConnectionId))
            {
                
                await Clients.OthersInGroup(room).SendAsync("Signal", Context.ConnectionId, type, data);
            }
            else
            {
                await Clients.Client(targetConnectionId).SendAsync("Signal", Context.ConnectionId, type, data);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var room in rooms)
            {
                if (room.Value.Remove(Context.ConnectionId))
                {
                    await Clients.Group(room.Key).SendAsync("PeerDisconnected", Context.ConnectionId);
                    if (room.Value.Count == 0)
                        rooms.Remove(room.Key);
                    break;
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
