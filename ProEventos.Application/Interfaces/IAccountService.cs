using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> UserExists(string userName);
        Task<UserUpdateDto> GetUserByUserNameAsynk(string userName);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);
        Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);
        Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userDto);
    }
}
