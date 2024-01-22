using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserPersist _userPersist;
        private readonly IMapper _mapper;

        public AccountService(
                                UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IUserPersist userPersist,
                                IMapper mapper
                                )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userPersist = userPersist;
            _mapper = mapper;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userResult = _mapper.Map<UserUpdateDto>(user);
                    return userResult;
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar usuário. Erro: {ex.Message}");
            }
        }


        public async Task<UserUpdateDto> GetUserByUserNameAsynk(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);
                if (user == null) return null;

                var userReturn = _mapper.Map<UserUpdateDto>(user);
                return userReturn;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName).Result;
                if (user == null) return null;

                userUpdateDto.UserId = user.UserId;

                _mapper.Map(userUpdateDto,user);

                if(userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userPersist.Update<User>(user);

                var resultUpdate = await _userPersist.SaveChangesAsync();

                if (resultUpdate)
                {
                    var userReturn = await _userPersist.GetUserByUserNameAsync(user.UserName);

                    return  _mapper.Map<UserUpdateDto>(userReturn);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower()); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
            }
        }

    }
}
