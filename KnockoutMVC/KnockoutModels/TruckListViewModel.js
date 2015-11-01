
// Detail Record
function Truck(name) {

    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]
    self.truck = ko.observable(name);

}

// List Array
function TruckList() {

    var self = this;

    self.myTruck = Truck("Truck Name");

    self.removeTruck = function (truck) {

        var dataObject = ko.toJSON(truck);

        $.ajax({
            url: (path + 'api/truck/remove'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (value) {

                self.Trucks.remove(truck);

            }

, error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });


        

    };

   self.addTruck = function () {

       var dataObject = ko.toJSON(self.myTruck);

       $.ajax({
           url: (path + 'api/truck'),
           type: 'post',
           data: dataObject,
           contentType: 'application/json',
           success: function (value) {

               self.Trucks.push(value);

           }

    , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

       });
        
   };



    // observable arrays are update binding elements upon array changes
    self.Trucks = ko.observableArray([]);

        self.Trucks.removeAll();

        $("#TbTrucks").click();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
            $.getJSON(path + 'api/truck/getTruckList', function (data) {

                $.each(data, function (key, value) {
    
                    self.Trucks.push(new Truck(value));
                    
                })
    
                $('body').css('cursor', 'default');
    
            }
            )
                   /*   .done(function () {
    
                          alert("second success");
    
                      })*/
    
              .fail(function (jqXHR, exception) { errorFunction(jqXHR, exception); })
    
              .always(function () {
    
                 // alert("finished");
    
              });
                  
}

