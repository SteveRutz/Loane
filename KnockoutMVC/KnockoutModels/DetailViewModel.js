
// use as register detail views view model

function Detail(orderEvent, id, item, qty, checkout, checkin, truck, available) {

    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]
    self.orderEvent = ko.observable(orderEvent);
    self.Id = ko.observable(id);
    self.Item = ko.observable(item);
    self.orderQty = ko.observable(qty);
    self.CheckOut = ko.observable(checkout);
    self.CheckIn = ko.observable(checkin);
    self.Truck = ko.observable(truck);
    self.InvAvl = ko.observable(available);
        
    self.Truck.subscribe(function (data) { alert(data); });
    // create computed field by combining first name and last name
    /*
    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, self);
    */

    // Non-editable catalog data - should come from the server
    /*
    self.truckList = [
        "Moby",
        "Sam",
        "Other"
    ];
    */
//function Detail() {

    self.truckList = ko.observableArray([]);

    $.getJSON(path + 'api/trucklist', function (data) { self.truckList(data); });

    self.itemList = ko.observableArray([]);

    $.getJSON(path + 'api/itemlist', function (data) { self.itemList(data); });

}


// use as detail list view's view model
function DetailList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    //self.details = ko.observableArray([]);

    self.getDetails = function (Event) {
        $("#TbDetail").click();
    };

    self.addEventItem = function (event) {
        //new Detail(value.eventid, value.id, value.item, value.orderQty, value.checkout, value.checkin, value.truck, value.available

        var detail = new Detail(event, 0, "", 1, moment(ViewModel.EventDate).format('l'), moment(ViewModel.EventDate).format('l'), "", 0)

        detail.Truck.subscribe(function (data) { alert(data); });

        var dataObject = ko.toJSON(detail);

        $.ajax({
            url: (path + 'api/detail'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (value){

                //event.orderList.push(new Detail(event, value.id, value.item, value.orderQty, value.checkout, value.checkin, value.truck, value.available));
                event.orderList.push(value);
                event.orderCount(event.orderList().length);

            }

            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });

        $('body').css('cursor', 'default');

    };


    self.saveAll = function () {

        var dataObject = ko.toJSON(ViewModel.oEvent);

            $.ajax({
                url: (path + 'api/itemlist'),
                type: 'post',
                data: dataObject,
                contentType: 'application/json',
                success: function (data) {

                    alert(data);

                }
                , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

            });


    };


    // remove detail. current data context object is passed to function automatically.
    // Can this be deleted? removed?
    self.removeDetail = function (detail, event) {

        if (confirm("Remove Item2? Not Removable.")) {

            event.orderList.remove(detail);
            event.orderCount(event.orderList().length);

            self.saveAll();
        }


    };

}




