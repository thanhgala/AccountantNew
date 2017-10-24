using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace AccountantNew.Service
{
    public interface ICommentService
    {
        Comment Add(Comment comments);

        void Update(Comment comments);

        Comment Delete(int id);

        IEnumerable<Comment> GetAll();

        IEnumerable<Comment> GetListCommentByPostID(int id);

        Comment GetByID(int id);

        void Save();
    }

    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        private IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            this._commentRepository = commentRepository;
            this._unitOfWork = unitOfWork;
        }

        public Comment Add(Comment comment)
        {
            return _commentRepository.Add(comment);
        }

        public Comment Delete(int id)
        {
            return _commentRepository.Delete(id);
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Comment> GetAll()
        {
            return _commentRepository.GetAll();
        }

        public Comment GetByID(int id)
        {
            return _commentRepository.GetSingleById(id);
        }

        public IEnumerable<Comment> GetListCommentByPostID(int id)
        {
            return _commentRepository.GetMulti(x => x.PostID == id,new string[] { "ApplicationUser" });
        }
    }
}
