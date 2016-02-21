
// use as register views view model
function Item(id, master, item, qty, bomQty) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]

    self.id = ko.observable(id);
    self.master = ko.observable(master);
    self.item = ko.observable(item);
    self.qty = ko.observable(qty);
    self.bomQty = ko.observable(bomQty)
}

// use as list view's view model
function InventoryList() {

    var self = this;

    // observable arrays are update binding elements upon array changes
    self.Inventory = ko.observableArray([]);

    self.filterOrderItem = ko.observable(false);
    self.filterItem = ko.observable('');
    self.filterInvQty = ko.observable();
    self.filterBomQty = ko.observable(false);

    self.MasterItems = ko.observableArray([]);

    self.bomMaster = ko.observable();

    self.bomMaster.subscribe(function (data) {
        //alert(data);
        if (data == '-- Bills-Of-Material -- ') {
            $('#lblbomQty').html("BOM");
        }
        else {
            $('#lblbomQty').html(data);
            //console.log(data);
            self.getInventory(data);
            cbxInventory(true);
        }

    });

    $.getJSON(path + 'api/bom', function (data) {
        $.each(data, function (key, value) {

            self.MasterItems.push(value.item);

        })

        $('body').css('cursor', 'default');
    });

    self.getInventory = function (masterItem) {

        self.Inventory.removeAll();

        $("#TbInventory").click();

        $('body').css('cursor', 'wait');

        var myPath = path + 'api/inventory';

        if (masterItem != null) {
            if (masterItem[0] != null) { myPath = myPath + "/" + masterItem; }
        }

        $.getJSON(myPath, function (data) {
            $.each(data, function (key, value) {

                    self.Inventory.push(new Item(value.id, value.master, value.item, value.qty, value.bomQty));
                
                })

                $('body').css('cursor', 'default');

        });

    };

    self.clearInventory = function () {
        self.Inventory.removeAll();
    }

    self.saveInventory = function () {

        var dataObject = ko.toJSON(self.Inventory);
        var MasterItem = self.bomMaster();

        $.ajax({
            url: (path + 'api/inventory/'+ MasterItem),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (data) {

                /*
                ViewModel.inventoryViewModel.Inventory.push(new Item(data.id, data.master, data.item, data.qty));

                self.Id(null);
                self.Item('');

                */
                alert(data.Data);

                

                $("#TbDetailDE").click();

            }
            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });


    };

    self.addInventoryItem = function () {

        var dataObject = ko.toJSON(new Item(0, false, " Added Item", 0));

        $.ajax({
            url: (path + 'api/inventoryitem'),
            type: 'post',
            data: dataObject,
            contentType: 'application/json',
            success: function (value) {

                 self.Inventory.push(new Item(value.id, value.master, value.item, value.qty, 0));

            }

            , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });

        $('body').css('cursor', 'default');

    };

    // remove Event
    self.removeInventoryItem = function (Item) {

        var txt;
        var r = confirm("Ok to delete '"+ Item.item() + ", qty: " + Item.qty() + "'?");
        if (r == true) {
            txt = "You pressed OK!";
            //alert(txt);

        } else {
            txt = "You pressed Cancel!";
            //alert(txt);
            return;

        }

        $.ajax({
            url: (path + 'api/inventory/' + Item.id()),
            type: 'get', //'delete'
            contentType: 'application/json',
            success: function () {
                self.Inventory.remove(Item);
            }

           , error: function (jqXHR, exception) { errorFunction(jqXHR, exception); }

        });
    };
    
    self.headers = [
    { title: 'Order<br/>Item', sortPropertyName: 'master', asc: true, active: false },
    { title: 'Item', sortPropertyName: 'item', asc: false, active: true },
    { title: 'Inv<br/>Qty', sortPropertyName: 'qty', asc: true, active: false },
    { title: "<span id='lblbomQty' /><br/>Qty", sortPropertyName: 'bomQty', asc: true, active: false }
    /*, {
        title: '<input type="button" class="btn btn-danger btn-xs" value=" [x] " />'
        , sortPropertyName: 'NA'
        , asc: true, active: false
    } */
    ];

    self.bomHeaders = [
{ title: 'Item', sortPropertyName: 'item', asc: false, active: true },
{ title: 'Inv<br/>Qty', sortPropertyName: 'qty', asc: true, active: false },
{ title: "BOM<br/>Qty", sortPropertyName: 'bomQty', asc: true, active: false }
/*, {
    title: '<input type="button" class="btn btn-danger btn-xs" value=" [x] " />'
    , sortPropertyName: 'NA'
    , asc: true, active: false
} */
    ];

    self.delHeaders = [
{ title: 'Order<br/>Item', sortPropertyName: 'master', asc: true, active: false },
{ title: 'Item', sortPropertyName: 'item', asc: false, active: true },
{ title: 'Inv<br/>Qty', sortPropertyName: 'qty', asc: true, active: false },
{ title: "Delete", sortPropertyName: 'bomQty', asc: true, active: false }
/*, {
    title: '<input type="button" class="btn btn-danger btn-xs" value=" [x] " />'
    , sortPropertyName: 'NA'
    , asc: true, active: false
} */
    ];

    //self.activeSort = ko.observable('item'); //set the default sort
    self.activeSort = ko.observable(function () { return 0; });
   // self.ascending = ko.observable(true);
    self.sort = function (header, event) {
        //if this header was just clicked a second time
        if (header.active) {
            header.asc = !header.asc; //toggle the direction of the sort
        }
        //make sure all other headers are set to inactive
        ko.utils.arrayForEach(self.headers, function (item) { item.active = false; });
        //the header that was just clicked is now active
        header.active = true;//our now-active header

        var prop = header.sortPropertyName;
        
        if (prop == "NA") { return;}

        var ascSort = function (a, b) {
           // alert(a[prop] < b[prop]); alert(a[prop] > b[prop]);
            //if (a[prop] < b[prop] ? -1 : a[prop] > b[prop] ? 1 : a[prop] == b[prop] ? 0 : 0 != 0) { alert("ascSort: " + prop);}
            return a[prop]() < b[prop]() ? -1 : a[prop]() > b[prop]() ? 1 : a[prop]() == b[prop]() ? 0 : 0;
        };
        var descSort = function (a, b) {
           // if (a[prop] < b[prop] ? -1 : a[prop] > b[prop] ? 1 : a[prop] == b[prop] ? 0 : 0 != 0) { alert("descSort: " + prop); }
            return a[prop]() > b[prop]() ? -1 : a[prop]() < b[prop]() ? 1 : a[prop]() == b[prop]() ? 0 : 0;
        };
        var sortFunc = header.asc ? ascSort : descSort;

        self.activeSort(sortFunc);

        //store the new active sort function
        //self.activeSort(prop);
        //self.ascending(header.asc);
    };

    self.filteredInventory = ko.computed(function () {

        return ko.utils.arrayFilter(self.Inventory(), function (rec) {
            return (
                
                      (self.filterOrderItem == null || self.filterOrderItem() == false || self.filterOrderItem() == rec.master())
                        &&
                      (self.filterItem().length == 0 ||
                            rec.item().toLowerCase().indexOf(self.filterItem().toLowerCase()) > -1
                           || rec.item().toLowerCase().indexOf('added') > -1
                        )
                        &&
                      (self.filterInvQty() == null || self.filterInvQty()=="" || self.filterInvQty() == rec.qty())
                        && 
                      (self.filterBomQty() == null || self.filterBomQty() == false || self.filterBomQty() == (rec.bomQty() > 0) )
                       &&
                      (self.activeSort() != null) 
                   )
        }).sort(self.activeSort());

    });

    self.filteredBOM = ko.computed(function () {

        return ko.utils.arrayFilter(self.Inventory(), function (rec) {
            return (

                      (rec.bomQty() > 0)

                   )
        }).sort(self.activeSort());

    });

    self.filteredMasterItems = ko.computed(function () {
        return ko.utils.arrayFilter(self.MasterItems(), function (rec) {
            
            if (rec.toLowerCase().indexOf('add') > -1) {
                alert(rec);
            }

            return (self.filterItem().length == 0

                || rec.toLowerCase().indexOf(self.filterItem().toLowerCase()) > -1

               )

        });

    });

}

