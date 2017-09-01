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
    }

    public class NewCategoryService : INewCategoryService
    {
        private INewCategoryRepository _newCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public NewCategoryService(INewCategoryRepository newCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._newCategoryRepository = newCategoryRepository;
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
            return _newCategoryRepository.GetAll();
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

    }
}
