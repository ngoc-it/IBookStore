/**
 * Main
 */

'use strict';

let menu, animate;

(function () {
    // Khởi tạo menu
    //-----------------

    // Lấy tất cả các phần tử có id là 'layout-menu' và khởi tạo menu
    let layoutMenuEl = document.querySelectorAll('#layout-menu');
    layoutMenuEl.forEach(function (element) {
        // Tạo menu với cấu hình chiều dọc và không tự động đóng các menu con
        menu = new Menu(element, {
            orientation: 'vertical',
            closeChildren: false
        });
        // Cuộn đến menu đang hoạt động nhưng không dùng hiệu ứng cuộn
        window.Helpers.scrollToActive((animate = false));
        window.Helpers.mainMenu = menu;
    });

    // Khởi tạo menu togglers và gắn sự kiện click cho từng cái
    let menuToggler = document.querySelectorAll('.layout-menu-toggle');
    menuToggler.forEach(item => {
        item.addEventListener('click', event => {
            event.preventDefault(); // Ngăn chặn hành động mặc định
            window.Helpers.toggleCollapsed(); // Đổi trạng thái đóng/mở của menu
        });
    });

    // Hiển thị menu toggler khi di chuột vào menu, với độ trễ
    let delay = function (elem, callback) {
        let timeout = null;
        elem.onmouseenter = function () {
            // Đặt thời gian chờ 300ms (không dùng cho màn hình nhỏ)
            timeout = setTimeout(callback, Helpers.isSmallScreen() ? 0 : 300);
        };

        elem.onmouseleave = function () {
            // Ẩn menu toggler và xóa thời gian chờ khi chuột rời khỏi
            document.querySelector('.layout-menu-toggle').classList.remove('d-block');
            clearTimeout(timeout);
        };
    };
    if (document.getElementById('layout-menu')) {
        delay(document.getElementById('layout-menu'), function () {
            if (!Helpers.isSmallScreen()) {
                // Chỉ hiện menu toggler nếu màn hình không phải là nhỏ
                document.querySelector('.layout-menu-toggle').classList.add('d-block');
            }
        });
    }

    // Hiển thị bóng cho menu khi cuộn
    let menuInnerContainer = document.getElementsByClassName('menu-inner'),
        menuInnerShadow = document.getElementsByClassName('menu-inner-shadow')[0];
    if (menuInnerContainer.length > 0 && menuInnerShadow) {
        menuInnerContainer[0].addEventListener('ps-scroll-y', function () {
            // Hiển thị bóng khi thanh cuộn di chuyển
            menuInnerShadow.style.display = this.querySelector('.ps__thumb-y').offsetTop ? 'block' : 'none';
        });
    }

    // Khởi tạo các chức năng trợ giúp và các mục bổ sung
    // --------------------

    // Khởi tạo Tooltip Bootstrap
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Thêm lớp 'active' cho các mục accordion khi mở
    const accordionActiveFunction = function (e) {
        if (e.type == 'show.bs.collapse' || e.type == 'show.bs.collapse') {
            e.target.closest('.accordion-item').classList.add('active');
        } else {
            e.target.closest('.accordion-item').classList.remove('active');
        }
    };

    // Thêm sự kiện cho accordion khi mở/đóng
    const accordionTriggerList = [].slice.call(document.querySelectorAll('.accordion'));
    accordionTriggerList.map(function (accordionTriggerEl) {
        accordionTriggerEl.addEventListener('show.bs.collapse', accordionActiveFunction);
        accordionTriggerEl.addEventListener('hide.bs.collapse', accordionActiveFunction);
    });

    // Tự động cập nhật bố cục dựa vào kích thước màn hình
    window.Helpers.setAutoUpdate(true);

    // Chuyển đổi trạng thái hiển thị mật khẩu
    window.Helpers.initPasswordToggle();

    // Kích hoạt tính năng chuyển đổi giọng nói thành văn bản
    window.Helpers.initSpeechToText();

    // Quản lý trạng thái mở/đóng của menu với localStorage
    //------------------------------------------------------------------

    // Nếu màn hình nhỏ hoặc bố cục hiện tại là ngang, bỏ qua
    if (window.Helpers.isSmallScreen()) {
        return;
    }

    // Nếu bố cục là chiều dọc và màn hình lớn, tự động điều chỉnh trạng thái menu
    window.Helpers.setCollapsed(true, false);
})();
