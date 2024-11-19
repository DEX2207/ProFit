document.addEventListener('DOMContentLoaded',function (){
    window.addEventListener('scroll',function (){
        var header= document.getElementById('header-top');
        var scrollTop =window.scrollY;
        var maxScroll =250;

        var opacity =Math.min(scrollTop/maxScroll,0.7);
        header.style.backgroundColor=`rgb(30,144,255,${opacity})`;
        header.style.backdropFilter=`blur(${opacity*10}px)`
    });
    function toggleMenu(){
        const sideMenu=document.getElementById('side-menu');
        
        sideMenu.classList.toggle('active');
    }
    
    document.getElementById('hamburger').addEventListener('click',toggleMenu);
});