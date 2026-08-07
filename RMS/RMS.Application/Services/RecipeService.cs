using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecipeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _unitOfWork.Repository<Recipe>().GetAllAsync();
            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto createDto)
        {
            var recipe = _mapper.Map<Recipe>(createDto);

            await _unitOfWork.Repository<Recipe>().AddAsync(recipe);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RecipeDto>(recipe);
        }
    }
}
