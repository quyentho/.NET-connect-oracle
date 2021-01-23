using AutoMapper;
using BL.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using OracleCRUD.Models.Admin;
using OracleCRUD.Models.User;

namespace OracleCRUD
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserIndexViewModel>().ForMember(dest => dest.AccountStatus, opt => opt.MapFrom
                   (src => src.AccountStatus == 1 ? "Open" : "Lock")); 

            CreateMap<ApplicationUser, UserDetailViewModel>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom
                   (src => src.AccountStatus == 1 ? "Open" : "Lock")); 

            CreateMap<ApplicationUser, UserDeleteViewModel>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom
                   (src => src.AccountStatus == 1 ? "Open" : "Lock")); 

            CreateMap<ApplicationUser, UserCreateViewModel>().ReverseMap()
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.ProfileSelected))
                .ForMember(dest => dest.TableSpaceName, opt => opt.MapFrom(src => src.TableSpaceSelected));

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileSelected, opt => opt.MapFrom(src => src.ProfileName))
                .ForMember(dest => dest.TableSpaceSelected, opt => opt.MapFrom(src => src.TableSpaceName))
                .ReverseMap()
              .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.ProfileSelected))
              .ForMember(dest => dest.TableSpaceName, opt => opt.MapFrom(src => src.TableSpaceSelected))
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
          

            CreateMap<BL.Profile.Profile, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<BL.TableSpace.TableSpace, SelectListItem>()
              .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<ApplicationUser, ProfileViewModel>().ReverseMap();
        }
    }
}