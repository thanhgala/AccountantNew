using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace AccountantNew.Service
{
    public interface IFileService
    {
        File Add(File files);

        void Update(File files);

        File Delete(int id);

        IEnumerable<File> GetAll();

        IEnumerable<File> GetListFileByCateID(int id);

        IEnumerable<File> GetListFileByCateID(int id,string keyword);

        IEnumerable<File> GetListFileByCateIDPaging(int id,string keyword,int page, int fileSize, out int totalRow,out int totalApproval);

        IEnumerable<File> GetListFileByCateIDPagingApproval(int id, string keyword, int page, int fileSize, out int totalRow);

        File GetByID(int id);

        void Save();
    }

    public class FileService : IFileService
    {
        private IFileRepository _fileRepository;
        private IUnitOfWork _unitOfWork;

        public FileService(IFileRepository fileRepository, IUnitOfWork unitOfWork)
        {
            this._fileRepository = fileRepository;
            this._unitOfWork = unitOfWork;
        }
        public File Add(File files)
        {
            return _fileRepository.Add(files);
        }

        public File Delete(int id)
        {
            return _fileRepository.Delete(id);
        }

        public void Update(File files)
        {
            _fileRepository.Update(files);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<File> GetAll()
        {
            return _fileRepository.GetAll();
        }

        public IEnumerable<File> GetListFileByCateID(int id)
        {
            return _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true).OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<File> GetListFileByCateID(int id,string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true && x.Name.Contains(keyword)).OrderByDescending(x => x.CreatedDate);
            }
            else
            {
                return _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true).OrderByDescending(x => x.CreatedDate);
            }
        }

        public IEnumerable<File> GetListFileByCateIDPaging(int id,string keyword , int page, int fileSize, out int totalRow,out int totalApproval)
        {
            IEnumerable<File> query;
            totalApproval = _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == false).Count();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true && x.Name.Contains(keyword)).OrderByDescending(x => x.CreatedDate);
            }
            else
            {
                query = _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == true).OrderByDescending(x => x.CreatedDate);
            }
            
            totalRow = query.Count();

            return query.Skip((page - 1) * fileSize).Take(fileSize);
        }

        public IEnumerable<File> GetListFileByCateIDPagingApproval(int id, string keyword, int page, int fileSize, out int totalRow)
        {
            IEnumerable<File> query;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == false && x.Name.Contains(keyword)).OrderByDescending(x => x.CreatedDate);
            }
            else
            {
                query = _fileRepository.GetMulti(x => x.NewCategoryID == id && x.Status == false).OrderByDescending(x => x.CreatedDate);
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * fileSize).Take(fileSize);
        }

        public File GetByID(int id)
        {
            return _fileRepository.GetSingleById(id);
        }
    }
}
