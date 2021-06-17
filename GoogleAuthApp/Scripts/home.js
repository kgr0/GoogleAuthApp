"use strict"

window.onload = function () {
    const parallax = document.querySelector('.parallax');
    //const parallax = document.querySelector('.images-parallax__item');

    if (parallax) {
        const content = document.querySelector('.parallax__container');

        //Koeficjent move
        const forBackend = 80;//40
        const forPuzzle1 = 30;//50
        const forPuzzle2 = 40;
        const forPuzzle3 = 20;
        const forPuzzle4 = 30;

        //Speed of animation
        const speed = 0.15;

        //Begin parameters
        let positionX = 0, positionY = 0;
        let coordXprocent = 0, coordYprocent = 0;
        let degry = 0;

        function setMouseParallaxStyle() {
            const distX = coordXprocent - positionX;
            const distY = coordYprocent - positionY;

            positionX = positionX + (distX * speed);
            positionY = positionY + (distY * speed);

            //Transmit styles
            $('.images-parallax__back').css('transform', 'translate(' + positionX / forBackend + '%,' + positionY / forBackend + '%) ');
            $('.images-parallax__puzzle1').css('transform', 'translate(' + positionX / forPuzzle1 + '%,' + positionY / forPuzzle1 + '%) ');
            $('.images-parallax__puzzle2').css('transform', 'translate(' + positionX / forPuzzle2 + '%,' + positionY / forPuzzle2 + '%) ');
            $('.images-parallax__puzzle3').css('transform', 'translate(' + positionX / forPuzzle3 + '%,' + positionY / forPuzzle3 + '%) ');
            $('.images-parallax__puzzle4').css('transform', 'translate(' + positionX / forPuzzle4 + '%,' + positionY / forPuzzle4 + '%) ');

            /*$('.images-parallax__puzzle1').css('transform', 'rotate(' + degry + 'deg) ');*/
           /* $('.images-parallax__puzzle1').css('transform-origin', positionX / forPuzzle1 + '%,' + positionY / forPuzzle1 + '%,' + 0 );*/
            requestAnimationFrame(setMouseParallaxStyle);
        }
        setMouseParallaxStyle();
        parallax.addEventListener("mousemove", function (e) {
            //Get height and width 
            const parallaxWidth = parallax.offsetWidth;
            const parallaxHeight = parallax.offsetHeight;
   
            // Zero is on center 
            const coordX = e.pageX - parallaxWidth / 2;
            const coordY = e.pageY - parallaxHeight / 2;

            //Get procents
            coordXprocent = coordX / parallaxWidth * 100;
            coordYprocent = coordY / parallaxHeight * 100;
        });
        function checkIt() {
            /*$('.images-parallax__puzzle1').css('transform', 'rotate(' + coordXprocent + '%) ');*/
            $('.images-parallax__puzzle2').css('transform', 'translate(' + positionX / forPuzzle2 + '%,' + positionY / forPuzzle2 + '%) ');
            $('.images-parallax__puzzle3').css('transform', 'translate(' + positionX / forPuzzle3 + '%,' + positionY / forPuzzle3 + '%) ');
            $('.images-parallax__puzzle4').css('transform', 'translate(' + positionX / forPuzzle4 + '%,' + positionY / forPuzzle4 + '%) ');
            requestAnimationFrame(checkIt);
        }
        checkIt()
        window.addEventListener('wheel', function (e) {
            degry = degry + 1;
        });
    }
}