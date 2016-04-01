using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Admin;
using Wams.DAL.Interfaces;
using Wams.Interfaces;
using Wams.ViewModels;
using Wams.ViewModels.Admin;

namespace Wams.BusinessLogic
{
    public class AdminLogic : IAdminLogic
    {
        private readonly IAdminRepository adminRepository;

        public AdminLogic(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public BaseResponse CreateAdmin(RegisterAdminRequest request)
        {
            var row = this.adminRepository.CreateAdmin(new CreateAdmin
            {
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = request.Role
            });

            return row == 1
                ? new BaseResponse { Success = true}
                : new BaseResponse();

        }

        public List<AdminUser> GetAllAdmins()
        {
            var users = this.adminRepository.GetAllAdmins();

            return
                users.Select(
                    x =>
                        new AdminUser
                        {
                            Id = x.AccountId,
                            EmailAddress = x.EmailAddress,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Role = x.LoginRole
                        }).ToList();
        }

        public AdminUser GetAdmin(int id)
        {
            var user = this.adminRepository.GetAdmin(id);

            return user == null ? null : new AdminUser
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = id,
                Role = user.LoginRole,
                Password = user.Password
            };
        }

        public BaseResponse EditAdmin(EditAdminRequest request)
        {
            var row = this.adminRepository.EditAdmin(new EditAdmin
            {
                Id = request.Id,
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = request.Role
            });

            return row == 1
                ? new BaseResponse { Success = true }
                : new BaseResponse();
        }

        public BaseResponse DeleteAdmin(int id)
        {
            var row = this.adminRepository.DeleteAdmin(id);

            return row == 1
                ? new BaseResponse { Success = true }
                : new BaseResponse();
        }
    }
}
