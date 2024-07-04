jQuery(document).ready(function ($) {
    "use strict";
  
  
    var header = $('.header');
    var topNav = $('.top_nav')
    var menuActive = false;
  
    setHeader();
  
    $(window).on('resize', function()
    {
        setHeader();
    });
  
    initQuantity();
    initStarRating();
    initFavorite();
    initTabs();
  
    /* 

    2. Set Header

    */
  
    function setHeader()
    {
        if(window.innerWidth < 992)
        {
            if($(window).scrollTop() > 100)
            {
                header.css({'top':"0"});
            }
            else
            {
                header.css({'top':"0"});
            }
        }
        else
        {
            if($(window).scrollTop() > 100)
            {
                header.css({'top':"-50px"});
            }
            else
            {
                header.css({'top':"0"});
            }
        }
        if(window.innerWidth > 991 && menuActive)
        {
            closeMenu();
        }
    }
  
    /* 

    5. Tạo Quantity

    */
  
    function initQuantity()
    {
        if($('.plus').length && $('.minus').length)
        {
            var plus = $('.plus');
            var minus = $('.minus');
            var value = $('#quantity_value');
  
            plus.on('click', function()
            {
                var x = parseInt(value.text());
                value.text(x + 1);
            });
  
            minus.on('click', function()
            {
                var x = parseInt(value.text());
                if(x > 1)
                {
                    value.text(x - 1);
                }
            });
        }
    }
  
    /* 

    6. Tạo đánh giá Star

    */
  
    function initStarRating()
    {
        if($('.user_star_rating li').length)
        {
            var stars = $('.user_star_rating li');
  
            stars.each(function()
            {
                var star = $(this);
  
                star.on('click', function()
                {
                    var i = star.index();
  
                    stars.find('i').each(function()
                    {
                        $(this).removeClass('fa-star');
                        $(this).addClass('fa-star-o');
                    });
                    for(var x = 0; x <= i; x++)
                    {
                        $(stars[x]).find('i').removeClass('fa-star-o');
                        $(stars[x]).find('i').addClass('fa-star');
                    };
                });
            });
        }
    }
  
    /* 

    7. Tạo Favorite

    */
    function initFavorite()
    {
        if($('.product_favorite').length)
        {
            var fav = $('.product_favorite');
  
            fav.on('click', function()
            {
                fav.toggleClass('active');
            });
        }
    }
  
    /* 

    8. Tạo Tabs

    */
  
    function initTabs()
    {
        if($('.tabs').length)
        {
            var tabs = $('.tabs li');
            var tabContainers = $('.tab_container');
  
            tabs.each(function()
            {
                var tab = $(this);
                var tab_id = tab.data('active-tab');
  
                tab.on('click', function()
                {
                    if(!tab.hasClass('active'))
                    {
                        tabs.removeClass('active');
                        tabContainers.removeClass('active');
                        tab.addClass('active');
                        $('#' + tab_id).addClass('active');
                    }
                });
            });
        }
    }
  
    //thêm giỏ hàng
    var numOfItems = 0; // số lượng sản phẩm trong giỏ hàng ban đầu là 0
    var checkoutItems = $('#checkout_items'); // chọn phần tử hiển thị số lượng sản phẩm trong giỏ hàng
    var btnAddToCart = $('#btnAddToCart'); // chọn nút thêm sản phẩm vào giỏ hàng
    var cart = $('.checkout'); // chọn phần tử giỏ hàng
    var currentProduct = "";
    var currentProductPrice = "";
  
    window.onload = function() {
        var cartString = localStorage.getItem('cart');
        if (cartString) {
            var cartItems = JSON.parse(cartString);
            for (var i = 0; i < cartItems.length; i++) {
                var item = cartItems[i];
                numOfItems += parseInt(item.quantity);
            }
            // Hiển thị số lượng sản phẩm trong giỏ hàng ban đầu
            checkoutItems.text(numOfItems);
        }
    }
  
    btnAddToCart.on('click', function () {
        // Kiểm tra xem người dùng đã chọn dung tích hay chưa
        let selectedSize = document.querySelector('.size span.selected');
        if (!selectedSize) {
            ;
        } else {
			  
            // Thêm sản phẩm vào giỏ hàng
            // Lấy đối tượng hình ảnh sản phẩm
            var imgtodrag = $('#flyToCart');
  
            // Nếu có hình ảnh sản phẩm
            if (imgtodrag) {
                // Clone hình ảnh sản phẩm và di chuyển nó đến giỏ hàng
                var imgclone = imgtodrag.clone()
                    .offset({
                        top: imgtodrag.offset().top,
                        left: imgtodrag.offset().left
                    })
                    .css({
                        'opacity': '0.5',
                        'position': 'absolute',
                        'height': '200px',
                        'width': '200px',
                        'z-index': '100'
                    })
                    .appendTo($('body'))
                    .animate({'top': cart.offset().top + 10,
                        'left': cart.offset().left + 10,
                        'width': 75,
                        'height': 75
                    }, 2000, 'easeInOutExpo');
  
                // Sau 1.5s, thực hiện các hành động sau
                setTimeout(function () {
                    // Lắc giỏ hàng để tạo hiệu ứng
                    cart.effect("shake", {
                        times: 1
                    }, 200);
  
                    // Lấy số lượng sản phẩm được chọn từ phần tử hiển thị
                    var quantity = parseInt($('#quantity_value').text());
  
                    // Tăng số lượng sản phẩm trong giỏ hàng
                    numOfItems = numOfItems + quantity;
                    // Hiển thị số lượng sản phẩm trong giỏ hàng sau khi tăng
                    checkoutItems.html(numOfItems);
					  
                    // Lấy danh sách sản phẩm hiện tại từ localStorage
                    let cartItems = JSON.parse(localStorage.getItem('cart')) || [];
  
                    // Kiểm tra xem sản phẩm mới có trùng với một sản phẩm đã có trong giỏ hàng hay không
                    let existingItem = cartItems.find(item => item.title === currentProduct);
  
                    if (existingItem) {
                        // Nếu sản phẩm mới trùng với một sản phẩm đã có trong giỏ hàng, tăng số lượng của sản phẩm đó lên
                        existingItem.quantity += quantity;
                    } else {
                        // Nếu sản phẩm mới không trùng với bất kỳ sản phẩm nào trong giỏ hàng, thêm nó vào danh sách
                        let newItem = {title: currentProduct, price: currentProductPrice, imgSrc: 'images/big.png', quantity: quantity};
                        cartItems.push(newItem);
                    }
  
                    // Lưu lại danh sách sản phẩm vào localStorage
                    localStorage.setItem('cart', JSON.stringify(cartItems));
                }, 1500);
  
                // Khi hình ảnh sản phẩm di chuyển đến giỏ hàng xong, ẩn nó đi
                imgclone.animate({
                    'width': 0,
                    'height': 0
                }, function () {
                    $(this).detach()
                });
            }
        }
    });
  
  
    //===============================================================================================
    //khi di chuột vào hình nhỏ nó sẽ hiện lên khung hình lớn
	  
    // Lấy tất cả các hình ảnh nhỏ trong product-content-left-small-img
    const smallImages = document.querySelectorAll('.product-content-left-small-img img');
    // Lấy hình ảnh lớn trong product-content-left-big-img
    const bigImage = document.querySelector('.product-content-left-big-img img');
    // Lưu giá trị src ban đầu của hình ảnh lớn
    const originalBigImageSrc = bigImage.src;
  
    // Duyệt qua mỗi hình ảnh nhỏ
    smallImages.forEach(image => {
        // Thêm sự kiện mouseover cho mỗi hình ảnh nhỏ
        image.addEventListener('mouseover', () => {
            // Khi di chuột vào hình ảnh nhỏ, thay đổi src của hình ảnh lớn thành src của hình ảnh nhỏ
            bigImage.src = image.src;
});
// Thêm sự kiện mouseout cho mỗi hình ảnh nhỏ
image.addEventListener('mouseout', () => {
    // Khi di chuột ra khỏi hình ảnh nhỏ, thay đổi src của hình ảnh lớn trở lại giá trị ban đầu
    bigImage.src = originalBigImageSrc;
});
});
  
//=================================================================================
// thêm bình luận mới vào
document.getElementById("review_form").addEventListener("submit", function(event) {
    //Ngăn chặn việc gửi form thông qua phương thức mặc định
    event.preventDefault();
    //Lấy thông tin
    var name = document.getElementById("review_name").value;
    var email = document.getElementById("review_email").value;
    var message = document.getElementById("review_message").value;
	  
    //Tạo phần đánh giá mới
    var newReview = document.createElement("div");
    //Thêm các class cho phần tử đánh giá
    newReview.classList.add("user_review_container");
    newReview.classList.add("d-flex");
    newReview.classList.add("flex-column");
    newReview.classList.add("flex-sm-row");
	  
    //Tạo phần tử chứa thông tin người dùng (userDiv)
    var userDiv = document.createElement("div");
    userDiv.classList.add("user");
    //Tạo phần tử chứa ảnh đại diện của người dùng (userPicDiv)
    var userPicDiv = document.createElement("div");
    userPicDiv.classList.add("user_pic");
    userDiv.appendChild(userPicDiv);
	  
    //Tạo phần tử chứa đánh giá sao của người dùng (ratingDiv)
    var ratingDiv = document.createElement("div");
    ratingDiv.classList.add("user_rating");
    // Tạo phần tử ul để chứa các sao
    var ratingUl = document.createElement("ul");
    ratingUl.classList.add("star_rating");
	  
    // Lấy số sao đánh giá từ form
    var stars = document.querySelectorAll(".user_star_rating li i");
    for (var i = 0; i < stars.length; i++) {
        // Tạo phần tử li để chứa từng sao
        var starLi = document.createElement("li");
        // Tạo phần tử i để chứa biểu tượng sao
        var starI = document.createElement("i");
        // Sao chưa được đánh giá sẽ có class rỗng, sao đã đánh giá sẽ có class "fa fa-star"
        starI.className = stars[i].className;
        starI.setAttribute("aria-hidden", "true");
        starLi.appendChild(starI);
        ratingUl.appendChild(starLi);
    }
	  
    // Thêm phần tử chứa đánh giá sao vào phần tử chứa thông tin người dùng (userDiv)
    ratingDiv.appendChild(ratingUl);
    userDiv.appendChild(ratingDiv);
  
    // Tạo phần tử chứa nội dung đánh giá (reviewDiv)
    var reviewDiv = document.createElement("div");
    reviewDiv.classList.add("review");
	  
    // Tạo phần tử chứa ngày đăng đánh giá (dateDiv)
    var dateDiv = document.createElement("div");
    dateDiv.classList.add("review_date");
	  
    // Lấy ngày hiện tại
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    // Định dạng ngày tháng năm hiện tại
    today = dd + ' ' + mm + ' ' + yyyy;
    // Đặt nội dung của phần tử dateDiv là ngày tháng năm hiện tại
    dateDiv.innerHTML = today;
    // Thêm phần tử dateDiv vào phần tử reviewDiv
    reviewDiv.appendChild(dateDiv);
	  
    // Tạo một phần tử div mới để chứa tên người dùng
    var nameDiv = document.createElement("div");
    // Thêm class "user_name" cho phần tử nameDiv
    nameDiv.classList.add("user_name");
    // Đặt nội dung của phần tử nameDiv là tên người dùng
    nameDiv.innerHTML = name;
    // Thêm phần tử nameDiv vào phần tử reviewDiv
    reviewDiv.appendChild(nameDiv);
	  
    // Tạo một phần tử p mới để chứa nội dung bình luận
    var messageP = document.createElement("p");
    // Đặt nội dung của phần tử messageP là nội dung bình luận
    messageP.innerHTML = message;
    // Thêm phần tử messageP vào phần tử reviewDiv
    reviewDiv.appendChild(messageP);
	  
    // Thêm các phần tử userDiv và reviewDiv vào phần tử newReview
    newReview.appendChild(userDiv);
    newReview.appendChild(reviewDiv);
	  
    // Tìm phần tử chứa danh sách các bình luận hiện có trên trang web
    var reviewsContainer = document.querySelector(".reviews_col");
    // Thêm phần tử newReview vào cuối danh sách các bình luận hiện có trên trang web
    reviewsContainer.appendChild(newReview);
});
  
//=============================================================================
  



  

//================================================================================
document.getElementById("btnZaloChat").onclick = function() 
{
    // Tạo một phần tử img và đặt thuộc tính src cho nó
    var img = document.createElement("img");
    img.src = "images/LienHe.jpg";
    img.style.display = "block";
    img.style.margin = "auto";
    img.style.width = "40%";
    img.style.marginTop = "80px";
		
    // Tạo một phần tử div và đặt thuộc tính style cho nó để làm mờ trang hiện có
    var overlay = document.createElement("div");
    overlay.style.position = "fixed";
    overlay.style.top = 0;
    overlay.style.left = 0;
    overlay.style.width = "100%";
    overlay.style.height = "100%";
    overlay.style.backgroundColor = "rgba(0,0,0,0.5)";
    overlay.style.zIndex = 9999;

    // Thêm sự kiện onclick cho phần tử overlay
    overlay.onclick = function() {
        document.body.removeChild(overlay);
    }
		
    // Thêm phần tử img và div vào trang
    overlay.appendChild(img);
    document.body.appendChild(overlay);
}

});
