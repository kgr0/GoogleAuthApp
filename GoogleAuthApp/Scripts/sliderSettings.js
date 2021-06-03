$(document).ready(function(){
    $('.slider').slick({
        arrows: true,
        dots: true,
        adaptiveHeight: true,
        sliderToShow: 1,
        slidesToScroll: 1,
        speed: 1000,
        infinite: true,
        initialSlide: 0,
        //autoplay: true,
        //autoplaySpeed: 5000,
        pausedOnFocus: true,
        pauseOnHover: true,
        pauseOnDotsHover: true,
        draggable: true,
        swipe: true,
        touchTreshold: 7,
        touchMove: true,
        waitForAnimate: false,
        centerMode: true,
        fade: false
    });
});
