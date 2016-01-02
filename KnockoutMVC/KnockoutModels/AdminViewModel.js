
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

    self.cbxNewEvent = ko.observable();

    self.cbxNewEvent.subscribe(function (data) {
        if (data) {
            $('#TbNewEvent').click();
        }
    });

    self.cbxEditEvent = ko.observable();

    self.cbxEditEvent.subscribe(function (data) {
        if (data) {
            $('#TbEditEvent').click();
        }
    });
    
    self.cbxLoadList = ko.observable();

    self.cbxLoadList.subscribe(function (data) {
        if(data)
        {
            ViewModel.loadListViewModel.getEventLoad(ViewModel.oEvent);
            $('#TbLoadList').click();
        }
        else {
            ViewModel.loadListViewModel.clearLoad();
        }
    });

    self.cbxPrint = ko.observable();

    self.cbxPrint.subscribe(function (data) {
        if (data) {
            $('#TbPrint').click();
            ViewModel.loadListViewModel.getEventLoad(ViewModel.oEvent);
        }
    });

    self.cbxNotes = ko.observable();

    self.cbxNotes.subscribe(function (data) {
        if (data) {
            $('#TbEventNotes').click();
        }
    });

}