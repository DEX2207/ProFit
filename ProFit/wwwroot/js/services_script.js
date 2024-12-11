document.getElementById('adult-min-price').addEventListener('input',updatePriceValues);
document.getElementById('adult-max-price').addEventListener('input',updatePriceValues);

function updatePriceValues(){
    const adultMin=document.getElementById('adult-min-price').value;
    const adultMax=document.getElementById('adult-max-price').value;
    
    document.getElementById('adult-price-values').innerText=`${adultMin}-${adultMax}`;
}
const sortSelect=document.getElementById('sort-options');
const productContainer=document.querySelector('.container-categories')

sortSelect.addEventListener('change',() =>{
   const sortOption=sortSelect.value;
   
   const products=Array.from(productContainer.querySelectorAll('.product-item'));
   
   products.sort((a,b)=>{
      switch (sortOption){
          case 'price-adult-asc':{
              const priceA=parseFloat(a.getElementById('price'));
              const priceB=parseFloat(b.getElementById('price'));
              return priceA-priceB;
          }
          case 'price-adult-desc':{
              const priceA=parseFloat(a.getElementById('price'));
              const priceB=parseFloat(b.getElementById('price'));
              return priceB-priceA;
          }
          case 'days-asc':{
              const priceA=parseFloat(a.getElementById('price'));
              const priceB=parseFloat(b.getElementById('price'));
              return priceA-priceB;
          }
          case 'days-desc':{
              const priceA=parseFloat(a.getElementById('validPer'));
              const priceB=parseFloat(b.getElementById('validPer'));
              return priceB-priceA;
          }
          default:
              location.reload();
      } 
   });
   
   products.forEach(product=>productContainer.appendChild(product));
});
document.getElementById('apply-filter').addEventListener('click',()=>{
   const adultMin=document.getElementById('adult-min-price').value;
   const adultMax=document.getElementById('adult-max-price').value;
   
   const mealTypes=Array.from(document.querySelectorAll('.meal-types input[type="checkbox"]:checked')).map(
       (checkbox)=>checkbox.value
   );
   
   const filterData={
       priceAfultMin: adultMin,
       priceAfultMax: adultMax
   };
   console.log('Отправляемые данные:', filterData);
   fetch('/Product/Filter',{
       method: 'POST',
       headers:{
           'Content-Type':'application/json',
       },
       body: JSON.stringify(filterData),
   })
       .then((response)=>{
           if(!response.ok){
               throw new Error('Ошибка при фильтрации данных');
           }
           return response.json();
       })
       .then((data)=>{
           console.log('Результаты фильтрации',data);
           
           dataDisplay(data);
       })
       .catch((error)=>{
           console.error('Ошибка:',error);
       });
});
function dataDisplay(data){
    const productList=document.querySelector('.list-products')
    productList.innerHTML='';
    if(!data || data.length ===0){
        const noProductsMessage=`<h2>По данному фильтру нет товаров</h2>>`;
        productList.innerHTML=noProductsMessage;
    }
    else {
        data.forEach((product)=>{
            const productItem=`
            <div class="list-products">
                        <div class="product-item">
                            <img src="${product.pathImage}" class="item-product-img" />
                            <div class="item-info">
                                <h2>${product.Name}</h2>
                                <h2 class="validPer">На ${product.ValidityPeriod} дней</h2>
                            </div>
                            <table>
                                <tbody>
                                <tr>
                                    <td>Цена:</td>
                                </tr>
                                <tr>
                                    <td class="price">${product.Price}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    }
            </div>`;
            productList.innerHTML +=productItem;
        })
    }
}