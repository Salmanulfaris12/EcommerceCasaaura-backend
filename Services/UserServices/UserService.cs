using AutoMapper;
using CasaAura.Models.UserModels.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CasaAura.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<UserViewDTO>> GetUsers()
        {
            try
            {
                var u = await _context.Users.Where(u=>u.Role=="user").ToListAsync();
                return _mapper.Map<List<UserViewDTO>>(u);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserViewDTO> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return null;
                }
                return _mapper.Map<UserViewDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BlockUnblockRes> BlockAndUnBlock(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                user.IsBlocked = !user.IsBlocked;
                await _context.SaveChangesAsync();

                return new BlockUnblockRes
                {
                    IsBlocked = user.IsBlocked == true ? true : false,
                    Message = user.IsBlocked == true ? "User is blocked " : "User is unBlocked"
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
