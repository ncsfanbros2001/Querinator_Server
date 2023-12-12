using AutoMapper;
using Models.Entity;

namespace Application.HelperMethods
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
