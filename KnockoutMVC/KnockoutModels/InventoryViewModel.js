
// use as register views view model
function Item(id, master, item, qty) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    self.id = ko.observable(id);
    self.master = ko.observable(master);
    self.item = ko.observable(item);
    self.qty = ko.observable(qty);

}


// use as list view's view model
function InventoryList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.Inventory = ko.observableArray([]);

    self.getInventory = function () {

        self.Inventory.removeAll();

        $("#TbInventory").click();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(path + 'api/inventory', function (data) {
            $.each(data, function (key, value) {

                self.Inventory.push(new Item(value.id, value.master, value.item, value.qty));
                
            })

            $('body').css('cursor', 'default');

        }
        );
    };

    self.saveInventory = function () {

        var dataObject = ko.toJSON(self.Inventory);

        $.ajax({
            url: (path + 'api/inventory'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (data) {

                /*
                ViewModel.inventoryViewModel.Inventory.push(new Item(data.id, data.master, data.item, data.qty));

                self.Id(null);
                self.Item('');

                */
                alert(data);

                

                $("#TbDetailDE").click();

            }
            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });


    };

    self.addInventoryItem = function () {

        var dataObject = ko.toJSON(new Item(0, false, "Added Item", 0));

        $.ajax({
            url: (path + 'api/inventoryitem'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (value) {

                 self.Inventory.push(new Item(value.id, value.master, value.item, value.qty));

            }

            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });

        $('body').css('cursor', 'default');

    };

    // remove Event
    self.removeInventoryItem = function (Item) {

        var txt;
        var r = confirm("Ok to delete '"+ Item.item() + ", qty: " + Item.qty() + "'?");
        if (r == true) {
            txt = "You pressed OK!";
            //alert(txt);

        } else {
            txt = "You pressed Cancel!";
            //alert(txt);
            return;

        }

        $.ajax({
            url: (path + 'api/inventory/' + Item.id()),
            type: 'get', //'delete'
            contentType: 'application/json',
            success: function () {
                self.Inventory.remove(Item);
            }

           , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
    };

}

