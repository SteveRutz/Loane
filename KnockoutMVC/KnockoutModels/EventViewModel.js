
// use as register student views view model
function Event(id, eventDate, checkOut, checkIn, eventName, available, comments, eventItems) {
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
    self.comments = ko.observable(comments);
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

        ViewModel.LoadDateBegin(moment(event.checkOut()).format('l'));
        ViewModel.LoadDateEnd(moment(event.checkIn()).format('l'));

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
                    
                ViewModel.eventListViewModel.events.push(new Event(data.id
                    , moment(data.eventDate).format('l')
                    , moment(data.checkOut).format('l')
                    , moment(data.checkIn).format('l')
                    , data.eventName
                    , data.available
                    , data.comments
                    , data.eventOrders));

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

    self.filterEvent = ko.observable();

    // observable arrays are update binding elements upon array changes
    self.events = ko.observableArray([]);

    self.getEventAsOf = function (EventAsOf) {

        if (EventAsOf==null) { return;}

        self.events.removeAll();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
        $.getJSON(path + 'api/event/asof/' + moment(EventAsOf).format('MM-DD-YYYY').toString(), function (data) {
            $.each(data, function (key, value) {

                self.events.push(new Event(value.id
                    , moment(value.eventDate).format('l')
                    , moment(value.checkOut).format('l')
                    , moment(value.checkIn).format('l')
                    , value.eventName
                    , value.available
                    , value.comments
                    , value.orderList));

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

    self.evtHeaders = [
{ title: 'Items', sortPropertyName: 'orderCount', asc: true, active: false },
{ title: 'Event', sortPropertyName: 'eventName', asc: false, active: true},
{ title: 'Event<br/>Date', sortPropertyName: 'eventDate', asc: true, active: false },
{ title: "Check-Out", sortPropertyName: 'checkOut', asc: true, active: false },
{ title: "Check-In", sortPropertyName: 'checkIn', asc: true, active: false },
{ title: "Delete", sortPropertyName: 'eventDate', asc: true, active: false }
    ];

    //self.activeSort = ko.observable('eventName'); //set the default sort
    self.activeSort = ko.observable(function () { return 0; });
    // self.ascending = ko.observable(true);
    self.sort = function (header, event) {
        //alert('sort');
        //alert(JSON.stringify(header));
        //if this header was just clicked a second time
        if (header.active) {
            header.asc = !header.asc; //toggle the direction of the sort
        }
        //make sure all other headers are set to inactive
        ko.utils.arrayForEach(self.evtHeaders, function (item) { item.active = false; });
        //the header that was just clicked is now active
        header.active = true;//our now-active header

        var prop = header.sortPropertyName;

        if (prop == "NA") { return; }

        var ascSort = function (a, b) {
            //alert(prop);
            //alert(a[prop] < b[prop]); alert(a[prop] > b[prop]);
            //if (a[prop] < b[prop] ? -1 : a[prop] > b[prop] ? 1 : a[prop] == b[prop] ? 0 : 0 != 0) { alert("ascSort: " + prop);}

            if (prop == 'eventName') {
                return a[prop]() < b[prop]() ? -1 : a[prop]() > b[prop]() ? 1 : a[prop]() == b[prop]() ? 0 : 0;
            }
            else {
                return new Date(a[prop]()) < new Date(b[prop]()) ? -1 : new Date(a[prop]()) > new Date(b[prop]()) ? 1 : new Date(a[prop]()) == new Date(b[prop]()) ? 0 : 0;
            }
        };
        var descSort = function (a, b) {
            //alert(prop);
            //if (a[prop] < b[prop] ? -1 : a[prop] > b[prop] ? 1 : a[prop] == b[prop] ? 0 : 0 != 0) { alert("descSort: " + prop); }
            
            if (prop == 'eventName') {
                return a[prop]() > b[prop]() ? -1 : a[prop]() < b[prop]() ? 1 : a[prop]() == b[prop]() ? 0 : 0;
            } else {
                return new Date(a[prop]()) > new Date(b[prop]()) ? -1 : new Date(a[prop]()) < new Date(b[prop]()) ? 1 : new Date(a[prop]()) == new Date(b[prop]()) ? 0 : 0;
            }
        };
        var sortFunc = header.asc ? ascSort : descSort;

        self.activeSort(sortFunc);

        //store the new active sort function
        //self.activeSort(prop);
        //self.ascending(header.asc);
    };

    self.filteredEvents = ko.computed(function () {

        return ko.utils.arrayFilter(self.events(), function (rec) {
            return (
                      (self.filterEvent() == null ||
                            rec.eventName().toLowerCase().indexOf(self.filterEvent().toLowerCase()) > -1)
                        &&
                      (self.activeSort() != null)
                   )
        }).sort(self.activeSort());

    });

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




