(function ($) {
    "use strict"; // Start of use strict, giúp bắt lỗi cảnh báo khi gặp lỗi cú pháp

    // Toggle the side navigation
    $("#sidebarToggle, #sidebarToggleTop").on('click', function (e) {
        $("body").toggleClass("sidebar-toggled");
        $(".sidebar").toggleClass("toggled");
        if ($(".sidebar").hasClass("toggled")) {
            $('.sidebar .collapse').collapse('hide');
        };
    });
    //Khi người dùng bấm vào phần tử có ID là #sidebarToggle hoặc #sidebarToggleTop, đoạn mã này sẽ thêm hoặc xóa lớp CSS "sidebar-toggled" vào phần tử <body> và "toggled" vào phần tử .sidebar. Điều này sẽ điều khiển việc mở hoặc đóng thanh điều hướng (sidebar).
    //Nếu thanh điều hướng đã được "toggled", nó sẽ đóng các menu con được mở(collapse('hide')).



    // Close any open menu accordions when window is resized below 768px
    //đóng menu khi thay đổi kích thước cửa sổ
    $(window).resize(function () {
        if ($(window).width() < 768) {
            $('.sidebar .collapse').collapse('hide');
        };

        // Toggle the side navigation when window is resized below 480px
        if ($(window).width() < 480 && !$(".sidebar").hasClass("toggled")) {
            $("body").addClass("sidebar-toggled");
            $(".sidebar").addClass("toggled");
            $('.sidebar .collapse').collapse('hide');
        };
    });

//Khi kích thước cửa sổ thay đổi, đoạn mã này kiểm tra chiều rộng của cửa sổ:
//Nếu chiều rộng nhỏ hơn 768px, nó sẽ đóng tất cả các menu con(dùng collapse('hide')).
//Nếu chiều rộng nhỏ hơn 480px và sidebar chưa được toggled, nó sẽ thêm lớp "sidebar-toggled" vào < body > và "toggled" vào.sidebar, đồng thời đóng các menu con.





    // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
    //Ngăn cuộn khi di chuột lên thanh điều hướng cố định
    $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function (e) {
        if ($(window).width() > 768) {
            var e0 = e.originalEvent,
                delta = e0.wheelDelta || -e0.detail;
            this.scrollTop += (delta < 0 ? 1 : -1) * 30;
            e.preventDefault();
        }
    });
    //Nếu phần thân của trang có lớp "fixed-nav" và người dùng cuộn chuột trên thanh điều hướng khi chiều rộng cửa sổ lớn hơn 768px, 
    //nó sẽ điều chỉnh giá trị cuộn của thanh điều hướng theo giá trị delta(tốc độ và hướng cuộn).Điều này ngăn không cho toàn bộ
    //nội dung của trang cuộn theo khi người dùng cuộn trên thanh điều hướng.




    // Scroll to top button appear
    //Hiển thị nút "Scroll to Top" (cuộn lên đầu trang)
    $(document).on('scroll', function () {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
            $('.scroll-to-top').fadeIn();
        } else {
            $('.scroll-to-top').fadeOut();
        }
    });
    //Khi người dùng cuộn trang, nếu khoảng cách cuộn lớn hơn 100px, nút "Scroll to Top"
    //(nút cuộn lên đầu trang) sẽ hiện ra(bằng cách dùng fadeIn).Nếu khoảng cách nhỏ hơn, nút này sẽ biến mất(fadeOut).



    // Smooth scrolling using jQuery easing (cuộn mượt mà)
    $(document).on('click', 'a.scroll-to-top', function (e) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($anchor.attr('href')).offset().top)
        }, 1000, 'easeInOutExpo');
        e.preventDefault();
    });
    //Khi người dùng nhấp vào một liên kết có lớp scroll-to-top, 
    //trang sẽ cuộn mượt mà đến vị trí mà liên kết này trỏ tới.Thao tác này được thực hiện trong khoảng thời gian 1000ms
    //với hiệu ứng easeInOutExpo để tạo ra sự chuyển động mượt mà.


    //Hiển thị tắt bật mật khẩu
    $(document).on('click', '.form-password-toggle i', function (e) {
        const formPasswordToggle = e.currentTarget.closest('.form-password-toggle');
        const formPasswordToggleIcon = formPasswordToggle.querySelector('i');
        const formPasswordToggleInput = formPasswordToggle.querySelector('input');

        if (formPasswordToggleInput.getAttribute('type') === 'text') {
            formPasswordToggleInput.setAttribute('type', 'password');
            formPasswordToggleIcon.classList.replace('bx-show', 'bx-hide');
        } else if (formPasswordToggleInput.getAttribute('type') === 'password') {
            formPasswordToggleInput.setAttribute('type', 'text');
            formPasswordToggleIcon.classList.replace('bx-hide', 'bx-show');
        }
    });
    //Khi người dùng nhấp vào biểu tượng i trong phần tử có lớp form-password-toggle, đoạn mã này sẽ chuyển đổi kiểu của input giữa 'text' (hiển thị mật khẩu) và 'password' (ẩn mật khẩu).
//Nó cũng thay đổi biểu tượng đi kèm giữa bx - show và bx - hide để báo hiệu trạng thái hiển thị mật khẩu.


})(jQuery); // End of use strict
