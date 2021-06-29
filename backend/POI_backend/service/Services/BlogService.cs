using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq.Expressions;
using System.Linq;

namespace POI.service.Services
{
    public interface IBlogService : IGenericService<Blog>
    {
        public Task<CreateEnum> CreateNewBlog(CreateBlogViewModel model, Guid userID);
        public List<ResponseBlogViewModel> GetBlogs(Expression<Func<Blog, bool>> predicate, bool istracked);
        public UpdateEnum UpdateBlog(UpdateBlogViewModel model, Guid userId);
        public DeleteEnum ArchiveBlog(Guid blogId, Guid userId);
        public UpdateEnum ApproveBlog(Guid id);
    }
    public class BlogService : GenericService<Blog>, IBlogService
    {
        private readonly IPoiBlogRepository _poiBlogRepository;
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;
        private readonly ITripRepository _tripRepository;

        public BlogService(IPoiBlogRepository poiBlogRepository,
            IMapper mapper,
            ITripRepository tripRepository,
            IBlogRepository blogRepository) :
            base(blogRepository)
        {
            _mapper = mapper;
            _blogRepository = blogRepository;
            _tripRepository = tripRepository;
            _poiBlogRepository = poiBlogRepository;
        }

        public async Task<CreateEnum> CreateNewBlog(CreateBlogViewModel model, Guid userID)
        {
            Blog blog = _mapper.Map<Blog>(model);
            blog.UserId = userID;
            var trip = _tripRepository.FirstOrDefault(m => m.UserId.Equals(userID) && m.TripId.Equals(model.TripId), false);

            if (trip != null)
            {
                try
                {
                    await AddAsync(blog);
                    await SaveChangesAsync();
                    var blogID = blog.BlogId;
                    foreach (Guid poiId in model.PoiIds)
                    {
                        Poiblog poiBlog = new Poiblog
                        {
                            BlogId = blogID,
                            PoiId = poiId,
                            Status = (int)PoiBlogEnum.Available
                        };
                        await _poiBlogRepository.AddAsync(poiBlog);
                        await _poiBlogRepository.SaveChangesAsync();
                    }
                    return CreateEnum.Success;
                }
                catch
                {
                    return CreateEnum.ErrorInServer;
                }
            }
            else
            {
                return CreateEnum.Error;
            }
        }

        public UpdateEnum UpdateBlog(UpdateBlogViewModel model, Guid userId)
        {
            Blog blog = _mapper.Map<Blog>(model);
            blog.UserId = userId;
            var trip = _tripRepository.FirstOrDefault(m => m.UserId.Equals(userId) && m.TripId.Equals(model.TripId), false);
            if (trip != null)
            {
                Update(blog);
                Savechanges();
                var blogId = blog.BlogId;
                //var poiBlogs = _poiBlogRepository.Find(m => m.BlogId.Equals(blogId), false);
                foreach (Guid poiId in model.PoiIds)
                {
                    var poiBlog = _poiBlogRepository.FirstOrDefault(m => m.BlogId.Equals(blog.BlogId) && m.PoiId.Equals(poiId), false);
                    if (poiBlog == null)
                    {
                        Poiblog newpoiBlog = new Poiblog
                        {
                            BlogId = blog.BlogId,
                            PoiId = poiId,
                            Status = (int)PoiBlogEnum.Available
                        };
                        _poiBlogRepository.Add(newpoiBlog);
                        _poiBlogRepository.SaveChanges();
                    }
                }
                return UpdateEnum.Success;
            }
            else
            {
                return UpdateEnum.Error;
            }
        }

        public DeleteEnum ArchiveBlog(Guid blogId, Guid userId)
        {
            Blog blog = _blogRepository.GetByID(blogId);
            if (blog == null || blog.Status == (int)BlogEnum.Disable)
            {
                return DeleteEnum.Failed;
            }
            else if (blog.UserId.Equals(userId))
            {
                blog.Status = (int)BlogEnum.Disable;
                try
                {
                    Update(blog);
                    Savechanges();
                    return DeleteEnum.Success;
                }
                catch (Exception e)
                {
                    return DeleteEnum.ErrorInServer;
                }
            }
            else
            {
                return DeleteEnum.NotOwner;
            }
        }
        public UpdateEnum ApproveBlog(Guid id)
        {
            Blog blog = _blogRepository.GetByID(id);
            if (blog == null || blog.Status != (int)BlogEnum.Pending)
            {
                return UpdateEnum.Error;
            }
            else
            {
                blog.Status = (int)BlogEnum.Available;
                try
                {
                    Update(blog);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    return UpdateEnum.ErrorInServer;
                }
            }
        }

        public List<ResponseBlogViewModel> GetBlogs(Expression<Func<Blog, bool>> predicate, bool istracked)
        {
            IQueryable<Blog> blogs = _blogRepository.GetBlogs(predicate, istracked);
            List<Blog> blogList = blogs.ToList();
            List<ResponseBlogViewModel> responses = _mapper.Map<List<ResponseBlogViewModel>>(blogs);
            for (int i = 0; i < responses.Count(); i++)
            {
                var response = responses[i];
                var blog = blogList[i];
                response.User = _mapper.Map<AuthenticatedUserViewModel>(blog.User);
            }
            return responses;
        }
    }
}
