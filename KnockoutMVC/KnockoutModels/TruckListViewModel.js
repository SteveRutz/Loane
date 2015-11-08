
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

    // observable arrays are update binding elements upon array changes
    self.Trucks = ko.observableArray([]);

        self.Trucks.removeAll();

        $("#TbTrucks").click();

        $('body').css('cursor', 'wait');

        // retrieve students list from server side and push each object to model's students list
            $.getJSON(path + 'api/trucklist/getTruckList', function (data) {

                $.each(data, function (idx, value) { self.Trucks.push(new Truck(value)); });
                        
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

            self.addTruck = function () {

                $.ajax({
                    url: path + 'api/truck/'+self.myTruck+'/add'
                    , method: "post"
                    , cache: false
                    , async: "true"
                    , success: function (value) {

                           // alert(value);
                            self.Trucks.push(new Truck(value));

                        }
                    , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }
                });

            }

            self.removeTruck = function (truck) {

                var dataObject = ko.toJSON(truck);

                $.ajax({
                    url: path + 'api/truck/' + JSON.parse(dataObject).truck + '/delete',
                    type: 'post',
                    data: dataObject,
                    contentType: 'application/json',
                    success: function (value) {

                        self.Trucks.remove(truck);

                    }

                   , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

                });




            };

                  
}







