using AutoMapper;
using CasaAura.Models.AddressModels;
using CasaAura.Models.AddressModels.AddresDTOs;
using Microsoft.EntityFrameworkCore;

namespace CasaAura.Services.AddressServices
{
    public class AddressService:IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAddress(int userId, AddressCreateDTO newAddress)
        {
            try
            {
                if(userId == null)
                {
                    throw new Exception("userId is not valid");
                }
                if(newAddress == null)
                {
                    throw new Exception("Address cannot be null");
                }
                var userAddress = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
                if(userAddress.Count>5)
                {
                    throw new Exception("Maximum limit of Address reached");
                }

                var address = new Address
                {
                    UserId = userId,
                    FullName = newAddress.FullName,
                    PhoneNumber = newAddress.PhoneNumber,
                    HouseName = newAddress.HouseName,
                    Place = newAddress.Place,
                    PostOffice = newAddress.PostOffice,
                    Pincode = newAddress.Pincode,
                    LandMark = newAddress.LandMark,

                };
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();  
                return true;    

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AddressResDTO>>GetAddresses(int userId)
        {
            try
            {
                if (userId == null)
                {
                    throw new Exception("userId not valid");
                }
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var addresses = await _context.Addresses
                    .Where(u => u.UserId == userId)
                    .ToListAsync();
                if (addresses != null)
                {
                    return _mapper.Map<List<AddressResDTO>>(addresses);
                }
                return new List<AddressResDTO>();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool>RemoveAddress(int userId ,int addressId)
        {
            try
            {
                if (userId == null)
                {
                    throw new Exception("userId not found");
                }
                if(addressId == null)
                {
                    throw new Exception("AddressID cannot be null");
                }
                var address = await _context.Addresses.FirstOrDefaultAsync(u => u.UserId == userId & u.AddressId == addressId);
                if(address != null)
                {
                     _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
