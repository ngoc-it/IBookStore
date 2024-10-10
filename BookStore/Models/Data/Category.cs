﻿namespace BookStore.Models.Data
{
    public class Category : BaseData
    {
        public string CategoryId { get; set; } //Mã loại

        public string CategoryName { get; set; } //Tên lọai sách
        public string Description { get; set; } //mô tả

        public bool IsActive { get; set; } //Trạng thái

    }
}