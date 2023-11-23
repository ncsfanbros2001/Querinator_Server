using AutoMapper;
using JWT_Demo.Models.Entity;
using Models.Entity;

namespace JWT_Demo.HelperMethods
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<QueryToSave, QueryToSave>();

            CreateMap<AppUser, AppUser>();
        }
    }
}
