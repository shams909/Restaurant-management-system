using BCrypt.Net;
using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
        {
            var user = _mapper.Map<User>(createDto);

            // [SECURITY]: Intercept the raw password and scramble it before saving!
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.PasswordHash);

            // Save to the database
            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UserDto>(user);
        }

    }
}
