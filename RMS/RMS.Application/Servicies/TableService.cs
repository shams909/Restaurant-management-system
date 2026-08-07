using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TableDto>> GetAllTablesAsync()
        {
            var tables = await _unitOfWork.Repository<Table>().GetAllAsync();
            return _mapper.Map<IEnumerable<TableDto>>(tables);
        }

        public async Task<TableDto> CreateTableAsync(CreateTableDto createDto)
        {
            var table = _mapper.Map<Table>(createDto);

            await _unitOfWork.Repository<Table>().AddAsync(table);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<TableDto>(table);
        }
    }
}
