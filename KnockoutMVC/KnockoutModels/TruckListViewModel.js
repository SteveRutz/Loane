
// Detail Record
function Truck(id, name) {

    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]
    self.id = ko.observable(id);
    self.truck = ko.observable(name);

}


// List Array
function TruckList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.Trucks = ko.observableArray([]);

    self.getTrucks = function () {

        self.Trucks.removeAll();

        $("#TbTrucks").click();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(path + 'api/truck', function (data) {
            $.each(data, function (key, value) {

                self.Trucks.push(new Truck(value.id, value.name));
                
            })

            $('body').css('cursor', 'default');

        }
        );
    };

    self.save = function () {

        var dataObject = ko.toJSON(self.Trucks);

        $.ajax({
            url: (path + 'api/truck'),
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
    self.removeTruck = function (truck) {

        var txt;
        var r = confirm("Ok to delete '"+ truck.name() + "'?");
        if (r == true) {
            txt = "You pressed OK!";
            //alert(txt);

        } else {
            txt = "You pressed Cancel!";
            //alert(txt);
            return;

        }

        $.ajax({
            url: (path + 'api/truck/' + truck.id()),
            type: 'get', //'delete'
            contentType: 'application/json',
            success: function () {
                self.Inventory.remove(truck);
            }

           , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
    };

}

