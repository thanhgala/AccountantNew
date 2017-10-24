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
    public interface INewCategoryService
    {
        NewCategory Add(NewCategory news);

        void Update(NewCategory news);

        NewCategory Delete(int id);

        IEnumerable<NewCategory> GetAll();

        IEnumerable<NewCategory> GetRootCategory();

        IEnumerable<NewCategory> GetChildCategory(int id);

        NewCategory GetByID(int id);

        NewCategory GetByAlias(string alias);

        void Save();

        bool AddCategoryToGroups(IEnumerable<ApplicationCateGroup> catePermission, int groupId,bool add);

        IEnumerable<int> GetListCategoryByGroupId(int groupId);
    }

    public class NewCategoryService : INewCategoryService
    {
        private INewCategoryRepository _newCategoryRepository;
        private IApplicationCateGroupRepository _appCateGroupRepository;
        private IUnitOfWork _unitOfWork;

        public NewCategoryService(INewCategoryRepository newCategoryRepository, IApplicationCateGroupRepository appCateGroupRepository, IUnitOfWork unitOfWork)
        {
            this._newCategoryRepository = newCategoryRepository;
            this._appCateGroupRepository = appCateGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public NewCategory Add(NewCategory newCategory)
        {
            return _newCategoryRepository.Add(newCategory);
        }

        public NewCategory Delete(int id)
        {
            return _newCategoryRepository.Delete(id);
        }

        public IEnumerable<NewCategory> GetAll()
        {
            return _newCategoryRepository.GetMulti(x=>x.Status == true);
        }

        public IEnumerable<NewCategory> GetRootCategory()
        {
            return _newCategoryRepository.GetMulti(x => x.ParentID == null).OrderBy(x=>x.DisplayOrder);
        }

        public IEnumerable<NewCategory> GetChildCategory(int id)
        {
            return _newCategoryRepository.GetMulti(x => x.ParentID == id);
        }

        public NewCategory GetByID(int id)
        {
            return _newCategoryRepository.GetSingleById(id);
        }

        public NewCategory GetByAlias(string alias)
        {
            return _newCategoryRepository.GetByAlias(alias);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NewCategory newCategory)
        {
            _newCategoryRepository.Update(newCategory);
        }

        public bool AddCategoryToGroups(IEnumerable<ApplicationCateGroup> cateGroups, int groupId,bool add)
        {
            if (add)
            {
                foreach (var cateGroup in cateGroups)
                {
                    _appCateGroupRepository.Add(cateGroup);
                }
            }
            else
            {
                foreach (var cateGroup in cateGroups)
                {
                    _appCateGroupRepository.DeleteMulti(x => x.GroupId == cateGroup.GroupId && x.CategoryId == cateGroup.CategoryId);
                }
            }
            return true;
        }

        public IEnumerable<int> GetListCategoryByGroupId(int groupId)
        {
            return _newCategoryRepository.GetListCategoryByGroupId(groupId);
        }

    }
}
