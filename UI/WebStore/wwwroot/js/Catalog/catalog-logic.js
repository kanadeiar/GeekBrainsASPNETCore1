
Catalog = {
    _properties: {
        getProductPartialViewLink: "",
        getCatalogPaginationPartialViewLink: ""
    },
    
    init: properties => {
        $.extend(Catalog._properties, properties);

        $(".pagination li a").click(Catalog.clickOnPagination);
    },

    clickOnPagination: function(e) {
        e.preventDefault();

        const button = $(this);

        if (button.prop("href").length > 0) {
            let container = $("#catalog-items-container");
           
            container.LoadingOverlay("show", { fade: [300, 200] });

            let page = button.data("page");

            console.log("page = " + page);

            let query = "";

            let data = button.data();

            for (let key in data)
                if (data.hasOwnProperty(key))
                    query += `${key}=${data[key]}&`;

            console.log("query = " + query);

            $.get(Catalog._properties.getProductPartialViewLink + "?" + query)
                .done(catalogHtml => {
                    container.html(catalogHtml);
                    $(".add-to-cart").click(Cart.addToCart);
                    container.LoadingOverlay("hide");

                    $.get(Catalog._properties.getCatalogPaginationPartialViewLink + "?" + query)
                        .done(function (paginationHtml) {
                            let pagination = $("#catalog-pagination-container");
                            pagination.html(paginationHtml);
                            $(".pagination li a").click(Catalog.clickOnPagination);
                        }).fail(function () {
                            console.log("getCatalogPagination fail");
                        });

                })
                .fail(() => {
                    container.LoadingOverlay("hide");
                    console.log("clickOnPagination fail");
                });

        }
    }
}