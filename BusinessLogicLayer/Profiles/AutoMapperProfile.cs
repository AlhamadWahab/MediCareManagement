using AutoMapper;
using BusinessLogicLayer.DTOs.AppointmentDto;
using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using BusinessLogicLayer.ViewModelsDTO;
using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.ViewModels;

namespace BusinessLogicLayer.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<Patient, PatientDTO>().ReverseMap(); ;
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<DP_AppVM, DP_AppVMDTO>().ReverseMap();
            CreateMap<DP_AppVMDTO, Appointment>().ReverseMap();
            CreateMap<DP_AppVM, AppointmentDTO>().ReverseMap();
            CreateMap<DP_AppVMDTO, AppointmentDTO>().ReverseMap();
            CreateMap<DP_AppVM, Appointment>().ReverseMap();
        }
    }
}
