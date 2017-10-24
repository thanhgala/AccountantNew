using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace AccountantNew.Service
{
    public interface IPostService
    {
        Post Add(Post posts);

        void Update(Post posts);

        Post Delete(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetListPost(int id);

        IEnumerable<Post> GetListPostPaging(int id,int page,int pageSize,out int totalRow);

        Post GetByID(int id);

        Post GetPostWithAppUser(int id);

        Post GetByAlias(string alias);

        void Save();
    }

    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
        }
        public Post Add(Post posts)
        {
            return _postRepository.Add(posts);
        }

        public Post Delete(int id)
        {
            return _postRepository.Delete(id);
        }

        public void Update(Post posts)
        {
            _postRepository.Update(posts);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }

        public IEnumerable<Post> GetListPost(int id)
        {
            return _postRepository.GetMulti(x => x.NewCategoryID == id,new string[] { "ApplicationUser"}).OrderByDescending(x=>x.CreatedDate);
        }

        public IEnumerable<Post> GetListPostPaging(int id, int page, int pageSize, out int totalRow)
        {
            var query = _postRepository.GetMulti(x => x.NewCategoryID == id, new string[] { "ApplicationUser" }).OrderByDescending(x => x.CreatedDate);
            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Post GetByID(int id)
        {
            return _postRepository.GetSingleById(id);
        }

        public Post GetPostWithAppUser(int id)
        {
            return _postRepository.GetPostWithAppUser(id);
        }

        public Post GetByAlias(string alias)
        {
            return _postRepository.GetByAlias(alias);
        }
    }
}
