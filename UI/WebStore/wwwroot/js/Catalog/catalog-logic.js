
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

        let button = $(this);

        if (button.prop("href").length > 0) {
            let container = $("#catalog-items-container");
            let data = button.data;
            let query = "";

            container.LoadingOverlay("show", { fade: [300, 200] });

            for (let key in data) {
                if (data.hasOwnProperty(key))
                    query += `${key}=${data[key]}&`;
            }

            $.get(Catalog._properties.getProductPartialViewLink + "?" + query)
                .done(catalogHtml => {
                    container.html(catalogHtml);
                    container.LoadingOverlay("hide");


                })
                .fail(() => {
                    container.LoadingOverlay("hide");
                    console.log("clickOnPagination fail");
                });

        }
    }
}