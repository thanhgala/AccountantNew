using AccountantNew.Common;
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
    public interface IApplicationRoleService
    {
        ApplicationRole GetDetail(string id);

        IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<ApplicationRole> GetAll();

        ApplicationRole Add(ApplicationRole appRole);

        void Update(ApplicationRole appRole);

        void Delete(string id);

        //Add roles to a sepcify group
        bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId);

        //Get list role by group id
        IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId);

        IEnumerable<ApplicationRole> GetListRoleByRoleGroupId(int groupId,string roleId);

        ApplicationRoleGroup GetRoleGroup(int groupId, string roleId);

        void Save();
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private IApplicationRoleRepository _appRoleRepository;
        private IApplicationRoleGroupRepository _appRoleGroupRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationRoleService(IUnitOfWork unitOfWork,
           IApplicationRoleRepository appRoleRepository, IApplicationRoleGroupRepository appRoleGroupRepository)
        {
            this._appRoleRepository = appRoleRepository;
            this._appRoleGroupRepository = appRoleGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public ApplicationRole GetDetail(string id)
        {
            return _appRoleRepository.GetSingleByCondition(x => x.Id == id);
        }

        public IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appRoleRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Description.Contains(filter));
            }
            totalRow = query.Count();
            return query.OrderBy(x => x.Description).Skip(page * pageSize).Take(pageSize);
        }

        public IEnumerable<ApplicationRole> GetAll()
        {
            return _appRoleRepository.GetAll();
        }

        public ApplicationRole Add(ApplicationRole appRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Name == appRole.Name))
            {
                throw new NameDuplicatedException("Tên không được trùng");
            }
            return _appRoleRepository.Add(appRole);
        }

        public void Update(ApplicationRole appRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Description == appRole.Description && x.Id != appRole.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            _appRoleRepository.Update(appRole);
        }

        public void Delete(string id)
        {
            var appRole = _appRoleRepository.GetSingleByCondition(x => x.Id == id);
            _appRoleRepository.Delete(appRole);
        }

        public bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var role in roleGroups)
            {
                _appRoleGroupRepository.Add(role);
            }
            return true;
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId)
        {
            return _appRoleRepository.GetListRoleByGroupId(groupId);
        }

        public IEnumerable<ApplicationRole> GetListRoleByRoleGroupId(int groupId,string roleId)
        {
            return _appRoleRepository.GetListRoleByGroupRoleId(groupId,roleId);
        }

        public ApplicationRoleGroup GetRoleGroup(int groupId, string roleId)
        {
            return _appRoleGroupRepository.GetRoleGroup(groupId, roleId);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
