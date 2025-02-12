using AutoMapper;
using Repara.DTO;
using Repara.DTO.Peca;
using Repara.Model;

namespace Repara.Helpers.Mappers
{
    public class PecaProfile : Profile
    {
        private const string DateOnlyFormat = "dd-MM-yyyy";
        private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

        public PecaProfile()
        {

            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));

            CreateMap<Peca, PecaDTO>()
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn.ToString(DateTimeFormat)))
                .ForMember(c => c.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn.ToString(DateTimeFormat)));


            CreateMap<PecaCreateDTO, Peca>();


            CreateMap<PecaUpdateDTO, Peca>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                {

                    if (srcMember == null) return false;


                    if (srcMember.GetType().IsValueType)
                    {
                        var defaultValue = Activator.CreateInstance(srcMember.GetType());
                        return !srcMember.Equals(defaultValue); // Só mapear se não for o valor padrão
                    }
                    return true;
                }));
        }
    }
}
