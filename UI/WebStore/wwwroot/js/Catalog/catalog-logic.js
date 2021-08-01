
Catalog = {
    _properties: {
        getProductPartialView: ""
    },
    
    init: properties => {
        $.extend(Catalog._properties, properties);

        $(".pagination li a").click(Catalog.clickOnPagination);
    },

    clickOnPagination: e => {
        e.preventDefault();

        let button = this;
        Catalog.showToolTip(button, "123");

        alert("1");
    },
    
    showToolTip: function(button, message) {
        button.tooltip({ title: message }).tooltip("show");
        setTimeout(function() {
            button.tooltip("destroy");
        }, 1000);
    },
}