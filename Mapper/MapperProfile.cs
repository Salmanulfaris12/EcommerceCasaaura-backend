﻿using AutoMapper;
using CasaAura.Models.AddressModels;
using CasaAura.Models.AddressModels.AddresDTOs;
using CasaAura.Models.CategoryModels;
using CasaAura.Models.CategoryModels.CategoryDTOs;
using CasaAura.Models.ProductModels;
using CasaAura.Models.ProductModels.ProductDTOs;
using CasaAura.Models.UserModels;
using CasaAura.Models.UserModels.DTOs;
using CasaAura.Models.WishListModels;
using CasaAura.Models.WishListModels.WishListDTOs;

namespace CasaAura.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile() { 
            CreateMap<User,UserRegisterDTO>().ReverseMap();
            CreateMap<Category,CategoryResDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<Product, AddProductDTO>().ReverseMap();
            CreateMap<WishList,WishListDTO>().ReverseMap();
            CreateMap<User,UserViewDTO>().ReverseMap();
            CreateMap<Address,AddressResDTO>().ReverseMap();
        }
    }
}
