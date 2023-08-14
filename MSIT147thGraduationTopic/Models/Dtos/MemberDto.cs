using Humanizer;
using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Salt { get; set; }
        public bool IsActivated { get; set; }
        public string ConfirmGuid { get; set; }
    }

    public class MemberEditDto
    {
        public string? NickName { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public bool IsActivated { get; set; }
        public string? ConfirmGuid { get; set; }
    }

    public class MemberSearchDto
    {
        public int? MemberID { get; set; }
        public string? MemberName { get; set; }
        public int? MaxQueryNumber { get; set; }
    }

    static public class MemberTransfer
    {
        static public Member ToEF(this MemberDto dto)
        {
            return new Member
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                NickName = dto.NickName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Account = dto.Account,
                Password = dto.Password,
                Phone = dto.Phone,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Email = dto.Email,
                Avatar = dto.Avatar,
                Salt = dto.Salt,
                ConfirmGuid = dto.ConfirmGuid,
                IsActivated = dto.IsActivated,
            };
        }

        static public MemberDto ToDto(this Member entity)
        {
            return new MemberDto
            {
                MemberId = entity.MemberId,
                MemberName = entity.MemberName,
                NickName = entity.NickName,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                City = entity.City,
                District = entity.District,
                Account = entity.Account,
                Password = entity.Password,
                Phone = entity.Phone,
                Address = entity.Address,
                Email = entity.Email,
                Avatar = entity.Avatar,
                Salt = entity.Salt,
                ConfirmGuid = entity.ConfirmGuid,
                IsActivated = entity.IsActivated,
            };
        }

        static public void ChangeByEditDto(this Member entity, MemberEditDto dto)
        {
            entity.NickName = dto.NickName;
            if (dto.Account != null) entity.Account = dto.Account;
            if (dto.Password != null) entity.Password = dto.Password;
            if (dto.Phone != null) entity.Phone = dto.Phone;
            if (dto.City != null) entity.City = dto.City;
            if (dto.District != null) entity.District = dto.District;
            if (dto.Address != null) entity.Address = dto.Address;
            if (dto.Email != null) entity.Email = dto.Email;
            entity.Avatar = dto.Avatar;
            entity.IsActivated = dto.IsActivated;
        }
    }
}
