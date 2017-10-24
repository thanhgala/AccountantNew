using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Service
{
    public interface IApplicationUserService
    {
        IEnumerable<ApplicationUser> GetAll();
        IEnumerable<ApplicationUser> GetUser(string keyword,int page,int pageSize,out int totalRow);
        IEnumerable<ApplicationUser> GetAdmin(string keyword, int page, int pageSize, out int totalRow);
        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupID);
    }

    class ApplicationUserService : IApplicationUserService
    {
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _appUserRepository;
        private IApplicationGroupRepository _appGroupRepository;

        public ApplicationUserService(IUnitOfWork unitOfWork,
           IApplicationUserRepository appUserRepository,
           IApplicationGroupRepository appGroupRepository)
        {
            this._unitOfWork = unitOfWork;
            this._appUserRepository = appUserRepository;
            this._appGroupRepository = appGroupRepository;
        }

        public IEnumerable<ApplicationUser> GetUser(string keyword, int page, int pageSize, out int totalRow)
        {
            var lst = _appUserRepository.GetListUserByName("User").ToList();
            List<ApplicationUser> model = new List<ApplicationUser>();
            for (int i = 0; i < lst.Count(); i++)
            {
                if (!model.Contains(lst[i]))
                {
                    model.Add(lst[i]);
                }
            }
            if (string.IsNullOrEmpty(keyword))
            {
                totalRow = model.Count();
                return model.OrderBy(x => x.NamePCA).Skip(page * pageSize).Take(pageSize);
            }
            else
            {
                model = model.Where(x => x.FullName.ToLower().Contains(keyword.ToLower())
                    || x.NamePCA.ToLower().Contains(keyword.ToLower())
                    || x.Department.ToLower().Contains(keyword.ToLower())).ToList();
                totalRow = model.Count();
                return model.OrderBy(x => x.FullName).Skip(page * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<ApplicationUser> GetAdmin(string keyword, int page, int pageSize, out int totalRow)
        {
            var model = _appUserRepository.GetAll().Except(_appUserRepository.GetListUserByName("User")).Where(x => x.Id != "user12");
            if (string.IsNullOrEmpty(keyword))
            {
                totalRow = model.Count();
                return model.OrderBy(x => x.NamePCA).Skip(page * pageSize).Take(pageSize);
            }
            else
            {
                model = model.Where(x => x.FullName.ToLower().Contains(keyword.ToLower())
                    || x.NamePCA.ToLower().Contains(keyword.ToLower())
                    || x.Department.ToLower().Contains(keyword.ToLower()));
                totalRow = model.Count();
                return model.OrderBy(x => x.FullName).Skip(page * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupID)
        {
            return _appUserRepository.GetListUserByGroupId(groupID);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _appUserRepository.GetAll();
        }
    }
}
