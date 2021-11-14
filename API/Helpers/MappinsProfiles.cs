using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappinsProfiles : Profile
    {
        public MappinsProfiles()
        {
           CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=> d.ProudctBrand, o => o.MapFrom
                    (s=> s.ProudctBrand.Name))
                .ForMember(d=> d.ProductType, o => o.MapFrom
                    (s=> s.ProductType.Name))
                .ForMember(d=> d.PictureUrl, o=> o.MapFrom<ProductUrlResolver>());
        }
    }
}