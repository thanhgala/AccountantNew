using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace AccountantNew.Service
{
    public interface INewService
    {
        New Add(New news);

        void Update(New news);

        New Delete(int id);

        IEnumerable<New> GetAll();

        IEnumerable<New> GetAllNews(string keyword,out int totalApproval);

        IEnumerable<New> GetAllNewsApproval(string keyword);

        IEnumerable<New> GetAllNotifications(string keyword);

        New GetByID(int id);

        New GetNewWithAppUser(int id);

        IEnumerable<New> GetLastestNew(int top,int categoryID);

        IEnumerable<New> GetHotNew(int top);

        IEnumerable<New> GetListNewByParentID(int parentId);

        IEnumerable<New> GetListNewByID(int id);

        IEnumerable<New> GetListNewByName(string name,bool isAuthen);

        IEnumerable<New> GetListNewByCategoryIdPaging(int categoryId, int page, int pageSize, out int totalRow);

        IEnumerable<New> GetListNotificationByCategoryIdPaging(int categoryId, int page, int pageSize, out int totalRow);

        List<New> GetListNotification(int id);

        void Save();
    }

    public class NewService : INewService
    {
        private INewRepository _newRepository;
        private INewCategoryRepository _newCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public NewService(INewRepository newRepository, INewCategoryRepository newCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._newRepository = newRepository;
            this._newCategoryRepository = newCategoryRepository;
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

        public IEnumerable<New> GetAllNews(string keyword,out int totalApproval)
        {
            totalApproval = _newRepository.GetMulti(x => x.NewCategoryID == Common.CommonConstants.NewID1 && x.Status == false).Count();

            if (!string.IsNullOrEmpty(keyword))
            {
                return _newRepository.GetMulti(n => (n.Name.Contains(keyword) || n.ApplicationUser.FullName.Contains(keyword)) && n.NewCategoryID == Common.CommonConstants.NewID1 && n.Status == true,new string[] { "ApplicationUser" });
            }
            else
            {
                return _newRepository.GetMulti(x => x.NewCategoryID == Common.CommonConstants.NewID1 && x.Status == true, new string[] { "ApplicationUser" });
            }
        }

        public IEnumerable<New> GetAllNewsApproval(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _newRepository.GetMulti(n => n.Name.Contains(keyword) && n.NewCategoryID == Common.CommonConstants.NewID1 && n.Status == false, new string[] { "ApplicationUser" });
            }
            else
            {
                return _newRepository.GetMulti(x => x.NewCategoryID == Common.CommonConstants.NewID1 && x.Status == false, new string[] { "ApplicationUser" });
            }
        }

        public IEnumerable<New> GetAllNotifications(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _newRepository.GetMulti(n => (n.Name.Contains(keyword) || n.NewCategory.Name.Contains(keyword)) && (n.NewCategoryID == Common.CommonConstants.LocalAccounting || n.NewCategoryID == Common.CommonConstants.AnotherDepart ),new string[] { "NewCategory","ApplicationUser" });
            }
            else
            {
                return _newRepository.GetMulti(x => x.NewCategoryID == Common.CommonConstants.LocalAccounting || x.NewCategoryID == Common.CommonConstants.AnotherDepart,new string[] { "NewCategory","ApplicationUser" });
            }
        }

        public New GetByID(int id)
        {
            return _newRepository.GetSingleById(id);
        }

        public New GetNewWithAppUser(int id)
        {
            return _newRepository.GetNewWithAppUser(id);
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
            return _newRepository.GetMulti(x => x.NewCategory.ParentID == parentId && x.Status == true).OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<New> GetListNewByID(int id)
        {
            return _newRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true, new string[] { "NewCategory" });
        }

        public IEnumerable<New> GetListNewByName(string name,bool isAuthen)
        {
            if(!isAuthen)
                return _newRepository.GetMulti(x => x.Name.Contains(name) && x.Status == true && x.NewCategoryID != Common.CommonConstants.LocalAccounting, new string[] { "NewCategory" }).OrderByDescending(x => x.CreatedDate);
            return _newRepository.GetMulti(x => x.Name.Contains(name) && x.Status == true,new string[] { "NewCategory" }).OrderByDescending(x=>x.CreatedDate);
        }

        public IEnumerable<New> GetListNewByCategoryIdPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            var query = _newRepository.GetMulti(x => x.Status == true && x.NewCategoryID == categoryId, new string[] { "NewCategory" }).OrderByDescending(x=>x.CreatedDate);
            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<New> GetListNotificationByCategoryIdPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            var query = GetListNotification(categoryId);
            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        private void LoadChildCategory(int id, List<New> lstAdd)
        {
            var childCategoryModel = _newCategoryRepository.GetMulti(x => x.ParentID == id && x.Status == true).OrderBy(x => x.DisplayOrder);
            if (childCategoryModel.Count() > 0)
            {
                foreach (var itemCategory in childCategoryModel)
                {
                    LoadChildCategory(itemCategory.ID, lstAdd);
                }
            }
            else
            {
                var newModel = GetListNewByID(id).OrderByDescending(x => x.CreatedDate).ToList();
                loadNew(newModel, lstAdd);
            }
        }

        private void loadNew(List<New> lst, List<New> lstAdd)
        {
            foreach (var itemNew in lst)
            {
                lstAdd.Add(itemNew);
            }
        }

        public List<New> GetListNotification(int id)
        {
            List<New> lstNotification = new List<New>();
            LoadChildCategory(id, lstNotification);
            return lstNotification;
        }

    }
}
