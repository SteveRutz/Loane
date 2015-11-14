
// use as register detail views view model

function TruckLoad(truck, eventName, eventDate, checkOut, Avl, component, LoadQty)
{

    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]
    self.Truck = ko.observable(truck);
    self.EventName = ko.observable(eventName);
    self.EventDate = ko.observable(eventDate);
    self.CheckOut = ko.observable(checkOut);
    self.Available = ko.observable(Avl);
    self.Component = ko.observable(component);
    self.LoadQuantity = ko.observable(LoadQty);

}

// use as detail list view's view model
function LoadList() {

   // if (checkOut == null && Truck == null) { return;}

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.Load = ko.observableArray([]);

    self.firstRecord = ko.observable(new TruckLoad('', '', '', '', '', ''));

    self.getEventLoad = function (event) {

        self.Load.removeAll();

        dataObject = ko.toJSON(event);

        $.ajax({
            url: (path + 'api/loadlist/event'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (data) {

                //alert(data);

                $.each(data, function (key, value) {
                    self.Load.push(new TruckLoad(value.truck, value.eventName, new Date(value.eventDate).toLocaleDateString(), value.checkOut, value.Avl, value.component, value.LoadQty));

                });

                try {
                    self.firstRecord(self.Load()[1]);
                }
                catch (e) { }

            }
            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });

    };

    self.getLoad = function (detail){

        try{
            if (detail.truck!=null) { ViewModel.LoadTruck(detail.truck);}
        } catch (e) { }

        detail.dateEnd = ViewModel.LoadDateEnd();
        detail.dateBegin = ViewModel.LoadDateBegin();
        detail.truck = ViewModel.LoadTruck();

        self.Load.removeAll();

        dataObject = ko.toJSON(detail);

        $.ajax({
            url: (path + 'api/loadlist'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (data) {

                //alert(data);

                $.each(data, function (key, value) {
                    self.Load.push(new TruckLoad(value.truck, value.eventName, new Date(value.eventDate).toLocaleDateString(), value.checkOut, value.Avl, value.component, value.LoadQty));

                });

                try {
                    self.firstRecord(self.Load()[1]);
                }
                catch (e) { }

                $("#TbLoadList").click();



            }
            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
       /*
        // retrieve details list from server side and push each object to model's details list
        $.getJSON(path + 'api/loadlist?LoadDateEnd=' + moment(ViewModel.LoadDateEnd()).format("MM/DD/YYYY") + '&LoadDateBegin=' + moment(ViewModel.LoadDateBegin()).format("MM/DD/YYYY") + '&Truck=' + ViewModel.LoadTruck(), function (data) {

            $.each(data, function (key, value) {
                self.Load.push(new TruckLoad(value.truck, value.eventName, value.eventDate, value.checkOut, value.Avl, value.component, value.LoadQty));
            });

            try{
                self.firstRecord(self.Load()[1]);
            }
            catch (e) { }
            
            $("#TbLoadList").click();

        });
            */
    };


}




