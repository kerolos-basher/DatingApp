using AutoMapper;
using DAtingApp.Extentions;
using DAtingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Photo, PhotoToReturn>();
            CreateMap<photoForCreationDto,Photo>();
            CreateMap<User, UserForDetailDto>()
                .ForMember(des => des.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(d => d.DateOfBirth.calculateage());

                });
            CreateMap<User, UserForListDto>()
                .ForMember(des => des.PhotoUrl , opt=>{
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(d => d.DateOfBirth.calculateage());

                });
            CreateMap<Photo, photoForDetaildDto>();
        }

       
    }
}
