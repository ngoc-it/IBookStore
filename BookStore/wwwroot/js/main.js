(function ($) {
    "use strict";

    // Dropdown on mouse hover
    $(document).ready(function () {
        // Hàm để chuyển đổi giữa hover và click trên dropdown, dựa trên kích thước của cửa sổ
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                // Thêm sự kiện hover vào dropdown khi cửa sổ lớn hơn 992px
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                // Xóa sự kiện hover khi kích thước cửa sổ nhỏ hơn 992px
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod(); // Gọi hàm ngay khi trang tải lần đầu
        $(window).resize(toggleNavbarMethod); // Gọi lại hàm khi cửa sổ thay đổi kích thước

        // Kiểm tra mật khẩu khi người dùng nhập vào trường mật khẩu
        $('.register-form #inp-password').on('input', function () {
            let pass = this.value,
                name = this.getAttribute('name');

            // Kiểm tra xem mật khẩu có bị trống không
            if (pass.length == 0) {
                $(`span[data-valmsg-for="${name}"]`).text('Mật khẩu không được để trống');
            }
            // Kiểm tra xem mật khẩu có ít hơn 8 ký tự không
            else if (pass.length < 8) {
                $(`span[data-valmsg-for="${name}"]`).text('Mật khẩu chứa tối thiểu 8 ký tự');
            }
            /* Kiểm tra xem mật khẩu có chứa ít nhất 1 chữ cái, 1 số, và 1 ký tự đặc biệt không
            else if (!(pass.match(/[a-zA-Z]/) && pass.match(/[0-9]/) && pass.match(/([!,%,&,@,#,$,^,*,?,_,~])/))) {
                $(`span[data-valmsg-for="${name}"]`).text('Mật khẩu chứa tối thiểu 1 chữ số và 1 ký tự đặc biệt');
            }
            */
            else {
                $(`span[data-valmsg-for="${name}"]`).text('');
            }
        });

        // Kiểm tra mật khẩu nhập lại có khớp với mật khẩu ban đầu không
        $('.register-form #inp-re-password').on('input', function () {
            let repass = this.value,
                name = this.getAttribute('name'),
                pass = $('.register-form #inp-password').val();

            // Kiểm tra xem mật khẩu nhập lại có khớp không
            if (repass !== pass) {
                $(`span[data-valmsg-for="${name}"]`).text('Mật khẩu không khớp');
            } else {
                $(`span[data-valmsg-for="${name}"]`).text('');
            }
        });
    });

    // Back to top button (Nút quay về đầu trang)
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        // Chuyển trang lên đầu một cách mượt mà khi nhấn vào nút quay về đầu trang
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });

    // Vendor carousel (Carousel cho nhà cung cấp)
    $('.vendor-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0: {
                items: 2 // Hiển thị 2 item ở độ phân giải nhỏ nhất
            },
            576: {
                items: 3
            },
            768: {
                items: 4
            },
            992: {
                items: 5
            },
            1200: {
                items: 6 // Hiển thị 6 item ở độ phân giải lớn nhất
            }
        }
    });

    // Related carousel (Carousel cho các sản phẩm liên quan)
    $('.related-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0: {
                items: 1 // Hiển thị 1 item ở độ phân giải nhỏ nhất
            },
            576: {
                items: 2
            },
            768: {
                items: 3
            },
            992: {
                items: 4 // Hiển thị 4 item ở độ phân giải lớn hơn
            }
        }
    });

    // Product Quantity (Điều chỉnh số lượng sản phẩm)
    $('.quantity button').on('click', function () {
        var button = $(this);
        let maxQuantity = parseInt(button.attr('max-quantity')); // Lấy số lượng tối đa từ thuộc tính
        var oldValue = button.parent().parent().find('input').val();

        // Tăng số lượng khi nút cộng được nhấn
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue);

            if (oldValue < maxQuantity) {
                newVal = parseFloat(oldValue) + 1;
            }
        }
        // Giảm số lượng khi nút trừ được nhấn
        else {
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        button.parent().parent().find('input').val(newVal); // Cập nhật giá trị input
    });

})(jQuery);
