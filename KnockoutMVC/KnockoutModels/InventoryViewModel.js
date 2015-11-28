
// use as register views view model
function Item(id, master, item, qty, bomQty) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    self.id = ko.observable(id);
    self.master = ko.observable(master);
    self.item = ko.observable(item);
    self.qty = ko.observable(qty);
    self.bomQty = ko.observable(bomQty)
}

// use as list view's view model
function InventoryList() {

    var self = this;

    self.MasterItems = ko.observableArray([]);

    self.bomMaster = ko.observable();

    self.bomMaster.subscribe(function (data) {
        alert(data);
        //console.log(data);
        self.getInventory(data);
    });

    $.getJSON(path + 'api/bom', function (data) {
        $.each(data, function (key, value) {

            self.MasterItems.push(value.item);

        })

        $('body').css('cursor', 'default');
    });

    // observable arrays are update binding elements upon array changes
    self.Inventory = ko.observableArray([]);

    self.getInventory = function (masterItem) {

        self.Inventory.removeAll();

        $("#TbInventory").click();

        $('body').css('cursor', 'wait');

        var myPath = path + 'api/inventory';
        if (masterItem[0] != null) { myPath = myPath + "/" + masterItem; }

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(myPath, dataObject, function (data) {
            $.each(data, function (key, value) {

                    self.Inventory.push(new Item(value.id, value.master, value.item, value.qty, value.bomQty));
                
                })

                $('body').css('cursor', 'default');

            });
    };

    self.saveInventory = function () {

        var dataObject = ko.toJSON(self.Inventory);
        var MasterItem = self.MasterItems().valueOf();

        $.ajax({
            url: (path + 'api/inventory/'+ MasterItem[0]),
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

                 self.Inventory.push(new Item(value.id, value.master, value.item, value.qty, 0));

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

