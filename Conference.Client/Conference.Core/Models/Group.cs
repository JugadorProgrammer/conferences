﻿using System.Collections.Concurrent;

namespace Conference.Core.Models
{
    public class Group
    {

        private ConcurrentDictionary<UserId, User> _usersInGroup = [];

        public required int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool TryAddUserToGroup(User user)
            => _usersInGroup.TryAdd(user.Id, user);

        public bool TryUpdateUserStatus(UserId id, ConnectionStatus status)
        {
            if(!_usersInGroup.TryGetValue(id, out User? user) || user is null)
            {
                return false;
            }

            var oldUser = user.Clone(); // TODO: check in real work
            user.Status = status;
            return _usersInGroup.TryUpdate(id, user, oldUser);
        }

        public bool TryRemove(UserId id, out User? user)
            => _usersInGroup.TryRemove(id, out user);

        public ICollection<User> GetAllUsersInGroup()
            => _usersInGroup.Values;
    }
}