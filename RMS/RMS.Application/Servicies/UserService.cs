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
            // The Chef receives the password and maps it to the real User entity
            var user = _mapper.Map<User>(createDto);

            // Hand it to the Pantry to save to the database!
            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveAsync();

            // When returning the data, AutoMapper automatically rips the password out!
            return _mapper.Map<UserDto>(user);
        }
    }
}
