app.factory('cartService', function () {
    let cart;
    let cartCount=0;

    let updateCart = function(cartObj, cartCountObj){
        cart = cartObj;
        cartCount = cartCountObj;
    };

    let getCart = function(){
        return cart;
    }

    let getCartCount = function(){
        return cartCount;
    }

    return {
        updateCart: updateCart,
        getCart: getCart,
        getCartCount:getCartCount
    };

});