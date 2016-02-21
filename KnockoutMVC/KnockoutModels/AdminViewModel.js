
function Admin() {

    self.cbxInventory = ko.observable();

    self.cbxInventory.subscribe(function (data) {
 
        if (data) {
            ViewModel.inventoryViewModel.getInventory();
        }
        else
        {
            ViewModel.inventoryViewModel.clearInventory();
            ViewModel.oEvent() != null ? $("#TbDetail").click() : $("#TbEvent").click();
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
            ViewModel.oEvent() != null ? $("#TbDetail").click() :  $("#TbEvent").click(); 
            
        }
    });

    self.cbxNewEvent = ko.observable();

    self.cbxNewEvent.subscribe(function (data) {
        if (data) {
            $('#TbNewEvent').click();
        }
        else {
            ViewModel.oEvent() != null ? $("#TbDetail").click() : $("#TbEvent").click();
        }
    });

    self.cbxEditEvent = ko.observable();

    self.cbxEditEvent.subscribe(function (data) {
        if (data) {
            $('#TbEditEvent').click();
        }
        else {
            $("#TbDetail").click();
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
            $("#TbDetail").click();
        }
    });

    self.cbxPrint = ko.observable();

    self.cbxPrint.subscribe(function (data) {
        if (data) {
            $('#TbPrint').click();
            ViewModel.loadListViewModel.getEventLoad(ViewModel.oEvent);
        }
        else {
            $("#TbDetail").click();
        }
    });

    self.cbxNotes = ko.observable();

    self.cbxNotes.subscribe(function (data) {
        if (data) {
            $('#TbEventNotes').click();
        }
        else {
            $("#TbDetail").click();
        }
    });

    self.cbxDetail = ko.observable();

    self.cbxDetail.subscribe(function () {
            $('#TbDetail').click();
    });

    self.cbxEvent = ko.observable();

    self.cbxEvent.subscribe(function (data) {

            $('#TbEvent').click();

    });



}
