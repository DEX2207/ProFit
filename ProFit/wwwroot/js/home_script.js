document.addEventListener('DOMContentLoaded',function (){
    let currentSlide = 0;
    const cardWrapper=document.querySelector('.card-wrapper');
    const cards=document.querySelectorAll(".card");
    const cardsPerRow=3;
    const totalSlides=Math.ceil(cards.length/cardsPerRow);
    
    function next(){
        if(currentSlide<totalSlides-1){
            currentSlide++;
            cardWrapper.style.transform=`translateX(-${current*100}%)`;
        }
    }
    
    function prev(){
        if(currentSlide>0){
            currentSlide--;
            cardWrapper.style.transform=`translateX(-${currentSlide*100}%)`;
        }
    }
    
    document.querySelector('.arrow.right').addEventListener('click',next);
    document.querySelector('.arrow.left').addEventListener('click',prev);
})