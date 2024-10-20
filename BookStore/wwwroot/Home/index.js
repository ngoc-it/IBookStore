//console.log(anh.length)
let index = 0;
const traibtn = document.querySelector('.bx-chevron-left');
const phaibtn = document.querySelector('.bx-chevron-right');

const anh = document.querySelectorAll('.poster-content-anh img');

phaibtn.addEventListener("click", function () {
    index = index + 1;
    if (index > anh.length - 1) {
        index = 0;
    }
    document.querySelector(".poster-content-anh").style.right = index * 100 + "%";
});

traibtn.addEventListener("click", function () {
    index = index - 1;
    if (index < 0) {
        index = anh.length - 1;
    }
    document.querySelector(".poster-content-anh").style.right = index * 100 + "%";
});

// Tiêu đề
const anhli = document.querySelectorAll('.poster-content-tieude li');

anhli.forEach(function (image, index) {
    image.addEventListener("click", function () {
        removetd1();
        document.querySelector(".poster-content-anh").style.right = index * 100 + "%";
        image.classList.add("td1");
    });
});

function removetd1() {
    let anhtd1 = document.querySelector('.td1');
    anhtd1.classList.remove("td1");
}
