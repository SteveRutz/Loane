
// use as register student views view model
function Event(id, eventDate, eventName, available, orderCnt) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    if (eventDate == null) { eventDate = new Date();}
    self.Id = ko.observable(id);
    self.EventDate = ko.observable(eventDate);
    self.EventName = ko.observable(eventName);
    self.InvAvl = ko.observable(available);
    self.OrderCount = ko.observable(orderCnt)

    self.addEvent = function () {

        var dataObject = ko.toJSON(this);

        $.ajax({
            url: (path + 'api/event'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (data) {
                    
                ViewModel.eventListViewModel.events.push(new Event(data.id, data.eventDate, data.eventName, data.available, data.OrderCnt));

                self.Id(null);
                self.EventName('');

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

                self.events.push(new Event(value.id, value.eventDate, value.eventName, value.available, value.OrderCnt));
                $('body').css('cursor', 'default');

            })
        }
        );
    };

    // remove Event
    self.removeEvent = function (Event) {

        var txt;
        var r = confirm("Ok to delete '"+Event.EventName()+"'?");
        if (r == true) {
            txt = "You pressed OK!";
            //alert(txt);

        } else {
            txt = "You pressed Cancel!";
            //alert(txt);
            return;

        }

        $.ajax({
            url: (path + 'api/event/' + Event.Id()),
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

// remove computed field from JSON data which server is not expecting
//delete dataObject.FullName;

// create computed field by combining first name and last name
/*
self.FullName = ko.computed(function () {
    return self.FirstName() + " " + self.LastName();
}, self);

self.Age = ko.observable(age);
self.Description = ko.observable(description);
self.Gender = ko.observable(gender);

// Non-editable catalog data - should come from the server
self.genders = [
    "Male",
    "Female",
    "Other"
];
*/


/*
self.getEvents = function () {

    self.events.removeAll();

    $('body').css('cursor', 'wait');

    // retrieve students list from server side and push each object to model's students list
    $.getJSON(path + 'api/event', function (data) {
        $.each(data, function (key, value) {

            self.events.push(new Event(value.id, value.eventDate, value.eventName, value.available, value.OrderCnt));
            $('body').css('cursor', 'default');

        });
    });
};
*/


