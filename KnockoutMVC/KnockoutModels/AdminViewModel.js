
function Admin() {

    self.cbxInventory = ko.observable();

    self.cbxInventory.subscribe(function (data) {
 
        if (data) {
            ViewModel.inventoryViewModel.getInventory();
        }
        else
        {
            ViewModel.inventoryViewModel.clearInventory();
        }
    });

    self.cbxTrucks = ko.observable();

    self.cbxTrucks.subscribe(function (data) {

        if (data) {
            ViewModel.truckViewModel.getTrucks();
            $("#TbTruck").click();
        }
        else {
            ViewModel.truckViewModel.clearTrucks();
        }
    });




}