﻿using AutoMapper;
using BookStore.Models.Code;
using BookStore.Models.Data;
using System.Linq.Expressions;

namespace BookStore.Models.Service
{
    public class BookService : BaseService<Book>, IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBaseService<Category> _cateService;
        //private readonly IBaseService<BookReview> _reviewService;

        public BookService(IGenericRepository<Book> baseRepo, ILogger<Book> logger,
            IBaseService<Category> cateService,
            //IBaseService<BookReview> reviewService,
            IMapper mapper) : base(baseRepo, logger)
        {
            _mapper = mapper;
            _cateService = cateService;
           // _reviewService = reviewService;
        }

        public List<Book> GetBookActiveInCategoryActive(Expression<Func<Book, bool>> expresstion)
        {
            // Lấy danh sách danh mục hoạt động
            var cateActive = _cateService.GetDbSet().Where(x => x.IsActive).ToList();
            // Lấy danh sách sách hoạt động có thuộc tính được chỉ định
            var bookActive = from b in _baseRepo.GetDbSet().Where(expresstion).ToList()
                             join c in cateActive
                             on b.CategoryId equals c.Id
                             where b.IsActive && b.Quantity > 0
                             select b;

            return bookActive.ToList();
        }

    }
}
