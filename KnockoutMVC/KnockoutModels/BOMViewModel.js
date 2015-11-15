
// use as register views view model
function Part(Id, Master, Part, Qty) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    self.id = ko.observable(Id);
    self.master = ko.observable(Master);
    self.part = ko.observable(Part);
    self.qty = ko.observable(Qty);

}


// use as list view's view model
function PartList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.BOM = ko.observableArray([]);

    self.getBOM = function () {

        self.BOM.removeAll();

        $("#tbBOM").click();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(path + 'api/bom', function (data) {
            $.each(data, function (key, value) {

                self.BOM.push(new Part(value.id, value.item, value.component, value.qty));
                
            })

            $('body').css('cursor', 'default');

        }
        );
    };

    self.saveBOM = function () {

        var dataObject = ko.toJSON(self.Inventory);

        $.ajax({
            url: (path + 'api/bom'),
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

    self.addPart = function () {

        var dataObject = ko.toJSON(new Part(0, "Master Item", "Component Part", 0));

        $.ajax({
            url: (path + 'api/BOM'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (value) {

                 self.BOM.push(new Item(value.id, value.master, value.item, value.qty));

            }

            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });

        $('body').css('cursor', 'default');

    };

    // remove Event
    self.removePart = function (Item) {

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
            url: (path + 'api/bom/' + Item.id() + '/delete'),
            type: 'get', //'delete'
            contentType: 'application/json',
            success: function () {
                self.BOM.remove(Item);
            }

           , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
    };

}

