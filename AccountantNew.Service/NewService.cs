using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace AccountantNew.Service
{
    public interface INewService
    {
        New Add(New news);

        void Update(New news);

        New Delete(int id);

        IEnumerable<New> GetAll();

        IEnumerable<New> GetAll(string keyword);

        New GetByID(int id);

        IEnumerable<New> GetLastestNew(int top,int categoryID);

        IEnumerable<New> GetHotNew(int top);

        IEnumerable<New> GetListNewByParentID(int parentId);

        IEnumerable<New> GetListNewByID(int id);

        void Save();
    }

    public class NewService : INewService
    {
        private INewRepository _newRepository;
        private IUnitOfWork _unitOfWork;

        public NewService(INewRepository newRepository, IUnitOfWork unitOfWork)
        {
            this._newRepository = newRepository;
            this._unitOfWork = unitOfWork;
        }
        public New Add(New news)
        {
            return _newRepository.Add(news);
        }

        public New Delete(int id)
        {
            return _newRepository.Delete(id);
        }

        public void Update(New news)
        {
            _newRepository.Update(news);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<New> GetAll()
        {
            return _newRepository.GetAll();
        }

        public IEnumerable<New> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _newRepository.GetMulti(n => n.Name.Contains(keyword));
            }
            else
            {
                return _newRepository.GetAll();
            }
        }

        public New GetByID(int id)
        {
            return _newRepository.GetSingleById(id);
        }

        public IEnumerable<New> GetLastestNew(int top, int categoryID)
        {
            return _newRepository.GetLastestNew().Where(x=>x.NewCategoryID == categoryID || x.NewCategory.ParentID == categoryID).OrderByDescending(x=>x.CreatedDate).Take(top);
        }

        public IEnumerable<New> GetHotNew(int top)
        {
            return _newRepository.GetHotNew().OrderByDescending(x=>x.CreatedDate).Take(top);
        }

        public IEnumerable<New> GetListNewByParentID(int parentId)
        {
            return _newRepository.GetMulti(x => x.NewCategory.ParentID == parentId && x.Status == true && x.Private == false).OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<New> GetListNewByID(int id)
        {
            return _newRepository.GetMulti(x => x.NewCategoryID == id);
        }
    }
}
