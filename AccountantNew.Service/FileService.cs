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
            return _fileRepository.GetMulti(x => x.NewCategoryID == id).OrderByDescending(x=>x.CreatedDate);
        }

        public File GetByID(int id)
        {
            return _fileRepository.GetSingleById(id);
        }
    }
}
