using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class UserPersist : GeralPersistence, IUserPersist
    {
        private readonly ProEventoContext _context;

        public UserPersist(ProEventoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user =  await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName);

            if (user.UserId ==0)
            {
                user.UserId = user.Id;
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetUserAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
