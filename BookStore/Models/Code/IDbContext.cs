using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStore.Models.Code
{
    // Giao diện IDbContext định nghĩa các phương thức cho ngữ cảnh cơ sở dữ liệu
    public interface IDbContext : IDisposable
    {
        // Phương thức này cho phép lấy DbSet cho một loại thực thể.
        DbSet<T> Set<T>()
            where T : class; // Chỉ định T phải là một lớp

        // Phương thức này lấy thông tin về trạng thái của một thực thể trong ngữ cảnh.
        EntityEntry Entry(object o);

        /// <param name="acceptAllChangesOnSuccess">Chấp nhận tất cả thay đổi nếu thao tác lưu thành công.</param>
        /// <param name="cancellationToken">Token để hủy thao tác lưu.</param>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken));

        //Bằng cách định nghĩa các phương thức cần thiết để tương tác với cơ sở dữ liệu,
        //nó giúp tách biệt logic truy cập dữ liệu khỏi các phần khác của ứng dụng,
        //từ đó cải thiện khả năng bảo trì và mở rộng của mã nguồn.
    }
}
