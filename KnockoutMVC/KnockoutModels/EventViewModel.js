
// use as register student views view model
function Event(id, eventDate, checkOut, checkIn, eventName, available, eventItems) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    if (eventDate == null) {
        eventDate = new Date();
        checkOut = eventDate;
        checkIn = eventDate;
    }
    self.id = ko.observable(id);
    self.eventDate = ko.observable(eventDate);
    self.checkOut = ko.observable(checkOut);
    self.checkIn = ko.observable(checkIn);
    self.eventName = ko.observable(eventName);
    self.available = ko.observable(available);
    self.orderList = ko.observableArray(eventItems);
    self.orderCount = ko.observable(self.orderList().length);

    self.removeDetail = function (order) {
        alert('removeDetail');

        $.ajax({
            url: path + 'api/detail/' + order.id + '/delete',
            type: 'get',
            contentType: 'application/json',
            success: function () {

                self.orderList.remove(order);

            }
        });

    };

    self.save = function (event) {

        var dataObject = ko.toJSON(event);

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



    self.addEvent = function () {

        var dataObject = ko.toJSON(this);

        $.ajax({
            url: (path + 'api/event'),
            type: 'post',
            data: dataObject, 
            contentType: 'application/json',
            success: function (data) {
                    
                ViewModel.eventListViewModel.events.push(new Event(data.id, data.eventDate, data.checkOut, data.checkIn, data.eventName, data.available, data.eventOrders));

                self.id(null);
                self.eventName('');

                $("#TbEvent").click();

            }
            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }
            
        });


    };

}


// use as student list view's view model
function EventList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.events = ko.observableArray([]);

    self.getEventAsOf = function (EventAsOf) {

        if (EventAsOf==null) { return;}

        self.events.removeAll();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(path + 'api/event/asof/' + moment(EventAsOf).format('MM-DD-YYYY').toString(), function (data) {
            $.each(data, function (key, value) {

                self.events.push(new Event(value.id, value.eventDate, value.checkOut, value.checkIn, value.eventName, value.available, value.orderList));
                $('body').css('cursor', 'default');

            })
        }
        );
    };



    // remove Event
    self.removeEvent = function (Event) {

        var txt;
        var r = confirm("Ok to delete '"+Event.eventName()+"'?");
        if (r == true) {
            txt = "You pressed OK!";
            //alert(txt);

        } else {
            txt = "You pressed Cancel!";
            //alert(txt);
            return;

        }

        $.ajax({
            url: (path + 'api/event/' + Event.id()),
            type: 'get', //'delete'
            contentType: 'application/json',
            success: function () {
                self.events.remove(Event);
            }

           , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
    };

}

function errorFunction(jqXHR, exception) {
    $("body").css('cursor', 'default');
    var rsp = JSON.parse(jqXHR.responseText);

    if (jqXHR.status === 0) {
        alert('Not connect.\n Verify Network.');
    } else if (jqXHR.status == 404) {
        alert('Requested page not found. [404]');
    } else if (jqXHR.status == 500) {
        if (rsp.ExceptionMessage == "could not execute update query[SQL: delete from \"events\" where id=?]") {
            alert("Delete Items from the Event prior to deleting the event itself.");
        }
        else {
            alert('Internal Server Error [500].\r\n check js parameters vs server.');
            alert(jqXHR.responseText);
        }
    } else if (exception === 'parsererror') {
        alert('Requested JSON parse failed.');
    } else if (exception === 'timeout') {
        alert('Time out error.');
    } else if (exception === 'abort') {
        alert('Ajax request aborted.');
    } else {
        alert('Uncaught Error.\n' + jqXHR.responseText);
    }

}




