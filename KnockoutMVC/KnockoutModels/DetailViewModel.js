
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

        return; 

        if (Event == null) { return; }
        try{if (Event.orderList() == null) { return;}
        } catch (e) { return;}
        
        //self.details.removeAll();
        //alert("get Details");

        /*
        $.each(Event.eventItems(), function(key,value){
            self.details.push(new Detail(Event, value.id, value.item, value.orderQty, moment(value.checkout).format('l'), moment(value.checkin).format('l'), value.truck, value.available));

        });
        */

        // event Items is the observable list of orders!
        //self.details = Event.eventItems;

        $("#TbDetail").click();
       
        // retrieve details list from server side and push each object to model's details list

        /*
        $.getJSON(path + 'api/detail/' + ViewModel.EventID(), function (data) {

            $.each(data, function (key, value) {
                self.details.push(new Detail(ViewModel.oEvent, value.id, value.item, value.orderQty, moment(value.checkout).format('l'), moment(value.checkin).format('l'), value.truck, value.available));
            });

            $("#TbDetail").click();

        });
        */
    };

    self.addEventItem = function (event) {
        //new Detail(value.eventid, value.id, value.item, value.orderQty, value.checkout, value.checkin, value.truck, value.available

        var detail = new Detail(event, 0, "", 1, moment(ViewModel.EventDate).format('l'), moment(ViewModel.EventDate).format('l'), "", 0)

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


        /*
        self.details().forEach(function (dtl) {

            dtl.CheckIn = moment(dtl.CheckIn()).format("MM-dd-YYYY");
            dtl.CheckOut = moment(dtl.CheckOut()).format("MM-dd-YYYY");;

        });
        */
        /*
        $.each(self.details, function (key, value) {

            value.orderEvent() = ViewModel.oEvent;                

        });*/
        
        //var dataObject = ko.toJSON(self.details);

        /*
        ViewModel.oEvent().orderList().forEach(function (dtl) {
            dtl.orderEvent = null;
        });
        */
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

    self.removeDetail = function (detail, event) {
        
        event.orderList.remove(detail);
        event.orderCount(event.orderList().length);

        self.saveAll();

        /*
        $.ajax({
            url: path + 'api/detail/' + detail.Id() + '/delete',
            type: 'get',
            contentType: 'application/json',
            success: function () {

                self.details.remove(detail);

            }
        });
        */

    };

}




