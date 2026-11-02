using AutoMapper;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagementSystem.BLL.ViewModels.Trainers;
using GymManagment.DAL.Models;
using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            //Member
            CreateMap<CreateMemberViewModel,Member>();

            CreateMap<Member, MemberViewModel>()
                .ForMember(D => D.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
                .ForMember(D => D.Address, o => o.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} Street - {s.Address.City}"));

            CreateMap<HealthRecord , HealthRecordViewModel>();
            CreateMap<Member , UpdateMemberViewModel>()
                .ForMember(D => D.BuildingNumber, O => O.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(D => D.City, O => O.MapFrom(s => s.Address.City))
                .ForMember(D => D.Street, O => O.MapFrom(s => s.Address.Street));


            //Sessions
            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer.Name))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.Capacity - s.Members.Count));
              

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>();
            CreateMap<UpdateSessionViewModel, Session>();



            //plan
            CreateMap<Plan, PlanViewModel>();


            CreateMap<Plan, UpdatePlanModelView>()
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Name));

 
            CreateMap<UpdatePlanModelView, Plan>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.Ignore());



            //trainer
    
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialty.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString()))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address == null
                    ? string.Empty
                    : $"{s.Address.BuildingNumber} - {s.Address.Street} Street - {s.Address.City}"));

           
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    BuildingNumber = s.BuildingNumber,
                    Street = s.Street,
                    City = s.City
                }));

         
            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialty))
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

      
            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Specialty, o => o.MapFrom(s => s.Specialties))
                .ForMember(d => d.Address, o => o.Ignore());   

        }
    }
}
