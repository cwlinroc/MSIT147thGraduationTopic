using MSIT147thGraduationTopic.EFModels;

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
        public string Address { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
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
                Address = dto.Address,
                Email = dto.Email,
                Avatar = dto.Avatar,
            };
        }

        static public MemberDto ToDto(this Member ef)
        {
            return new MemberDto
            {
                MemberId = ef.MemberId,
                MemberName = ef.MemberName,
                NickName = ef.NickName,
                DateOfBirth = ef.DateOfBirth,
                Gender = ef.Gender,
                Account = ef.Account,
                Password = ef.Password,
                Phone = ef.Phone,
                Address = ef.Address,
                Email = ef.Email,
                Avatar = ef.Avatar,
            };
        }

        static public void ChangeByDto(this Member ef, MemberDto dto)
        {
            ef.MemberId = dto.MemberId;
            ef.MemberName = dto.MemberName;           
            ef.NickName = dto.NickName;
            ef.DateOfBirth = dto.DateOfBirth;
            ef.Gender = dto.Gender;
            ef.Account = dto.Account;
            ef.Password = dto.Password;
            ef.Phone = dto.Phone;
            ef.Address = dto.Address;
            ef.Email = dto.Email;
            ef.Avatar = dto.Avatar;
        }
    }
}
