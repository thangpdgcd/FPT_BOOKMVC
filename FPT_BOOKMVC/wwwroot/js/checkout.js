 clickcheckout=()=>
 {
    
         $('.js-order-detail').each(function () {
             var nameProduct = $(this).parent().parent().parent().parent().html();
             $(this).on('click', function () {
                 swal("Successfully!", "You will receive your order after 4 days!", "success");
             });
         });
        
         $('.js-order-detail').each(function () {
             var nameProduct = $(this).parent().parent().parent().parent().html();
             $(this).on('click', function () {
                 swal("Error!");
             });
         });
      
   
     
}