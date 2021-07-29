
Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: ""
    },

    init: function(properties) {
        $.extend(Cart._properties, properties);

        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_up").click(Cart.addItemInCart);
    },

    addToCart: function(event) {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function(text) {
                Cart.refreshCartView();
                Cart.showToolTip(button, text.message);
            }).fail(function() {
                console.log("addToCart fail");
            });
    },

    addItemInCart: function (event) {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        let tr = button.closest("tr");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function (text) {
                let count = parseInt($(".cart_quantity_input", tr).val());
                $(".cart_quantity_input", tr).val(count + 1);
                Cart.refreshPrice(tr);
                Cart.refreshCartView();
                Cart.showToolTip(button, text.message);
            }).fail(function () {
                console.log("addItemInCart fail");
            });
    },

    showToolTip: function(button, message) {
        button.tooltip({ title: message }).tooltip("show");
        setTimeout(function() {
            button.tooltip("destroy");
        }, 1000);
    },

    refreshCartView: function() {
        let container = $("#cart-container");
        $.get(Cart._properties.getCartViewLink)
            .done(function(cartHtml) {
                container.html(cartHtml);
            }).fail(function() {
                console.log("refreshCartView fail");
            });
    },

    refreshPrice: function (tr) {
        let count = parseInt($(".cart_quantity_input", tr).val());
        let price = parseFloat($(".cart_price", tr).data("price"));

        let sumPrice = count * price;
        let value = sumPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });

        let cartTotalPrice = $(".cart_total_price", tr);
        cartTotalPrice.data("price", sumPrice);
        cartTotalPrice.html(value);

        Cart.refreshTotalPrice();
    },

    refreshTotalPrice: function () {
        let totalPrice = 0;

        $(".cart_total_price").each(function () {
            let price = parseFloat($(this).data("price"));
            totalPrice += price;
        });
        let value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });

        $("#pre-total-order-price").html(value);
        $("#total-order-price").html(value);
    }
}