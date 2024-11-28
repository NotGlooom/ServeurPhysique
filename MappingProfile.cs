using AutoMapper;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.DTOs;
using ProjetSynthese.Server.Models.Question;
using ProjetSynthese.Server.Models.Reponse;

namespace ProjetSynthese.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mappe Activite vers ActiviteDto
            CreateMap<Activite, ActiviteDto>();

            // Mappe Exercice vers ExerciceDto
            CreateMap<Exercice, ExerciceDto>();

            // Mappe Variable vers VariableDto
            CreateMap<Variable, VariableDto>();

            // Mappe QuestionBase vers QuestionDto et inclut les classes dérivées
            CreateMap<QuestionBase, QuestionDto>()
                .IncludeAllDerived();

            // Mappe Indice vers IndiceDto
            CreateMap<Indice, IndiceDto>();

            // Mappe GroupeCours vers GroupeCoursDto
            CreateMap<GroupeCours, GroupeCoursDto>();

            // Mappe ExcludeRange vers ExcludeRangeDto
            CreateMap<ExcludeRange, ExcludeRangeDto>();

            // Map Enseignant to EnseignantDto, including mapped IdentityUser properties
            CreateMap<Enseignant, EnseignantDto>()
                .ForMember(dest => dest.Utilisateur, opt => opt.MapFrom(src => src.utilisateur));
        }

        // Méthode d'aide pour obtenir le type de réponse sous forme de chaîne de caractères
        private static string GetReponseType(ReponseBase reponse)
        {
            return reponse switch
            {
                ReponseChoix => "choix",
                ReponseDeroulante => "deroulante",
                ReponseNumerique => "numerique",
                ReponseTroue => "troue",
                _ => "inconnu"
            };
        }
    }
}
